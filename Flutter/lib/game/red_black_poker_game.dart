import 'dart:math' as math;
import 'dart:ui' as ui;

import 'package:flame/cache.dart';
import 'package:flame/components.dart';
import 'package:flame/events.dart';
import 'package:flame/game.dart';
import 'package:flutter/material.dart';

import 'model/auto_hold_advisor.dart';
import 'model/hand_rank.dart';
import 'model/poker_round.dart';
import 'model/red_black_session.dart';
import 'theme/game_theme.dart';

class RedBlackPokerGame extends FlameGame {
  final PokerRound round = PokerRound(
    startingCredits: 100,
    bonusTarget: 10,
    bonusPrize: 25,
  );

  final List<_CardView> _cardViews = <_CardView>[];
  final Map<int, ui.Image> _cardImages = <int, ui.Image>{};
  final List<bool> _revealed = List<bool>.filled(5, false);
  final List<ui.Image> _zombieHeadImages = <ui.Image>[];
  final List<ui.Image> _cardBackImages = <ui.Image>[];
  ui.Image? _titleBannerArt;
  ui.Image? _backgroundArt;
  ui.Image? _paytableArt;
  ui.Image? _bottomPanelArt;
  ui.Image? _controlPanelHeaderArt;

  final AutoHoldAdvisor _autoHoldAdvisor = const AutoHoldAdvisor();
  Duration cardDisplayDelay = const Duration(milliseconds: 275);
  Duration autoHoldInitialDelay = const Duration(milliseconds: 120);
  Duration autoHoldStepDelay = const Duration(milliseconds: 275);
  bool _isAnimatingCards = false;
  bool _redBlackMode = false;
  bool _controlPanelOpen = false;
  GameThemeId _selectedTheme = GameThemeId.halloween;
  int _handsPlayed = 0;
  int _handsWon = 0;
  int _redBlackWins = 0;
  final Map<HandRank, int> _handStats = <HandRank, int>{};
  RedBlackSession? _redBlackSession;
  int? _redBlackCardId;
  String _redBlackMessage = 'CHOOSE RED OR BLACK';

  double _uiTime = 0;
  int _cardBackVariant = 0;
  int _lastBonusProgress = 0;
  int _bonusFlareIndex = -1;
  double _bonusFlareTimer = 0;

  late final _GameButton _mainDrawButton;
  late final _GameButton _betDownButton;
  late final _GameButton _betUpButton;
  late final _GameButton _betMaxButton;
  late final _GameButton _cashOutButton;
  late final _GameButton _insertCoinsButton;
  late final _GameButton _refillWalletButton;
  late final _GameButton _doubleUpButton;
  late final _GameButton _collectButton;
  late final _GameButton _redButton;
  late final _GameButton _blackButton;
  late final _GameButton _rbCollectButton;
  late final _GameButton _settingsButton;
  late final _GameButton _panelCloseButton;
  late final _GameButton _themePrevButton;
  late final _GameButton _themeNextButton;
  late final _GameButton _backPrevButton;
  late final _GameButton _backNextButton;

  _CabinetGeometry get _geometry => _CabinetGeometry(size.x, size.y);

  @override
  Color backgroundColor() => const Color(0xFF07090D);

  @override
  void update(double dt) {
    super.update(dt);
    _uiTime += dt;
    if (_bonusFlareTimer > 0) {
      _bonusFlareTimer = math.max(0, _bonusFlareTimer - dt);
      if (_bonusFlareTimer == 0) _bonusFlareIndex = -1;
    }
  }

  @override
  Future<void> onLoad() async {
    await super.onLoad();
    images = Images(prefix: 'assets/');

    for (var id = 0; id <= 52; id++) {
      _cardImages[id] = await images.load('cards/' + id.toString() + '.png');
    }

    _titleBannerArt = await images.load('title_banner.jpg');
    _backgroundArt = await images.load('background_main.jpg');
    _paytableArt = await images.load('paytable_frame.jpg');
    _bottomPanelArt = await images.load('bottom_panel.jpg');
    _controlPanelHeaderArt = await images.load('control_panel_header.jpg');

    for (var i = 0; i < 10; i++) {
      _zombieHeadImages.add(
        await images.load('zombie_' + (i + 1).toString().padLeft(2, '0') + '.jpg'),
      );
    }
    for (var i = 0; i < 4; i++) {
      _cardBackImages.add(await images.load('card_back_' + i.toString() + '.jpg'));
    }

    for (var i = 0; i < 5; i++) {
      final view = _CardView(
        index: i,
        onTap: () {
          round.toggleHold(i);
          _syncView();
        },
      );
      _cardViews.add(view);
      await add(view);
    }

    _mainDrawButton = _GameButton(
      label: 'DRAW',
      accent: const Color(0xFFBD1722),
      onPressed: _mainDrawPressed,
    );
    _betDownButton = _GameButton(
      label: 'BET -',
      accent: const Color(0xFF8E4D0B),
      onPressed: () {
        round.changeBet(-1);
        _syncView();
      },
    );
    _betUpButton = _GameButton(
      label: 'BET +',
      accent: const Color(0xFFB46D0B),
      onPressed: () {
        round.changeBet(1);
        _syncView();
      },
    );
    _betMaxButton = _GameButton(
      label: 'BET MAX',
      accent: const Color(0xFFB46D0B),
      onPressed: () {
        if (round.phase != RoundPhase.idle &&
            round.phase != RoundPhase.result) {
          return;
        }
        while (round.bet < 5) {
          round.changeBet(1);
        }
        _syncView();
      },
    );
    _cashOutButton = _GameButton(
      label: 'CASH OUT',
      accent: const Color(0xFF6D4A1C),
      onPressed: () {
        round.cashOut();
        _syncView();
      },
    );
    _insertCoinsButton = _GameButton(
      label: 'INSERT COINS',
      accent: const Color(0xFF365A72),
      onPressed: () {
        round.insertCoins(10);
        _syncView();
      },
    );
    _refillWalletButton = _GameButton(
      label: 'REFILL WALLET',
      accent: const Color(0xFF7A244F),
      onPressed: () {
        round.refillWallet();
        _syncView();
      },
    );
    _doubleUpButton = _GameButton(
      label: 'DOUBLE UP',
      accent: const Color(0xFF6E1A91),
      onPressed: _startDoubleUp,
    );
    _collectButton = _GameButton(
      label: 'COLLECT',
      accent: const Color(0xFF207538),
      onPressed: _collectMainWin,
    );

    _redButton = _GameButton(
      label: 'RED',
      accent: const Color(0xFFC4162A),
      onPressed: () => _guessRedBlack(RedBlackChoice.red),
    );
    _blackButton = _GameButton(
      label: 'BLACK',
      accent: const Color(0xFF25262A),
      onPressed: () => _guessRedBlack(RedBlackChoice.black),
    );
    _rbCollectButton = _GameButton(
      label: 'COLLECT',
      accent: const Color(0xFF207538),
      onPressed: _collectRedBlack,
    );
    _settingsButton = _GameButton(
      label: '⚙',
      accent: const Color(0xFF463A5F),
      onPressed: () {
        _controlPanelOpen = true;
        _layout();
        _syncView();
      },
    );
    _panelCloseButton = _GameButton(
      label: 'X',
      accent: const Color(0xFF8A1F2D),
      onPressed: () {
        _controlPanelOpen = false;
        _layout();
        _syncView();
      },
    );
    _themePrevButton = _GameButton(
      label: '<',
      accent: const Color(0xFF4B4167),
      onPressed: () {
        _cycleTheme(-1);
        _syncView();
      },
    );
    _themeNextButton = _GameButton(
      label: '>',
      accent: const Color(0xFF4B4167),
      onPressed: () {
        _cycleTheme(1);
        _syncView();
      },
    );
    _backPrevButton = _GameButton(
      label: '<',
      accent: const Color(0xFF5E3B25),
      onPressed: () {
        _cardBackVariant = (_cardBackVariant + 3) % 4;
        _syncView();
      },
    );
    _backNextButton = _GameButton(
      label: '>',
      accent: const Color(0xFF5E3B25),
      onPressed: () {
        _cardBackVariant = (_cardBackVariant + 1) % 4;
        _syncView();
      },
    );

    await addAll(<Component>[
      _mainDrawButton,
      _betDownButton,
      _betUpButton,
      _betMaxButton,
      _cashOutButton,
      _insertCoinsButton,
      _refillWalletButton,
      _doubleUpButton,
      _collectButton,
      _redButton,
      _blackButton,
      _rbCollectButton,
      _settingsButton,
      _panelCloseButton,
      _themePrevButton,
      _themeNextButton,
      _backPrevButton,
      _backNextButton,
    ]);

    _layout();
    _syncView();
  }

  @override
  void onGameResize(Vector2 canvasSize) {
    super.onGameResize(canvasSize);
    if (isLoaded) {
      _layout();
      _syncView();
    }
  }

  void _layout() {
    if (_cardViews.isEmpty) return;
    if (_controlPanelOpen) {
      _layoutControlPanel();
    } else if (_redBlackMode) {
      _layoutRedBlack();
    } else {
      _layoutMainGame();
    }
  }

