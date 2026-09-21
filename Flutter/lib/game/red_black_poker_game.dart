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

class RedBlackPokerGame extends FlameGame {
  final PokerRound round = PokerRound(
    startingCredits: 100,
    bonusTarget: 10,
    bonusPrize: 25,
  );

  final List<_CardView> _cardViews = <_CardView>[];
  final Map<int, ui.Image> _cardImages = <int, ui.Image>{};
  final List<bool> _revealed = List<bool>.filled(5, false);

  final AutoHoldAdvisor _autoHoldAdvisor = const AutoHoldAdvisor();
  Duration cardDisplayDelay = const Duration(milliseconds: 275);
  Duration autoHoldInitialDelay = const Duration(milliseconds: 120);
  Duration autoHoldStepDelay = const Duration(milliseconds: 275);
  bool _isAnimatingCards = false;
  bool _redBlackMode = false;
  RedBlackSession? _redBlackSession;
  int? _redBlackCardId;
  String _redBlackMessage = 'CHOOSE RED OR BLACK';

  late final _GameButton _mainDrawButton;
  late final _GameButton _betOneButton;
  late final _GameButton _betMaxButton;
  late final _GameButton _doubleUpButton;
  late final _GameButton _collectButton;
  late final _GameButton _redButton;
  late final _GameButton _blackButton;
  late final _GameButton _rbCollectButton;

  @override
  Color backgroundColor() => const Color(0xFF07090D);