  void _layoutMainGame() {
    final g = _geometry;

    for (var i = 0; i < _cardViews.length; i++) {
      final rect = g.cardRects[i];
      _cardViews[i]
        ..size = Vector2(rect.width, rect.height)
        ..position = Vector2(rect.left, rect.top);
    }

    void place(_GameButton button, ui.Rect rect) {
      button
        ..position = Vector2(rect.left, rect.top)
        ..size = Vector2(rect.width, rect.height);
    }

    place(_betDownButton, g.betDownButton);
    place(_betUpButton, g.betUpButton);
    place(_betMaxButton, g.betMaxButton);
    place(_mainDrawButton, g.mainButton);
    place(_cashOutButton, g.cashOutButton);
    place(_insertCoinsButton, g.insertCoinsButton);
    place(_refillWalletButton, g.refillWalletButton);
    place(_doubleUpButton, g.doubleUpButton);
    place(_collectButton, g.collectButton);
    place(_settingsButton, g.settingsButton);

    _hideButton(_redButton);
    _hideButton(_blackButton);
    _hideButton(_rbCollectButton);
    _hideButton(_panelCloseButton);
    _hideButton(_themePrevButton);
    _hideButton(_themeNextButton);
    _hideButton(_backPrevButton);
    _hideButton(_backNextButton);
  }

  void _layoutControlPanel() {
    final g = _geometry;

    for (final card in _cardViews) {
      card
        ..enabled = false
        ..position = Vector2(-2000, -2000)
        ..size = Vector2.zero();
    }

    for (final button in <_GameButton>[
      _mainDrawButton,
      _betDownButton,
      _betUpButton,
      _betMaxButton,
      _cashOutButton,
      _insertCoinsButton,
      _refillWalletButton,
      _doubleUpButton,
      _collectButton,
      _redButton,
      _blackButton,
      _rbCollectButton,
      _settingsButton,
    ]) {
      _hideButton(button);
    }

    void place(_GameButton button, ui.Rect rect) {
      button
        ..position = Vector2(rect.left, rect.top)
        ..size = Vector2(rect.width, rect.height);
    }

    place(_panelCloseButton, g.panelCloseButton);
    place(_themePrevButton, g.themePrevButton);
    place(_themeNextButton, g.themeNextButton);
    place(_backPrevButton, g.backPrevButton);
    place(_backNextButton, g.backNextButton);
  }

  void _layoutRedBlack() {
    final w = size.x;
    final h = size.y;

    for (final card in _cardViews) {
      card
        ..position = Vector2(-2000, -2000)
        ..size = Vector2.zero();
    }

    _hideButton(_mainDrawButton);
    _hideButton(_betDownButton);
    _hideButton(_betUpButton);
    _hideButton(_betMaxButton);
    _hideButton(_cashOutButton);
    _hideButton(_insertCoinsButton);
    _hideButton(_refillWalletButton);
    _hideButton(_doubleUpButton);
    _hideButton(_collectButton);
    _hideButton(_settingsButton);
    _hideButton(_panelCloseButton);
    _hideButton(_themePrevButton);
    _hideButton(_themeNextButton);
    _hideButton(_backPrevButton);
    _hideButton(_backNextButton);

    final top = h * 0.70;
    final gap = w * 0.025;
    final bw = (w * 0.90 - gap * 2) / 3;
    final bh = h * 0.075;
    final x = w * 0.05;

    _redButton
      ..position = Vector2(x, top)
      ..size = Vector2(bw, bh);
    _blackButton
      ..position = Vector2(x + bw + gap, top)
      ..size = Vector2(bw, bh);
    _rbCollectButton
      ..position = Vector2(x + (bw + gap) * 2, top)
      ..size = Vector2(bw, bh);
  }

  void _cycleTheme(int delta) {
    final themes = <GameThemeId>[
      GameThemeId.halloween,
      GameThemeId.classic,
      GameThemeId.christmas,
    ];
    final current = themes.indexOf(_selectedTheme);
    _selectedTheme = themes[(current + delta + themes.length) % themes.length];
  }

  String get _themeName {
    switch (_selectedTheme) {
      case GameThemeId.halloween:
        return 'HALLOWEEN';
      case GameThemeId.classic:
        return 'CLASSIC';
      case GameThemeId.christmas:
        return 'CHRISTMAS';
    }
  }

  String get _cardBackName {
    const names = <String>[
      'HAUNTED CASTLE',
      'WITCH MOON',
      'JACK-O-LANTERN',
      'SKULL HARLEQUIN',
    ];
    return names[_cardBackVariant % names.length];
  }

  void _hideButton(_GameButton button) {
    button
      ..position = Vector2(-2000, -2000)
      ..size = Vector2.zero();
  }

  Future<void> _mainDrawPressed() async {
    if (_isAnimatingCards || _redBlackMode || _controlPanelOpen) return;
    if (round.canStartHand) {
      await _dealAnimated();
    } else if (round.canDrawReplacement) {
      await _drawAnimated();
    }
  }

  Future<void> _dealAnimated() async {
    if (_isAnimatingCards || !round.canStartHand) return;

    _isAnimatingCards = true;
    _cardBackVariant = (_cardBackVariant + 1) % 4;
    round.startHand();
    for (var i = 0; i < _revealed.length; i++) {
      _revealed[i] = false;
    }
    _syncView();

    for (var i = 0; i < round.cards.length; i++) {
      await Future<void>.delayed(cardDisplayDelay);
      _revealed[i] = true;
      _syncView();
    }

    await _applyAutoHold();

    _isAnimatingCards = false;
    _syncView();
  }

  Future<void> _applyAutoHold() async {
    if (round.cards.length != 5 || round.phase != RoundPhase.chooseHolds) return;

    final decision = _autoHoldAdvisor.recommend(round.cards);
    if (!decision.held.any((value) => value)) return;

    await Future<void>.delayed(autoHoldInitialDelay);

    for (var i = 0; i < decision.held.length; i++) {
      if (!decision.held[i]) continue;
      round.held[i] = true;
      _syncView();
      await Future<void>.delayed(autoHoldStepDelay);
    }
  }

  Future<void> _drawAnimated() async {
    if (_isAnimatingCards || !round.canDrawReplacement) return;

    _isAnimatingCards = true;
    final heldBeforeDraw = List<bool>.of(round.held);
    round.drawReplacement();

    for (var i = 0; i < _revealed.length; i++) {
      _revealed[i] = heldBeforeDraw[i];
    }
    _syncView();

    for (var i = 0; i < round.cards.length; i++) {
      if (heldBeforeDraw[i]) continue;
      await Future<void>.delayed(cardDisplayDelay);
      _revealed[i] = true;
      _syncView();
    }

    _handsPlayed += 1;
    if (round.result != HandRank.none) {
      _handsWon += 1;
      _handStats[round.result] = (_handStats[round.result] ?? 0) + 1;
    }

    _isAnimatingCards = false;
    _syncView();

    if (round.phase == RoundPhase.bonusReady) {
      await Future<void>.delayed(const Duration(milliseconds: 450));
      _startBonusSession();
    }
  }

  void _collectMainWin() {
    if (!round.hasPendingWin) return;
    round.collectWin();
    _syncView();
    _dealAnimated();
  }

  void _startDoubleUp() {
    if (!round.hasPendingWin) return;
    final stake = round.beginDoubleUp();
    _enterRedBlack(stake);
  }

  void _startBonusSession() {
    if (round.phase != RoundPhase.bonusReady) return;
    final stake = round.beginBonus();
    _enterRedBlack(stake);
  }

  void _enterRedBlack(int stake) {
    _redBlackSession = RedBlackSession(startingWin: stake);
    _redBlackCardId = null;
    _redBlackMessage = 'CHOOSE RED OR BLACK';
    _redBlackMode = true;
    _layout();
    _syncView();
  }

  Future<void> _guessRedBlack(RedBlackChoice choice) async {
    final session = _redBlackSession;
    if (!_redBlackMode || session == null || !session.canGuess) return;

    _redButton.enabled = false;
    _blackButton.enabled = false;

    final turn = session.guess(choice);
    _redBlackCardId = turn.cardId;

    if (turn.correct) {
      _redBlackWins += 1;
      _redBlackMessage = session.phase == RedBlackPhase.jackpot
          ? '64X JACKPOT!'
          : 'CORRECT — ' + session.currentWin.toString();
    } else {
      _redBlackMessage = 'BUST';
    }

    await Future<void>.delayed(const Duration(milliseconds: 777));

    if (session.phase == RedBlackPhase.busted) {
      round.finishDoubleUp(0);
      _leaveRedBlack();
    } else if (session.phase == RedBlackPhase.jackpot) {
      round.finishDoubleUp(session.currentWin);
      _leaveRedBlack();
    } else {
      _redBlackCardId = null;
      _redBlackMessage = 'RED OR BLACK?';
      _syncView();
    }
  }

  void _collectRedBlack() {
    final session = _redBlackSession;
    if (!_redBlackMode || session == null || !session.canCollect) return;
    final amount = session.collect();
    round.finishDoubleUp(amount);
    _leaveRedBlack();
  }

  void _leaveRedBlack() {
    _redBlackMode = false;
    _redBlackSession = null;
    _redBlackCardId = null;
    _layout();
    _syncView();
  }

  void _syncView() {
    if (_controlPanelOpen) {
      _panelCloseButton.enabled = true;
      _themePrevButton.enabled = true;
      _themeNextButton.enabled = true;
      _backPrevButton.enabled = true;
      _backNextButton.enabled = true;
      return;
    }
    if (_redBlackMode) {
      final session = _redBlackSession;
      _redButton.enabled = session?.canGuess ?? false;
      _blackButton.enabled = session?.canGuess ?? false;
      _rbCollectButton.enabled = session?.canCollect ?? false;
      _redButton.lit = _redButton.enabled;
      _blackButton.lit = _blackButton.enabled;
      _rbCollectButton.lit = _rbCollectButton.enabled;
      return;
    }

    for (var i = 0; i < _cardViews.length; i++) {
      final hasCard = i < round.cards.length;
      final winningMask = _winningCardMask();
      _cardViews[i]
        ..cardImage = hasCard && _revealed[i]
            ? _cardImages[round.cards[i].assetId]
            : null
        ..held = round.phase == RoundPhase.chooseHolds &&
            round.held[i] &&
            _revealed[i]
        ..winning = i < winningMask.length && winningMask[i] && _revealed[i]
        ..backVariant = _cardBackVariant
        ..backImage = _cardBackImages.isEmpty
            ? null
            : _cardBackImages[_cardBackVariant % _cardBackImages.length]
        ..enabled = round.canDrawReplacement && !_isAnimatingCards;
    }

    _mainDrawButton.enabled =
        round.canPressMainDraw && !_isAnimatingCards;
    _mainDrawButton.lit = _mainDrawButton.enabled;
    _mainDrawButton.label = round.canStartHand ? 'DEAL' : 'DRAW';
    _mainDrawButton.accent = round.canStartHand
        ? const Color(0xFF23853A)
        : const Color(0xFF175C8F);
    final moneyControls = !_isAnimatingCards && round.canAdjustMoney;
    _betDownButton.enabled = moneyControls && round.bet > 1;
    _betUpButton.enabled = moneyControls && round.bet < 5;
    _betMaxButton.enabled = moneyControls && round.bet < 5;
    _cashOutButton.enabled = !_isAnimatingCards && round.canCashOut;
    _insertCoinsButton.enabled = !_isAnimatingCards && round.canInsertCoins;
    _refillWalletButton.enabled = !_isAnimatingCards && round.canRefillWallet;
    _betDownButton.lit = _betDownButton.enabled;
    _betUpButton.lit = _betUpButton.enabled;
    _betMaxButton.lit = _betMaxButton.enabled;
    _cashOutButton.lit = _cashOutButton.enabled;
    _insertCoinsButton.lit = _insertCoinsButton.enabled;
    _refillWalletButton.lit = _refillWalletButton.enabled;

    final decision = round.phase == RoundPhase.winDecision && !_isAnimatingCards;
    _doubleUpButton.enabled = decision;
    _collectButton.enabled = decision;
    _doubleUpButton.lit = decision;
    _collectButton.lit = decision;

    if (round.bonusProgress > _lastBonusProgress) {
      _bonusFlareIndex = round.bonusProgress - 1;
      _bonusFlareTimer = 1.15;
    }
    _lastBonusProgress = round.bonusProgress;

    _layoutMainGame();
    if (round.canRefillWallet && !decision) {
      _hideButton(_cashOutButton);
      _hideButton(_insertCoinsButton);
    } else {
      _hideButton(_refillWalletButton);
    }
    if (!decision) {
      _hideButton(_doubleUpButton);
      _hideButton(_collectButton);
    } else {
      _hideButton(_mainDrawButton);
      _hideButton(_betDownButton);
      _hideButton(_betUpButton);
      _hideButton(_betMaxButton);
      _hideButton(_cashOutButton);
      _hideButton(_insertCoinsButton);
      _hideButton(_refillWalletButton);
    }
  }

  List<bool> _winningCardMask() {
    final mask = List<bool>.filled(5, false);
    if (round.cards.length != 5 || round.pendingWin <= 0) return mask;

    final cards = round.cards;
    final jokerIndexes = <int>[
      for (var i = 0; i < cards.length; i++)
        if (cards[i].isJoker) i,
    ];

    switch (round.result) {
      case HandRank.fiveOfAKind:
      case HandRank.royalFlush:
      case HandRank.straightFlush:
      case HandRank.fullHouse:
      case HandRank.flush:
      case HandRank.straight:
        return List<bool>.filled(5, true);

      case HandRank.fourOfAKind:
      case HandRank.threeOfAKind:
        final target = round.result == HandRank.fourOfAKind ? 4 : 3;
        final byRank = <int, List<int>>{};
        for (var i = 0; i < cards.length; i++) {
          if (cards[i].isJoker) continue;
          byRank.putIfAbsent(cards[i].rank, () => <int>[]).add(i);
        }

        List<int>? best;
        for (final indexes in byRank.values) {
          final combined = <int>[...indexes, ...jokerIndexes];
          if (combined.length >= target &&
              (best == null || combined.length > best.length)) {
            best = combined;
          }
        }

        if (best != null) {
          for (final i in best.take(target)) {
            mask[i] = true;
          }
        }
        return mask;

      case HandRank.twoPair:
        final byRank = <int, List<int>>{};
        for (var i = 0; i < cards.length; i++) {
          if (cards[i].isJoker) continue;
          byRank.putIfAbsent(cards[i].rank, () => <int>[]).add(i);
        }
        final pairs = byRank.values.where((v) => v.length == 2).take(2);
        for (final pair in pairs) {
          for (final i in pair) {
            mask[i] = true;
          }
        }
        for (final i in jokerIndexes) {
          if (mask.where((v) => v).length < 4) mask[i] = true;
        }
        return mask;

      case HandRank.none:
        return mask;
    }
  }

  @override
  void render(ui.Canvas canvas) {
    if (_controlPanelOpen) {
      _renderSettingsPanel(canvas);
    } else if (_redBlackMode) {
      _renderRedBlack(canvas);
    } else {
      _renderBackdrop(canvas);
      _renderZombieCabinet(canvas);
      _renderPayTable(canvas);
      _renderControlPanel(canvas);
      _renderStatus(canvas);
    }
    super.render(canvas);
  }