  @override
  Future<void> onLoad() async {
    await super.onLoad();
    images = Images(prefix: 'assets/');

    for (var id = 0; id <= 52; id++) {
      _cardImages[id] = await images.load('cards/' + id.toString() + '.png');
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
    _betOneButton = _GameButton(
      label: 'BET ONE',
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

    await addAll(<Component>[
      _mainDrawButton,
      _betOneButton,
      _betMaxButton,
      _doubleUpButton,
      _collectButton,
      _redButton,
      _blackButton,
      _rbCollectButton,
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
    if (_redBlackMode) {
      _layoutRedBlack();
    } else {
      _layoutMainGame();
    }
  }

  void _layoutMainGame() {
    final w = size.x;
    final h = size.y;
    final cabinetTop = h * 0.055;
    final cabinetHeight = h * 0.115;
    final payTop = cabinetTop + cabinetHeight + h * 0.016;
    final payHeight = h * 0.16;
    final cardTop = payTop + payHeight + h * 0.025;

    final gap = (w * 0.012).clamp(4.0, 12.0);
    final cardWidth = ((w * 0.94) - gap * 4) / 5;
    final cardHeight = cardWidth * 1.42;
    final total = cardWidth * 5 + gap * 4;
    final startX = (w - total) / 2;

    for (var i = 0; i < _cardViews.length; i++) {
      _cardViews[i]
        ..size = Vector2(cardWidth, cardHeight)
        ..position = Vector2(startX + i * (cardWidth + gap), cardTop);
    }

    final statusTop = cardTop + cardHeight + h * 0.022;
    final buttonTop = statusTop + h * 0.088;
    final sidePad = w * 0.04;
    final gapButtons = w * 0.018;
    final normalButtonWidth =
        (w - sidePad * 2 - gapButtons * 2) / 3;
    final buttonHeight = h * 0.065;

    _betOneButton
      ..position = Vector2(sidePad, buttonTop)
      ..size = Vector2(normalButtonWidth, buttonHeight);
    _betMaxButton
      ..position = Vector2(
        sidePad + normalButtonWidth + gapButtons,
        buttonTop,
      )
      ..size = Vector2(normalButtonWidth, buttonHeight);
    _mainDrawButton
      ..position = Vector2(
        sidePad + (normalButtonWidth + gapButtons) * 2,
        buttonTop,
      )
      ..size = Vector2(normalButtonWidth, buttonHeight);

    final decisionButtonWidth = w * 0.24;
    final decisionGap = w * 0.025;
    final decisionStart =
        w * 0.5 - (decisionButtonWidth * 2 + decisionGap) * 0.5;

    _doubleUpButton
      ..position = Vector2(decisionStart, buttonTop)
      ..size = Vector2(decisionButtonWidth, buttonHeight);
    _collectButton
      ..position = Vector2(
        decisionStart + decisionButtonWidth + decisionGap,
        buttonTop,
      )
      ..size = Vector2(decisionButtonWidth, buttonHeight);

    _hideButton(_redButton);
    _hideButton(_blackButton);
    _hideButton(_rbCollectButton);
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
    _hideButton(_betOneButton);
    _hideButton(_betMaxButton);
    _hideButton(_doubleUpButton);
    _hideButton(_collectButton);

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

  void _hideButton(_GameButton button) {
    button
      ..position = Vector2(-2000, -2000)
      ..size = Vector2.zero();
  }

  Future<void> _mainDrawPressed() async {
    if (_isAnimatingCards || _redBlackMode) return;
    if (round.canStartHand) {
      await _dealAnimated();
    } else if (round.canDrawReplacement) {
      await _drawAnimated();
    }
  }

  Future<void> _dealAnimated() async {
    if (_isAnimatingCards || !round.canStartHand) return;

    _isAnimatingCards = true;
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
    if (_redBlackMode) {
      final session = _redBlackSession;
      _redButton.enabled = session?.canGuess ?? false;
      _blackButton.enabled = session?.canGuess ?? false;
      _rbCollectButton.enabled = session?.canCollect ?? false;
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
        ..enabled = round.canDrawReplacement && !_isAnimatingCards;
    }

    _mainDrawButton.enabled =
        round.canPressMainDraw && !_isAnimatingCards;
    _betOneButton.enabled = !_isAnimatingCards &&
        (round.phase == RoundPhase.idle || round.phase == RoundPhase.result);
    _betMaxButton.enabled = _betOneButton.enabled;

    final decision = round.phase == RoundPhase.winDecision && !_isAnimatingCards;
    _doubleUpButton.enabled = decision;
    _collectButton.enabled = decision;

    if (!decision) {
      _hideButton(_doubleUpButton);
      _hideButton(_collectButton);
    } else {
      _layoutMainGame();
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
    if (_redBlackMode) {
      _renderRedBlack(canvas);
    } else {
      _renderBackdrop(canvas);
      _renderZombieCabinet(canvas);
      _renderPayTable(canvas);
      _renderStatus(canvas);
    }
    super.render(canvas);
  }

  void _renderBackdrop(ui.Canvas canvas) {
    final rect = ui.Rect.fromLTWH(0, 0, size.x, size.y);
    final gradient = ui.Gradient.linear(
      ui.Offset(size.x * 0.5, 0),
      ui.Offset(size.x * 0.5, size.y),
      const <Color>[
        Color(0xFF06101F),
        Color(0xFF151018),
        Color(0xFF09090D),
      ],
      const <double>[0.0, 0.52, 1.0],
    );
    canvas.drawRect(rect, ui.Paint()..shader = gradient);

    _paintText(
      canvas,
      'RED BLACK POKER',
      ui.Offset(size.x * 0.5, size.y * 0.018),
      fontSize: size.x * 0.072,
      color: const Color(0xFFE9B96E),
      weight: FontWeight.w900,
      centered: true,
    );
  }

  void _renderZombieCabinet(ui.Canvas canvas) {
    final top = size.y * 0.055;
    final height = size.y * 0.115;
    final left = size.x * 0.025;
    final width = size.x * 0.95;
    final frame = ui.Rect.fromLTWH(left, top, width, height);

    canvas.drawRRect(
      ui.RRect.fromRectAndRadius(frame, const ui.Radius.circular(10)),
      ui.Paint()..color = const Color(0xFF18191B),
    );
    canvas.drawRRect(
      ui.RRect.fromRectAndRadius(frame, const ui.Radius.circular(10)),
      ui.Paint()
        ..style = ui.PaintingStyle.stroke
        ..strokeWidth = 2
        ..color = const Color(0xFF8B6334),
    );

    final gap = size.x * 0.006;
    final cellWidth = (width - gap * 11) / 10;
    final cellHeight = height - size.y * 0.025;

    for (var i = 0; i < 10; i++) {
      final x = left + gap + i * (cellWidth + gap);
      final cell = ui.Rect.fromLTWH(x, top + gap, cellWidth, cellHeight);
      final earned = i < round.bonusProgress;

      canvas.drawRRect(
        ui.RRect.fromRectAndRadius(cell, const ui.Radius.circular(5)),
        ui.Paint()
          ..color = earned ? const Color(0xFF314F26) : const Color(0xFF08090A),
      );

      if (earned) {
        canvas.drawCircle(
          cell.center.translate(0, -cell.height * 0.06),
          cell.width * 0.23,
          ui.Paint()..color = const Color(0xFF81A85D),
        );
        canvas.drawCircle(
          cell.center.translate(-cell.width * 0.075, -cell.height * 0.08),
          cell.width * 0.035,
          ui.Paint()..color = const Color(0xFFFF6A23),
        );
        canvas.drawCircle(
          cell.center.translate(cell.width * 0.075, -cell.height * 0.08),
          cell.width * 0.035,
          ui.Paint()..color = const Color(0xFFFF6A23),
        );
      }

      _paintText(
        canvas,
        (i + 1).toString(),
        ui.Offset(cell.center.dx, cell.bottom - cell.height * 0.12),
        fontSize: cell.width * 0.22,
        color: const Color(0xFFE6C185),
        weight: FontWeight.w700,
        centered: true,
      );
    }
  }

  void _renderPayTable(ui.Canvas canvas) {
    final top = size.y * 0.186;
    final left = size.x * 0.06;
    final width = size.x * 0.88;
    final height = size.y * 0.16;

    final rect = ui.Rect.fromLTWH(left, top, width, height);
    canvas.drawRRect(
      ui.RRect.fromRectAndRadius(rect, const ui.Radius.circular(8)),
      ui.Paint()..color = const Color(0xDD08090B),
    );

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
        color: const Color(0xFFE8D19C),
        weight: FontWeight.w700,
      );

      _paintText(
        canvas,
        (rows[i].basePayout * round.bet).toString(),
        ui.Offset(x + width * 0.35, y),
        fontSize: size.x * 0.026,
        color: const Color(0xFFFF4A43),
        weight: FontWeight.w800,
        centered: true,
      );
    }

    _paintText(
      canvas,
      'HIGH PAIR J / Q / K / A  →  ZOMBIE HEAD',
      ui.Offset(size.x * 0.5, top + height - size.y * 0.007),
      fontSize: size.x * 0.024,
      color: const Color(0xFFB6DB7B),
      weight: FontWeight.w700,
      centered: true,
    );
  }

  void _renderStatus(ui.Canvas canvas) {
    final cardTop = size.y * 0.371;
    final gap = (size.x * 0.012).clamp(4.0, 12.0);
    final cardWidth = ((size.x * 0.94) - gap * 4) / 5;
    final cardHeight = cardWidth * 1.42;
    final top = cardTop + cardHeight + size.y * 0.022;

    final panelWidth = size.x * 0.29;
    final panelHeight = size.y * 0.065;
    final labels = <String>['BET', 'CREDITS', 'WIN'];
    final values = <String>[
      round.bet.toString(),
      round.credits.toString(),
      round.pendingWin.toString(),
    ];

    for (var i = 0; i < 3; i++) {
      final x = size.x * 0.04 + i * (panelWidth + size.x * 0.025);
      final rect = ui.Rect.fromLTWH(x, top, panelWidth, panelHeight);
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
        color: const Color(0xFFE8D19C),
        weight: FontWeight.w700,
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
      ui.Offset(size.x * 0.5, top - size.y * 0.012),
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
    for (var i = 0; i < multipliers.length; i++) {
      final active = i < session.roundsWon;
      final x = size.x * (0.12 + i * 0.152);
      _paintText(
        canvas,
        multipliers[i],
        ui.Offset(x, size.y * 0.315),
        fontSize: size.x * 0.034,
        color: active ? const Color(0xFFFF4D4D) : const Color(0xFF8B7D72),
        weight: FontWeight.w800,
        centered: true,
      );
    }

    final cardW = size.x * 0.31;
    final cardH = cardW * 1.42;
    final cardRect = ui.Rect.fromLTWH(
      size.x * 0.5 - cardW * 0.5,
      size.y * 0.38,
      cardW,
      cardH,
    );

    final id = _redBlackCardId;
    if (id != null && _cardImages[id] != null) {
      final image = _cardImages[id]!;
      canvas.drawImageRect(
        image,
        ui.Rect.fromLTWH(0, 0, image.width.toDouble(), image.height.toDouble()),
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
        fontSize: size.x * 0.22,
        color: const Color(0xFFD9B36B),
        weight: FontWeight.w900,
        centered: true,
      );
    }

    _paintText(
      canvas,
      _redBlackMessage,
      ui.Offset(size.x * 0.5, size.y * 0.64),
      fontSize: size.x * 0.047,
      color: const Color(0xFFF0D39A),
      weight: FontWeight.w900,
      centered: true,
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

class _CardView extends PositionComponent with TapCallbacks {
  _CardView({
    required this.index,
    required this.onTap,
  });

  final int index;
  final VoidCallback onTap;
  ui.Image? cardImage;
  bool held = false;
  bool winning = false;
  bool enabled = false;

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
    } else {
      final painter = TextPainter(
        text: TextSpan(
          text: '♠',
          style: TextStyle(
            color: const Color(0xFF1B1A1A),
            fontWeight: FontWeight.w900,
            fontSize: size.x * 0.38,
          ),
        ),
        textAlign: TextAlign.center,
        textDirection: TextDirection.ltr,
      )..layout(maxWidth: size.x);
      painter.paint(
        canvas,
        ui.Offset((size.x - painter.width) / 2, (size.y - painter.height) / 2),
      );
    }

    if (held) {
      final holdRect = ui.Rect.fromLTWH(0, size.y * 0.79, size.x, size.y * 0.21);
      canvas.drawRect(holdRect, ui.Paint()..color = const Color(0xE80B0B0D));
      final painter = TextPainter(
        text: TextSpan(
          text: 'HOLD',
          style: TextStyle(
            color: const Color(0xFFFFC85A),
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

    if (winning) {
      final winRect = ui.Rect.fromLTWH(0, size.y * 0.79, size.x, size.y * 0.21);
      canvas.drawRect(winRect, ui.Paint()..color = const Color(0xE80B0B0D));
      final painter = TextPainter(
        text: TextSpan(
          text: 'WIN',
          style: TextStyle(
            color: const Color(0xFFFF4D4D),
            fontWeight: FontWeight.w900,
            fontSize: size.x * 0.17,
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
}

class _GameButton extends PositionComponent with TapCallbacks {
  _GameButton({
    required this.label,
    required this.accent,
    required this.onPressed,
  });

  final String label;
  final Color accent;
  final VoidCallback onPressed;
  bool enabled = true;
  bool _pressed = false;

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

    final rect = ui.Rect.fromLTWH(0, 0, size.x, size.y);
    final color = !enabled
        ? const Color(0xFF222326)
        : _pressed
            ? Color.lerp(accent, Colors.black, 0.28)!
            : accent;

    canvas.drawRRect(
      ui.RRect.fromRectAndRadius(rect, const ui.Radius.circular(9)),
      ui.Paint()..color = color,
    );
    canvas.drawRRect(
      ui.RRect.fromRectAndRadius(rect, const ui.Radius.circular(9)),
      ui.Paint()
        ..style = ui.PaintingStyle.stroke
        ..strokeWidth = 2
        ..color = enabled
            ? const Color(0xFFE6B660)
            : const Color(0xFF55565A),
    );

    final painter = TextPainter(
      text: TextSpan(
        text: label,
        style: TextStyle(
          color: enabled ? const Color(0xFFFFE7B0) : const Color(0xFF77787C),
          fontWeight: FontWeight.w900,
          fontSize: size.x * (label.length > 7 ? 0.12 : 0.17),
        ),
      ),
      textAlign: TextAlign.center,
      textDirection: TextDirection.ltr,
      maxLines: 1,
    )..layout(maxWidth: size.x * 0.9);

    painter.paint(
      canvas,
      ui.Offset((size.x - painter.width) / 2, (size.y - painter.height) / 2),
    );
  }
}