  void _renderSettingsPanel(ui.Canvas canvas) {
    final rect = ui.Rect.fromLTWH(0, 0, size.x, size.y);
    canvas.drawRect(rect, ui.Paint()..color = const Color(0xFF05070B));

    final g = _geometry;
    final panel = g.settingsPanel;
    canvas.drawRRect(
      ui.RRect.fromRectAndRadius(panel, const ui.Radius.circular(18)),
      ui.Paint()
        ..shader = ui.Gradient.linear(
          panel.topCenter,
          panel.bottomCenter,
          const <Color>[
            Color(0xFF18121A),
            Color(0xFF0A111A),
            Color(0xFF14090E),
          ],
          const <double>[0.0, 0.55, 1.0],
        ),
    );
    canvas.drawRRect(
      ui.RRect.fromRectAndRadius(panel, const ui.Radius.circular(18)),
      ui.Paint()
        ..style = ui.PaintingStyle.stroke
        ..strokeWidth = 4
        ..color = const Color(0xFF3A2618),
    );
    canvas.drawRRect(
      ui.RRect.fromRectAndRadius(panel.deflate(4), const ui.Radius.circular(15)),
      ui.Paint()
        ..style = ui.PaintingStyle.stroke
        ..strokeWidth = 1.4
        ..color = const Color(0xFFB37A34),
    );

    final header = ui.Rect.fromLTWH(
      panel.left + panel.width * 0.05,
      panel.top + panel.height * 0.025,
      panel.width * 0.90,
      panel.height * 0.11,
    );
    canvas.drawRRect(
      ui.RRect.fromRectAndRadius(header, const ui.Radius.circular(12)),
      ui.Paint()
        ..shader = ui.Gradient.linear(
          header.topCenter,
          header.bottomCenter,
          const <Color>[Color(0xFF321112), Color(0xFF14090B)],
        ),
    );
    canvas.drawRRect(
      ui.RRect.fromRectAndRadius(header, const ui.Radius.circular(12)),
      ui.Paint()
        ..style = ui.PaintingStyle.stroke
        ..strokeWidth = 1.6
        ..color = const Color(0xFF7B4B28),
    );

    if (_controlPanelHeaderArt != null) {
      final headerArtRect = ui.Rect.fromLTWH(
        panel.left + panel.width * 0.06,
        panel.top + panel.height * 0.025,
        panel.width * 0.88,
        panel.height * 0.105,
      );
      _drawImageCover(canvas, _controlPanelHeaderArt!, headerArtRect, opacity: 0.98);
    } else {
      _paintText(
        canvas,
        'CONTROL PANEL',
        g.settingsTitleCenter,
        fontSize: size.x * 0.060,
        color: const Color(0xFFFFD27A),
        weight: FontWeight.w900,
        centered: true,
      );
    }

    _paintText(
      canvas,
      'THEME',
      g.themeLabelCenter,
      fontSize: size.x * 0.032,
      color: const Color(0xFFE8D19C),
      weight: FontWeight.w800,
      centered: true,
    );
    _paintText(
      canvas,
      _themeName,
      g.themeValueCenter,
      fontSize: size.x * 0.046,
      color: const Color(0xFFFFFFFF),
      weight: FontWeight.w900,
      centered: true,
    );

    _paintText(
      canvas,
      'CARD BACK',
      g.cardBackLabelCenter,
      fontSize: size.x * 0.032,
      color: const Color(0xFFE8D19C),
      weight: FontWeight.w800,
      centered: true,
    );
    _paintText(
      canvas,
      _cardBackName,
      g.cardBackValueCenter,
      fontSize: size.x * 0.038,
      color: const Color(0xFFFFFFFF),
      weight: FontWeight.w900,
      centered: true,
    );

    _paintText(
      canvas,
      'STATISTICS',
      g.statisticsTitleCenter,
      fontSize: size.x * 0.038,
      color: const Color(0xFFFFD27A),
      weight: FontWeight.w900,
      centered: true,
    );

    final stats = <String, int>{
      'Hands': _handsPlayed,
      'Wins': _handsWon,
      'Two Pair': _handStats[HandRank.twoPair] ?? 0,
      'Three of a Kind': _handStats[HandRank.threeOfAKind] ?? 0,
      'Straights': _handStats[HandRank.straight] ?? 0,
      'Flushes': _handStats[HandRank.flush] ?? 0,
      'Full Houses': _handStats[HandRank.fullHouse] ?? 0,
      'Four of a Kind': _handStats[HandRank.fourOfAKind] ?? 0,
      'Straight Flushes': _handStats[HandRank.straightFlush] ?? 0,
      'Royal Flushes': _handStats[HandRank.royalFlush] ?? 0,
      'Five of a Kind': _handStats[HandRank.fiveOfAKind] ?? 0,
      'Red/Black Wins': _redBlackWins,
    };

    final left = g.statisticsLeft;
    var y = g.statisticsTop;
    for (final entry in stats.entries) {
      _paintText(
        canvas,
        entry.key,
        ui.Offset(left, y),
        fontSize: size.x * 0.026,
        color: const Color(0xFFE6CFA0),
        weight: FontWeight.w700,
      );
      _paintText(
        canvas,
        entry.value.toString(),
        ui.Offset(g.statisticsValueX, y),
        fontSize: size.x * 0.028,
        color: const Color(0xFFFF6A5E),
        weight: FontWeight.w900,
        centered: true,
      );
      y += g.statisticsRowGap;
    }
  }

  void _renderBackdrop(ui.Canvas canvas) {
    final g = _geometry;
    final rect = ui.Rect.fromLTWH(0, 0, size.x, size.y);

    canvas.drawRect(rect, ui.Paint()..color = const Color(0xFF030305));

    if (_backgroundArt != null) {
      _drawImageCover(
        canvas,
        _backgroundArt!,
        rect,
        opacity: 0.46,
      );
    }

    canvas.drawRect(
      rect,
      ui.Paint()
        ..shader = ui.Gradient.linear(
          rect.topCenter,
          rect.bottomCenter,
          const <Color>[
            Color(0x33000000),
            Color(0x6610060A),
            Color(0xCC030205),
          ],
          const <double>[0.0, 0.55, 1.0],
        ),
    );

    if (_titleBannerArt != null) {
      _drawImageContain(canvas, _titleBannerArt!, g.titleBanner);
    } else {
      _paintText(
        canvas,
        'RED BLACK POKER',
        g.titleBanner.center,
        fontSize: size.x * 0.070,
        color: const Color(0xFFFFC45C),
        weight: FontWeight.w900,
        centered: true,
      );
    }

    final railPaint = ui.Paint()..color = const Color(0xFF261713);
    canvas.drawRect(ui.Rect.fromLTWH(0, 0, size.x * 0.018, size.y), railPaint);
    canvas.drawRect(
      ui.Rect.fromLTWH(size.x * 0.982, 0, size.x * 0.018, size.y),
      railPaint,
    );

    if (_bottomPanelArt != null) {
      _drawImageCover(canvas, _bottomPanelArt!, g.bottomArt, opacity: 0.96);
      canvas.drawRRect(
        ui.RRect.fromRectAndRadius(g.bottomArt, const ui.Radius.circular(12)),
        ui.Paint()
          ..style = ui.PaintingStyle.stroke
          ..strokeWidth = 2
          ..color = const Color(0xFF8A5A2A),
      );
    }
  }

  void _renderZombieCabinet(ui.Canvas canvas) {
    final g = _geometry;
    final frame = g.zombieFrame;

    canvas.drawRRect(
      ui.RRect.fromRectAndRadius(frame, const ui.Radius.circular(11)),
      ui.Paint()..color = const Color(0xEE080709),
    );
    canvas.drawRRect(
      ui.RRect.fromRectAndRadius(frame, const ui.Radius.circular(11)),
      ui.Paint()
        ..style = ui.PaintingStyle.stroke
        ..strokeWidth = 2.2
        ..color = const Color(0xFF9A6331),
    );

    final gap = size.x * 0.006;
    final cellWidth = (frame.width - gap * 11) / 10;
    final cellHeight = frame.height - size.y * 0.017;

    for (var i = 0; i < 10; i++) {
      final cell = ui.Rect.fromLTWH(
        frame.left + gap + i * (cellWidth + gap),
        frame.top + size.y * 0.006,
        cellWidth,
        cellHeight,
      );
      final earned = i < round.bonusProgress;
      final flaring = _bonusFlareIndex == i && _bonusFlareTimer > 0;
      final flicker = 0.76 + 0.24 * math.sin(_uiTime * 9.0 + i * 1.7);

      canvas.drawRRect(
        ui.RRect.fromRectAndRadius(cell, const ui.Radius.circular(5)),
        ui.Paint()..color = const Color(0xFF050506),
      );

      if (i < _zombieHeadImages.length) {
        final artRect = ui.Rect.fromLTWH(
          cell.left + cell.width * 0.06,
          cell.top + cell.height * 0.04,
          cell.width * 0.88,
          cell.height * 0.76,
        );
        _drawImageCover(
          canvas,
          _zombieHeadImages[i],
          artRect,
          opacity: earned ? 1.0 : 0.26,
        );
      }

      if (earned) {
        final eyeGlow = ui.Paint()
          ..color = const Color(0xFFFF3A22)
              .withValues(alpha: flaring ? 0.68 : 0.28 * flicker)
          ..maskFilter =
              ui.MaskFilter.blur(ui.BlurStyle.normal, cell.width * 0.12);
        canvas.drawCircle(
          ui.Offset(cell.center.dx - cell.width * 0.11, cell.top + cell.height * 0.35),
          cell.width * 0.055,
          eyeGlow,
        );
        canvas.drawCircle(
          ui.Offset(cell.center.dx + cell.width * 0.11, cell.top + cell.height * 0.35),
          cell.width * 0.055,
          eyeGlow,
        );
      }

      final flameBase = ui.Offset(cell.center.dx, cell.bottom - cell.height * 0.10);
      final flameH = cell.height * (earned ? 0.18 + 0.03 * flicker : 0.045);
      final flameW = cell.width * (earned ? 0.16 : 0.07);
      final flame = ui.Path()
        ..moveTo(flameBase.dx, flameBase.dy - flameH)
        ..quadraticBezierTo(
          flameBase.dx + flameW,
          flameBase.dy - flameH * 0.45,
          flameBase.dx,
          flameBase.dy,
        )
        ..quadraticBezierTo(
          flameBase.dx - flameW,
          flameBase.dy - flameH * 0.45,
          flameBase.dx,
          flameBase.dy - flameH,
        )
        ..close();
      canvas.drawPath(
        flame,
        ui.Paint()
          ..color = earned
              ? const Color(0xFFFF781B)
              : const Color(0xFF4A2412),
      );

      _paintText(
        canvas,
        (i + 1).toString(),
        ui.Offset(cell.center.dx, cell.bottom - cell.height * 0.03),
        fontSize: cell.width * 0.18,
        color: earned ? const Color(0xFFFFD366) : const Color(0xFF755B42),
        weight: FontWeight.w900,
        centered: true,
      );

      canvas.drawRRect(
        ui.RRect.fromRectAndRadius(cell, const ui.Radius.circular(5)),
        ui.Paint()
          ..style = ui.PaintingStyle.stroke
          ..strokeWidth = flaring ? 2.0 : 1.0
          ..color = earned
              ? const Color(0xFFC2893D)
              : const Color(0xFF3B2A1D),
      );
    }
  }

  void _renderPayTable(ui.Canvas canvas) {
    final g = _geometry;
    final rect = g.payTable;
    final top = rect.top;
    final left = rect.left;
    final width = rect.width;
    final height = rect.height;

    if (_paytableArt != null) {
      _drawImageCover(canvas, _paytableArt!, rect, opacity: 0.96);
      final inner = rect.deflate(rect.width * 0.055);
      canvas.drawRRect(
        ui.RRect.fromRectAndRadius(inner, const ui.Radius.circular(5)),
        ui.Paint()..color = const Color(0xDDE0C58F),
      );
    } else {
      canvas.drawRRect(
        ui.RRect.fromRectAndRadius(rect, const ui.Radius.circular(8)),
        ui.Paint()..color = const Color(0xDD08090B),
      );
    }

    const rows = <HandRank>[
      HandRank.fiveOfAKind,
      HandRank.royalFlush,
      HandRank.straightFlush,
      HandRank.fourOfAKind,
      HandRank.fullHouse,
      HandRank.flush,
      HandRank.straight,
      HandRank.threeOfAKind,
      HandRank.twoPair,
    ];

    final rowHeight = height / 5.2;
    for (var i = 0; i < rows.length; i++) {
      final column = i < 5 ? 0 : 1;
      final row = i < 5 ? i : i - 5;
      final x = left + width * (column == 0 ? 0.04 : 0.56);
      final y = top + rowHeight * (row + 0.75);

      _paintText(
        canvas,
        rows[i].label,
        ui.Offset(x, y),
        fontSize: size.x * 0.026,
        color: const Color(0xFF2A160D),
        weight: FontWeight.w800,
      );

      _paintText(
        canvas,
        (rows[i].basePayout * round.bet).toString(),
        ui.Offset(x + width * 0.35, y),
        fontSize: size.x * 0.026,
        color: const Color(0xFFB31F16),
        weight: FontWeight.w900,
        centered: true,
      );
    }

    _paintText(
      canvas,
      'HIGH PAIR J / Q / K / A  →  ZOMBIE HEAD',
      ui.Offset(size.x * 0.5, top + height - size.y * 0.007),
      fontSize: size.x * 0.024,
      color: const Color(0xFF4D2C13),
      weight: FontWeight.w700,
      centered: true,
    );
  }

  void _renderControlPanel(ui.Canvas canvas) {
    final g = _geometry;
    final panel = g.controlPanel;

    canvas.drawRRect(
      ui.RRect.fromRectAndRadius(panel, const ui.Radius.circular(16)),
      ui.Paint()
        ..shader = ui.Gradient.linear(
          panel.topCenter,
          panel.bottomCenter,
          const <Color>[
            Color(0xFF2A0B0B),
            Color(0xFF14070A),
            Color(0xFF070608),
          ],
          const <double>[0.0, 0.52, 1.0],
        ),
    );
    canvas.drawRRect(
      ui.RRect.fromRectAndRadius(panel, const ui.Radius.circular(16)),
      ui.Paint()
        ..style = ui.PaintingStyle.stroke
        ..strokeWidth = 3.2
        ..color = const Color(0xFF3A2415),
    );
    canvas.drawRRect(
      ui.RRect.fromRectAndRadius(panel.deflate(3.5), const ui.Radius.circular(13)),
      ui.Paint()
        ..style = ui.PaintingStyle.stroke
        ..strokeWidth = 1.3
        ..color = const Color(0xFF9C652F),
    );

    for (final corner in <ui.Offset>[
      ui.Offset(panel.left + 9, panel.top + 9),
      ui.Offset(panel.right - 9, panel.top + 9),
      ui.Offset(panel.left + 9, panel.bottom - 9),
      ui.Offset(panel.right - 9, panel.bottom - 9),
    ]) {
      canvas.drawCircle(
        corner,
        3.2,
        ui.Paint()..color = const Color(0xFFB6844B),
      );
      canvas.drawCircle(
        corner,
        1.2,
        ui.Paint()..color = const Color(0xFF25160E),
      );
    }

    final walletRect = g.walletStrip;
    canvas.drawRRect(
      ui.RRect.fromRectAndRadius(walletRect, const ui.Radius.circular(7)),
      ui.Paint()..color = const Color(0xD9060709),
    );

    _paintText(
      canvas,
      'BANK  ' + round.bank.toString(),
      ui.Offset(walletRect.left + walletRect.width * 0.18, walletRect.center.dy),
      fontSize: size.x * 0.022,
      color: const Color(0xFFC9A96A),
      weight: FontWeight.w800,
      centered: true,
    );
    _paintText(
      canvas,
      r'$ WALLET $  ' + round.wallet.toString(),
      ui.Offset(walletRect.center.dx, walletRect.center.dy),
      fontSize: size.x * 0.025,
      color: const Color(0xFFFFD66F),
      weight: FontWeight.w900,
      centered: true,
    );
    _paintText(
      canvas,
      'MACHINE  ' + round.credits.toString(),
      ui.Offset(walletRect.right - walletRect.width * 0.18, walletRect.center.dy),
      fontSize: size.x * 0.022,
      color: const Color(0xFFC9A96A),
      weight: FontWeight.w800,
      centered: true,
    );
  }

  void _renderStatus(ui.Canvas canvas) {
    final g = _geometry;
    final top = g.statusTop;
    final panelHeight = g.statusPanels.first.height;
    final labels = <String>['BET', 'CREDITS', 'WIN'];
    final values = <String>[
      round.bet.toString(),
      round.credits.toString(),
      round.pendingWin.toString(),
    ];

    for (var i = 0; i < 3; i++) {
      final rect = g.statusPanels[i];
      canvas.drawRRect(
        ui.RRect.fromRectAndRadius(rect, const ui.Radius.circular(7)),
        ui.Paint()..color = const Color(0xE608090A),
      );
      canvas.drawRRect(
        ui.RRect.fromRectAndRadius(rect, const ui.Radius.circular(7)),
        ui.Paint()
          ..style = ui.PaintingStyle.stroke
          ..strokeWidth = 1.5
          ..color = const Color(0xFF8B6334),
      );

      _paintText(
        canvas,
        labels[i],
        ui.Offset(rect.center.dx, rect.top + panelHeight * 0.28),
        fontSize: size.x * 0.024,
        color: const Color(0xFF2A160D),
        weight: FontWeight.w800,
        centered: true,
      );
      _paintText(
        canvas,
        values[i],
        ui.Offset(rect.center.dx, rect.top + panelHeight * 0.68),
        fontSize: size.x * 0.041,
        color: i == 0 ? const Color(0xFFFF4747) : const Color(0xFFF1D86D),
        weight: FontWeight.w900,
        centered: true,
      );
    }

    final message = switch (round.phase) {
      RoundPhase.idle => 'PRESS DRAW',
      RoundPhase.chooseHolds => 'SELECT CARDS TO HOLD — PRESS DRAW',
      RoundPhase.result => round.result == HandRank.none
          ? 'HAND OVER — NO WIN'
          : 'HAND OVER',
      RoundPhase.winDecision => round.result.label + ' — DOUBLE UP OR COLLECT',
      RoundPhase.doubleUp => 'RED OR BLACK',
      RoundPhase.bonusReady => '10 ZOMBIE HEADS — BONUS!',
    };

    _paintText(
      canvas,
      message,
      ui.Offset(size.x * 0.5, g.statusMessageY),
      fontSize: size.x * 0.027,
      color: round.phase == RoundPhase.bonusReady
          ? const Color(0xFF9CFF65)
          : const Color(0xFFE5C78F),
      weight: FontWeight.w800,
      centered: true,
    );
  }

  void _renderRedBlack(ui.Canvas canvas) {
    final rect = ui.Rect.fromLTWH(0, 0, size.x, size.y);
    final gradient = ui.Gradient.radial(
      ui.Offset(size.x * 0.5, size.y * 0.36),
      size.x * 0.8,
      const <Color>[
        Color(0xFF5B101A),
        Color(0xFF151218),
        Color(0xFF050506),
      ],
      const <double>[0.0, 0.58, 1.0],
    );
    canvas.drawRect(rect, ui.Paint()..shader = gradient);

    _paintText(
      canvas,
      'RED OR BLACK',
      ui.Offset(size.x * 0.5, size.y * 0.08),
      fontSize: size.x * 0.10,
      color: const Color(0xFFF0C67A),
      weight: FontWeight.w900,
      centered: true,
    );

    final session = _redBlackSession;
    if (session == null) return;

    _paintText(
      canvas,
      'WIN',
      ui.Offset(size.x * 0.5, size.y * 0.18),
      fontSize: size.x * 0.05,
      color: const Color(0xFFE8D19C),
      weight: FontWeight.w800,
      centered: true,
    );
    _paintText(
      canvas,
      session.currentWin.toString(),
      ui.Offset(size.x * 0.5, size.y * 0.235),
      fontSize: size.x * 0.11,
      color: const Color(0xFFFFFFFF),
      weight: FontWeight.w900,
      centered: true,
    );

    const multipliers = <String>['2X', '4X', '8X', '16X', '32X', '64X'];

    final historyTop = size.y * 0.305;
    final historyGap = size.x * 0.012;
    final historyCardW = (size.x * 0.90 - historyGap * 5) / 6;
    final historyCardH = historyCardW * 1.42;
    final historyStartX = size.x * 0.05;

    for (var i = 0; i < 6; i++) {
      final x = historyStartX + i * (historyCardW + historyGap);
      final rect = ui.Rect.fromLTWH(
        x,
        historyTop + size.y * 0.028,
        historyCardW,
        historyCardH,
      );

      _paintText(
        canvas,
        multipliers[i],
        ui.Offset(rect.center.dx, historyTop),
        fontSize: size.x * 0.032,
        color: i < session.roundsWon
            ? const Color(0xFFFF4D4D)
            : const Color(0xFF8B7D72),
        weight: FontWeight.w800,
        centered: true,
      );

      if (i < session.history.length &&
          session.history[i].correct &&
          _cardImages[session.history[i].cardId] != null) {
        final image = _cardImages[session.history[i].cardId]!;
        canvas.drawImageRect(
          image,
          ui.Rect.fromLTWH(
            0,
            0,
            image.width.toDouble(),
            image.height.toDouble(),
          ),
          rect,
          ui.Paint()..filterQuality = ui.FilterQuality.high,
        );
      } else {
        canvas.drawRRect(
          ui.RRect.fromRectAndRadius(rect, const ui.Radius.circular(7)),
          ui.Paint()..color = const Color(0xFF111216),
        );
        canvas.drawRRect(
          ui.RRect.fromRectAndRadius(rect, const ui.Radius.circular(7)),
          ui.Paint()
            ..style = ui.PaintingStyle.stroke
            ..strokeWidth = 1.5
            ..color = i < session.roundsWon
                ? const Color(0xFFB28347)
                : const Color(0xFF4D4640),
        );
      }
    }

    final cardW = size.x * 0.25;
    final cardH = cardW * 1.42;
    final cardRect = ui.Rect.fromLTWH(
      size.x * 0.5 - cardW * 0.5,
      size.y * 0.50,
      cardW,
      cardH,
    );

    final id = _redBlackCardId;
    if (id != null && _cardImages[id] != null) {
      final image = _cardImages[id]!;
      canvas.drawImageRect(
        image,
        ui.Rect.fromLTWH(
          0,
          0,
          image.width.toDouble(),
          image.height.toDouble(),
        ),
        cardRect,
        ui.Paint()..filterQuality = ui.FilterQuality.high,
      );
    } else {
      canvas.drawRRect(
        ui.RRect.fromRectAndRadius(cardRect, const ui.Radius.circular(12)),
        ui.Paint()..color = const Color(0xFF17181B),
      );
      canvas.drawRRect(
        ui.RRect.fromRectAndRadius(cardRect, const ui.Radius.circular(12)),
        ui.Paint()
          ..style = ui.PaintingStyle.stroke
          ..strokeWidth = 3
          ..color = const Color(0xFFB28347),
      );
      _paintText(
        canvas,
        '?',
        cardRect.center,
        fontSize: size.x * 0.18,
        color: const Color(0xFFD9B36B),
        weight: FontWeight.w900,
        centered: true,
      );
    }

    _paintText(
      canvas,
      _redBlackMessage,
      ui.Offset(size.x * 0.5, size.y * 0.665),
      fontSize: size.x * 0.045,
      color: const Color(0xFFF0D39A),
      weight: FontWeight.w900,
      centered: true,
    );
  }

  void _drawImageCover(
    ui.Canvas canvas,
    ui.Image image,
    ui.Rect dst, {
    double opacity = 1.0,
  }) {
    final sw = image.width.toDouble();
    final sh = image.height.toDouble();
    final srcAspect = sw / sh;
    final dstAspect = dst.width / dst.height;
    late ui.Rect src;
    if (srcAspect > dstAspect) {
      final cropW = sh * dstAspect;
      src = ui.Rect.fromLTWH((sw - cropW) / 2, 0, cropW, sh);
    } else {
      final cropH = sw / dstAspect;
      src = ui.Rect.fromLTWH(0, (sh - cropH) / 2, sw, cropH);
    }
    canvas.drawImageRect(
      image,
      src,
      dst,
      ui.Paint()
        ..filterQuality = ui.FilterQuality.high
        ..color = Colors.white.withValues(alpha: opacity),
    );
  }

  void _drawImageContain(
    ui.Canvas canvas,
    ui.Image image,
    ui.Rect dst, {
    double opacity = 1.0,
  }) {
    final sw = image.width.toDouble();
    final sh = image.height.toDouble();
    final scale = math.min(dst.width / sw, dst.height / sh);
    final w = sw * scale;
    final h = sh * scale;
    final fitted = ui.Rect.fromLTWH(
      dst.center.dx - w / 2,
      dst.center.dy - h / 2,
      w,
      h,
    );
    canvas.drawImageRect(
      image,
      ui.Rect.fromLTWH(0, 0, sw, sh),
      fitted,
      ui.Paint()
        ..filterQuality = ui.FilterQuality.high
        ..color = Colors.white.withValues(alpha: opacity),
    );
  }

  void _paintText(
    ui.Canvas canvas,
    String text,
    ui.Offset position, {
    required double fontSize,
    required Color color,
    FontWeight weight = FontWeight.w500,
    bool centered = false,
  }) {
    final painter = TextPainter(
      text: TextSpan(
        text: text,
        style: TextStyle(
          color: color,
          fontSize: fontSize,
          fontWeight: weight,
          height: 1,
        ),
      ),
      textDirection: TextDirection.ltr,
      maxLines: 1,
    )..layout(maxWidth: size.x * 0.9);

    final offset = centered
        ? ui.Offset(position.dx - painter.width / 2, position.dy - painter.height / 2)
        : ui.Offset(position.dx, position.dy - painter.height / 2);
    painter.paint(canvas, offset);
  }
}

class _CabinetGeometry {
  _CabinetGeometry(this.w, this.h);

  final double w;
  final double h;

  double get side => w * 0.04;
  double get gap => (w * 0.012).clamp(4.0, 12.0);
  double get buttonGap => w * 0.014;

  ui.Rect get titleBanner =>
      ui.Rect.fromLTWH(w * 0.025, h * 0.012, w * 0.95, h * 0.19);

  ui.Rect get zombieFrame =>
      ui.Rect.fromLTWH(w * 0.035, h * 0.205, w * 0.93, h * 0.095);

  ui.Rect get payTable =>
      ui.Rect.fromLTWH(w * 0.055, h * 0.307, w * 0.89, h * 0.145);

  double get cardTop => payTable.bottom + h * 0.018;
  double get cardWidth => ((w * 0.94) - gap * 4) / 5;
  double get cardHeight => cardWidth * 1.42;

  List<ui.Rect> get cardRects {
    final total = cardWidth * 5 + gap * 4;
    final startX = (w - total) / 2;
    return List<ui.Rect>.generate(
      5,
      (i) => ui.Rect.fromLTWH(
        startX + i * (cardWidth + gap),
        cardTop,
        cardWidth,
        cardHeight,
      ),
    );
  }

  double get statusTop => cardTop + cardHeight + h * 0.022;
  double get statusMessageY => statusTop - h * 0.012;

  List<ui.Rect> get statusPanels {
    final width = w * 0.29;
    final height = h * 0.065;
    return List<ui.Rect>.generate(
      3,
      (i) => ui.Rect.fromLTWH(
        w * 0.04 + i * (width + w * 0.025),
        statusTop,
        width,
        height,
      ),
    );
  }

  double get controlsTop => statusTop + h * 0.072;
  double get buttonHeight => h * 0.052;
  double get smallButtonWidth =>
      (w - side * 2 - buttonGap * 3) / 4;

  ui.Rect get betDownButton =>
      ui.Rect.fromLTWH(side, controlsTop, smallButtonWidth, buttonHeight);
  ui.Rect get betUpButton => ui.Rect.fromLTWH(
        side + smallButtonWidth + buttonGap,
        controlsTop,
        smallButtonWidth,
        buttonHeight,
      );
  ui.Rect get betMaxButton => ui.Rect.fromLTWH(
        side + (smallButtonWidth + buttonGap) * 2,
        controlsTop,
        smallButtonWidth,
        buttonHeight,
      );
  ui.Rect get mainButton => ui.Rect.fromLTWH(
        side + (smallButtonWidth + buttonGap) * 3,
        controlsTop,
        smallButtonWidth,
        buttonHeight,
      );

  double get lowerTop => controlsTop + buttonHeight + h * 0.012;
  double get lowerButtonWidth =>
      (w - side * 2 - buttonGap) / 2;

  ui.Rect get cashOutButton =>
      ui.Rect.fromLTWH(side, lowerTop, lowerButtonWidth, buttonHeight);
  ui.Rect get insertCoinsButton => ui.Rect.fromLTWH(
        side + lowerButtonWidth + buttonGap,
        lowerTop,
        lowerButtonWidth,
        buttonHeight,
      );
  ui.Rect get refillWalletButton =>
      ui.Rect.fromLTWH(side, lowerTop, w - side * 2, buttonHeight);

  double get decisionButtonWidth => w * 0.30;
  double get decisionGap => w * 0.025;
  double get decisionStart =>
      w * 0.5 - (decisionButtonWidth * 2 + decisionGap) * 0.5;

  ui.Rect get doubleUpButton => ui.Rect.fromLTWH(
        decisionStart,
        controlsTop,
        decisionButtonWidth,
        buttonHeight,
      );
  ui.Rect get collectButton => ui.Rect.fromLTWH(
        decisionStart + decisionButtonWidth + decisionGap,
        controlsTop,
        decisionButtonWidth,
        buttonHeight,
      );

  ui.Rect get settingsButton =>
      ui.Rect.fromLTWH(w * 0.885, h * 0.012, w * 0.08, h * 0.04);

  ui.Rect get controlPanel =>
      ui.Rect.fromLTWH(w * 0.025, statusTop + h * 0.064, w * 0.95, h * 0.172);

  ui.Rect get bottomArt =>
      ui.Rect.fromLTWH(w * 0.035, h * 0.805, w * 0.93, h * 0.13);

  ui.Rect get walletStrip => ui.Rect.fromLTWH(
        controlPanel.left + w * 0.03,
        controlPanel.bottom - h * 0.042,
        controlPanel.width - w * 0.06,
        h * 0.034,
      );

  ui.Rect get settingsPanel =>
      ui.Rect.fromLTWH(w * 0.07, h * 0.07, w * 0.86, h * 0.80);

  ui.Rect get panelCloseButton =>
      ui.Rect.fromLTWH(settingsPanel.right - w * 0.085, settingsPanel.top + h * 0.018, w * 0.07, h * 0.042);

  ui.Rect get themePrevButton =>
      ui.Rect.fromLTWH(settingsPanel.left + w * 0.045, h * 0.25, w * 0.10, h * 0.052);
  ui.Rect get themeNextButton =>
      ui.Rect.fromLTWH(settingsPanel.right - w * 0.145, h * 0.25, w * 0.10, h * 0.052);
  ui.Rect get backPrevButton =>
      ui.Rect.fromLTWH(settingsPanel.left + w * 0.045, h * 0.39, w * 0.10, h * 0.052);
  ui.Rect get backNextButton =>
      ui.Rect.fromLTWH(settingsPanel.right - w * 0.145, h * 0.39, w * 0.10, h * 0.052);

  ui.Offset get settingsTitleCenter =>
      ui.Offset(settingsPanel.center.dx, settingsPanel.top + h * 0.065);
  ui.Offset get themeLabelCenter =>
      ui.Offset(settingsPanel.center.dx, h * 0.21);
  ui.Offset get themeValueCenter =>
      ui.Offset(settingsPanel.center.dx, h * 0.278);
  ui.Offset get cardBackLabelCenter =>
      ui.Offset(settingsPanel.center.dx, h * 0.35);
  ui.Offset get cardBackValueCenter =>
      ui.Offset(settingsPanel.center.dx, h * 0.418);
  ui.Offset get statisticsTitleCenter =>
      ui.Offset(settingsPanel.center.dx, h * 0.50);

  double get statisticsLeft => settingsPanel.left + w * 0.085;
  double get statisticsValueX => settingsPanel.right - w * 0.085;
  double get statisticsTop => h * 0.55;
  double get statisticsRowGap => h * 0.027;
}

class _CardView extends PositionComponent with TapCallbacks {
  _CardView({
    required this.index,
    required this.onTap,
  });

  final int index;
  final VoidCallback onTap;
  ui.Image? cardImage;
  ui.Image? backImage;
  bool held = false;
  bool winning = false;
  bool enabled = false;
  int backVariant = 0;

  @override
  void onTapUp(TapUpEvent event) {
    if (enabled) onTap();
  }

  @override
  void render(ui.Canvas canvas) {
    super.render(canvas);
    final rect = ui.Rect.fromLTWH(0, 0, size.x, size.y);
    canvas.drawRRect(
      ui.RRect.fromRectAndRadius(rect, ui.Radius.circular(size.x * 0.055)),
      ui.Paint()..color = const Color(0xFFF4F0E6),
    );

    if (cardImage != null) {
      canvas.drawImageRect(
        cardImage!,
        ui.Rect.fromLTWH(
          0,
          0,
          cardImage!.width.toDouble(),
          cardImage!.height.toDouble(),
        ),
        rect,
        ui.Paint()..filterQuality = ui.FilterQuality.high,
      );
    } else if (backImage != null) {
      canvas.drawImageRect(
        backImage!,
        ui.Rect.fromLTWH(
          0,
          0,
          backImage!.width.toDouble(),
          backImage!.height.toDouble(),
        ),
        rect,
        ui.Paint()..filterQuality = ui.FilterQuality.high,
      );
    } else {
      _renderHalloweenBack(canvas, rect);
    }

    if (held || winning) {
      final holdRect = ui.Rect.fromLTWH(0, size.y * 0.79, size.x, size.y * 0.21);
      canvas.drawRect(holdRect, ui.Paint()..color = const Color(0xE80B0B0D));
      final painter = TextPainter(
        text: TextSpan(
          text: winning ? 'WIN' : 'HOLD',
          style: TextStyle(
            color: winning ? const Color(0xFFFF4D4D) : const Color(0xFFFFC85A),
            fontWeight: FontWeight.w900,
            fontSize: size.x * 0.16,
          ),
        ),
        textAlign: TextAlign.center,
        textDirection: TextDirection.ltr,
      )..layout(maxWidth: size.x);
      painter.paint(
        canvas,
        ui.Offset((size.x - painter.width) / 2, size.y * 0.84 - painter.height / 2),
      );
    }
  }

  void _renderHalloweenBack(ui.Canvas canvas, ui.Rect rect) {
    final palettes = <List<Color>>[
      const [Color(0xFF07182C), Color(0xFF173B66), Color(0xFF8CC8FF)],
      const [Color(0xFF170B25), Color(0xFF4A1765), Color(0xFFC66BFF)],
      const [Color(0xFF241208), Color(0xFF7B2D0B), Color(0xFFFF8B25)],
      const [Color(0xFF240708), Color(0xFF681114), Color(0xFFFF493D)],
    ];
    final colors = palettes[backVariant % palettes.length];
    final inner = rect.deflate(size.x * 0.035);
    final radius = ui.Radius.circular(size.x * 0.045);

    canvas.drawRRect(
      ui.RRect.fromRectAndRadius(inner, radius),
      ui.Paint()
        ..shader = ui.Gradient.linear(
          inner.topCenter,
          inner.bottomCenter,
          <Color>[colors[1], colors[0]],
        ),
    );
    canvas.drawRRect(
      ui.RRect.fromRectAndRadius(inner, radius),
      ui.Paint()
        ..style = ui.PaintingStyle.stroke
        ..strokeWidth = math.max(1.4, size.x * 0.025)
        ..color = const Color(0xFFB98B48),
    );

    final moon = ui.Offset(inner.center.dx, inner.top + inner.height * 0.24);
    canvas.drawCircle(
      moon,
      inner.width * 0.18,
      ui.Paint()..color = colors[2].withValues(alpha: 0.85),
    );

    if (backVariant % 4 == 0) {
      final castle = ui.Path()
        ..moveTo(inner.left + inner.width * 0.22, inner.bottom - inner.height * 0.28)
        ..lineTo(inner.left + inner.width * 0.22, inner.top + inner.height * 0.43)
        ..lineTo(inner.left + inner.width * 0.36, inner.top + inner.height * 0.35)
        ..lineTo(inner.left + inner.width * 0.46, inner.top + inner.height * 0.44)
        ..lineTo(inner.left + inner.width * 0.55, inner.top + inner.height * 0.31)
        ..lineTo(inner.left + inner.width * 0.68, inner.top + inner.height * 0.42)
        ..lineTo(inner.left + inner.width * 0.78, inner.top + inner.height * 0.37)
        ..lineTo(inner.left + inner.width * 0.78, inner.bottom - inner.height * 0.28)
        ..close();
      canvas.drawPath(castle, ui.Paint()..color = const Color(0xE8000000));
    } else if (backVariant % 4 == 1) {
      final hat = ui.Path()
        ..moveTo(inner.center.dx - inner.width * 0.26, inner.top + inner.height * 0.48)
        ..lineTo(inner.center.dx + inner.width * 0.24, inner.top + inner.height * 0.48)
        ..lineTo(inner.center.dx + inner.width * 0.02, inner.top + inner.height * 0.23)
        ..close();
      canvas.drawPath(hat, ui.Paint()..color = const Color(0xE8000000));
      canvas.drawOval(
        ui.Rect.fromCenter(
          center: ui.Offset(inner.center.dx, inner.top + inner.height * 0.49),
          width: inner.width * 0.64,
          height: inner.height * 0.07,
        ),
        ui.Paint()..color = const Color(0xE8000000),
      );
    } else if (backVariant % 4 == 2) {
      canvas.drawOval(
        ui.Rect.fromCenter(
          center: ui.Offset(inner.center.dx, inner.top + inner.height * 0.48),
          width: inner.width * 0.52,
          height: inner.height * 0.36,
        ),
        ui.Paint()..color = const Color(0xFFFF781A),
      );
      final eyePaint = ui.Paint()..color = const Color(0xFF1B0B04);
      canvas.drawOval(
        ui.Rect.fromCenter(
          center: ui.Offset(inner.center.dx - inner.width * 0.10, inner.top + inner.height * 0.45),
          width: inner.width * 0.09,
          height: inner.height * 0.055,
        ),
        eyePaint,
      );
      canvas.drawOval(
        ui.Rect.fromCenter(
          center: ui.Offset(inner.center.dx + inner.width * 0.10, inner.top + inner.height * 0.45),
          width: inner.width * 0.09,
          height: inner.height * 0.055,
        ),
        eyePaint,
      );
    } else {
      canvas.drawCircle(
        ui.Offset(inner.center.dx, inner.top + inner.height * 0.46),
        inner.width * 0.20,
        ui.Paint()..color = const Color(0xFFD1C2A2),
      );
      canvas.drawRect(
        ui.Rect.fromCenter(
          center: ui.Offset(inner.center.dx, inner.top + inner.height * 0.60),
          width: inner.width * 0.24,
          height: inner.height * 0.13,
        ),
        ui.Paint()..color = const Color(0xFFD1C2A2),
      );
      final eyePaint = ui.Paint()..color = const Color(0xFF2A0808);
      canvas.drawCircle(
        ui.Offset(inner.center.dx - inner.width * 0.08, inner.top + inner.height * 0.44),
        inner.width * 0.045,
        eyePaint,
      );
      canvas.drawCircle(
        ui.Offset(inner.center.dx + inner.width * 0.08, inner.top + inner.height * 0.44),
        inner.width * 0.045,
        eyePaint,
      );
    }

    final painter = TextPainter(
      text: TextSpan(
        children: <InlineSpan>[
          TextSpan(
            text: 'RED ',
            style: TextStyle(
              color: const Color(0xFFFF473F),
              fontWeight: FontWeight.w900,
              fontSize: size.x * 0.14,
            ),
          ),
          TextSpan(
            text: 'BLACK\nPOKER',
            style: TextStyle(
              color: const Color(0xFFFFD27A),
              fontWeight: FontWeight.w900,
              fontSize: size.x * 0.14,
            ),
          ),
        ],
      ),
      textAlign: TextAlign.center,
      textDirection: TextDirection.ltr,
    )..layout(maxWidth: inner.width * 0.82);
    painter.paint(
      canvas,
      ui.Offset(
        inner.center.dx - painter.width / 2,
        inner.bottom - inner.height * 0.29,
      ),
    );
  }
}

class _GameButton extends PositionComponent with TapCallbacks {
  _GameButton({
    required this.label,
    required this.accent,
    required this.onPressed,
  });

  String label;
  Color accent;
  final VoidCallback onPressed;
  bool enabled = true;
  bool lit = false;
  bool _pressed = false;
  double _time = 0;

  @override
  void update(double dt) {
    super.update(dt);
    _time += dt;
  }

  @override
  void onTapDown(TapDownEvent event) {
    if (enabled) _pressed = true;
  }

  @override
  void onTapCancel(TapCancelEvent event) {
    _pressed = false;
  }

  @override
  void onTapUp(TapUpEvent event) {
    if (!enabled) return;
    _pressed = false;
    onPressed();
  }

  @override
  void render(ui.Canvas canvas) {
    super.render(canvas);
    if (size.x <= 0 || size.y <= 0) return;

    final press = _pressed ? size.y * 0.11 : 0.0;
    final flicker = 0.84 + 0.16 * math.sin(_time * 10.5 + size.x * 0.025);

    canvas.save();
    canvas.translate(0, press);

    final full = ui.Rect.fromLTWH(0, 0, size.x, size.y - press);
    final outer = full.deflate(size.x * 0.018);
    final bezel = outer.deflate(size.x * 0.022);
    final face = bezel.deflate(size.x * 0.018);

    // Button shadow / physical depth.
    final shadowRect = ui.Rect.fromLTWH(
      outer.left,
      outer.top + size.y * 0.08,
      outer.width,
      outer.height,
    );
    canvas.drawRRect(
      ui.RRect.fromRectAndRadius(shadowRect, const ui.Radius.circular(11)),
      ui.Paint()..color = const Color(0xFF020203),
    );

    // Outer forged-metal bezel.
    canvas.drawRRect(
      ui.RRect.fromRectAndRadius(outer, const ui.Radius.circular(11)),
      ui.Paint()
        ..shader = ui.Gradient.linear(
          outer.topCenter,
          outer.bottomCenter,
          const <Color>[
            Color(0xFF6B4A2C),
            Color(0xFF24160E),
            Color(0xFF8A6338),
          ],
          const <double>[0.0, 0.58, 1.0],
        ),
    );
    canvas.drawRRect(
      ui.RRect.fromRectAndRadius(outer, const ui.Radius.circular(11)),
      ui.Paint()
        ..style = ui.PaintingStyle.stroke
        ..strokeWidth = 1.4
        ..color = const Color(0xFFB88A4C),
    );

    final activeAccent = !enabled
        ? const Color(0xFF29292C)
        : _pressed
            ? Color.lerp(accent, Colors.black, 0.30)!
            : accent;

    if (enabled && lit) {
      canvas.drawRRect(
        ui.RRect.fromRectAndRadius(bezel.inflate(size.x * 0.018),
            const ui.Radius.circular(10)),
        ui.Paint()
          ..color = accent.withValues(alpha: 0.28 * flicker)
          ..maskFilter =
              ui.MaskFilter.blur(ui.BlurStyle.normal, size.x * 0.065),
      );
    }

    // Inner black gasket.
    canvas.drawRRect(
      ui.RRect.fromRectAndRadius(bezel, const ui.Radius.circular(9)),
      ui.Paint()..color = const Color(0xFF08080A),
    );

    // Illuminated glass/button face.
    canvas.drawRRect(
      ui.RRect.fromRectAndRadius(face, const ui.Radius.circular(8)),
      ui.Paint()
        ..shader = ui.Gradient.linear(
          face.topCenter,
          face.bottomCenter,
          <Color>[
            Color.lerp(activeAccent, Colors.white, enabled ? 0.18 : 0.03)!,
            activeAccent,
            Color.lerp(activeAccent, Colors.black, 0.45)!,
          ],
          const <double>[0.0, 0.47, 1.0],
        ),
    );

    // Top glass reflection.
    canvas.drawRRect(
      ui.RRect.fromRectAndRadius(
        ui.Rect.fromLTWH(
          face.left + face.width * 0.08,
          face.top + face.height * 0.08,
          face.width * 0.84,
          face.height * 0.18,
        ),
        const ui.Radius.circular(8),
      ),
      ui.Paint()
        ..color = Colors.white.withValues(alpha: enabled ? 0.12 : 0.035),
    );

    canvas.drawRRect(
      ui.RRect.fromRectAndRadius(face, const ui.Radius.circular(8)),
      ui.Paint()
        ..style = ui.PaintingStyle.stroke
        ..strokeWidth = _pressed ? 1.0 : 1.6
        ..color = enabled
            ? (lit ? const Color(0xFFFFD36D) : const Color(0xFFB27B3A))
            : const Color(0xFF4C4C50),
    );

    // Small lamp indicator.
    if (enabled) {
      final lamp = ui.Offset(face.left + face.width * 0.10, face.center.dy);
      canvas.drawCircle(
        lamp,
        size.x * 0.028,
        ui.Paint()
          ..color = lit
              ? const Color(0xFFFF7B1A).withValues(alpha: 0.30 * flicker)
              : const Color(0xFF2C1C12)
          ..maskFilter = lit
              ? ui.MaskFilter.blur(ui.BlurStyle.normal, size.x * 0.035)
              : null,
      );
      canvas.drawCircle(
        lamp,
        size.x * 0.013,
        ui.Paint()
          ..color =
              lit ? const Color(0xFFFFD15A) : const Color(0xFF6B3A17),
      );
    }

    final painter = TextPainter(
      text: TextSpan(
        text: label,
        style: TextStyle(
          color: enabled
              ? const Color(0xFFFFEDC5)
              : const Color(0xFF757579),
          fontWeight: FontWeight.w900,
          fontSize: size.x * (label.length > 10 ? 0.105 : label.length > 7 ? 0.125 : 0.155),
          letterSpacing: 0.3,
          shadows: enabled && lit
              ? <Shadow>[
                  Shadow(
                    color: accent.withValues(alpha: 0.75 * flicker),
                    blurRadius: size.x * 0.07,
                  ),
                ]
              : null,
        ),
      ),
      textAlign: TextAlign.center,
      textDirection: TextDirection.ltr,
      maxLines: 1,
    )..layout(maxWidth: face.width * 0.78);

    painter.paint(
      canvas,
      ui.Offset(
        face.center.dx - painter.width / 2 + (enabled ? face.width * 0.025 : 0),
        face.center.dy - painter.height / 2,
      ),
    );

    canvas.restore();
  }
}
