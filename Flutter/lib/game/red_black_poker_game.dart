import 'dart:ui' as ui;

import 'package:flame/components.dart';
import 'package:flame/events.dart';
import 'package:flame/game.dart';
import 'package:flutter/material.dart';

import 'model/hand_rank.dart';
import 'model/poker_round.dart';

class RedBlackPokerGame extends FlameGame {
  final PokerRound round = PokerRound(startingCredits: 100, bonusTarget: 10);

  final List<_CardView> _cardViews = <_CardView>[];
  final Map<int, ui.Image> _cardImages = <int, ui.Image>{};

  late final _GameButton _dealButton;
  late final _GameButton _drawButton;
  late final _GameButton _betOneButton;
  late final _GameButton _betMaxButton;

  @override
  Color backgroundColor() => const Color(0xFF07090D);

  @override
  Future<void> onLoad() async {
    await super.onLoad();

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

    _dealButton = _GameButton(
      label: 'DEAL',
      accent: const Color(0xFF28B84B),
      onPressed: () {
        round.deal();
        _syncView();
      },
    );

    _drawButton = _GameButton(
      label: 'DRAW',
      accent: const Color(0xFFBD1722),
      onPressed: () {
        round.draw();
        _syncView();
      },
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
        if (round.phase == RoundPhase.chooseHolds ||
            round.phase == RoundPhase.bonusReady) {
          return;
        }
        while (round.bet < 5) {
          round.changeBet(1);
        }
        _syncView();
      },
    );

    await addAll(<Component>[
      _dealButton,
      _drawButton,
      _betOneButton,
      _betMaxButton,
    ]);

    _layout();
    _syncView();
  }

  @override
  void onGameResize(Vector2 canvasSize) {
    super.onGameResize(canvasSize);
    if (isLoaded) _layout();
  }

  void _layout() {
    if (_cardViews.isEmpty) return;

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
    final smallButtonWidth = w * 0.205;
    final bigButtonWidth = w * 0.27;
    final buttonHeight = h * 0.065;

    _betOneButton
      ..position = Vector2(sidePad, buttonTop)
      ..size = Vector2(smallButtonWidth, buttonHeight);

    _betMaxButton
      ..position = Vector2(sidePad + smallButtonWidth + w * 0.018, buttonTop)
      ..size = Vector2(smallButtonWidth, buttonHeight);

    _dealButton
      ..position = Vector2(w * 0.50 - bigButtonWidth * 0.5, buttonTop)
      ..size = Vector2(bigButtonWidth, buttonHeight);

    _drawButton
      ..position = Vector2(w - sidePad - smallButtonWidth, buttonTop)
      ..size = Vector2(smallButtonWidth, buttonHeight);
  }

  void _syncView() {
    for (var i = 0; i < _cardViews.length; i++) {
      final hasCard = i < round.cards.length;
      _cardViews[i]
        ..cardImage = hasCard ? _cardImages[round.cards[i].assetId] : null
        ..held = round.held[i]
        ..enabled = round.canDraw;
    }

    _dealButton.enabled = round.canDeal;
    _drawButton.enabled = round.canDraw;
    _betOneButton.enabled =
        round.phase != RoundPhase.chooseHolds && round.phase != RoundPhase.bonusReady;
    _betMaxButton.enabled = _betOneButton.enabled;
  }

  @override
  void render(ui.Canvas canvas) {
    _renderBackdrop(canvas);
    _renderZombieCabinet(canvas);
    _renderPayTable(canvas);
    _renderStatus(canvas);
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
    );
    canvas.drawRect(rect, ui.Paint()..shader = gradient);

    final moon = ui.Offset(size.x * 0.72, size.y * 0.06);
    canvas.drawCircle(
      moon,
      size.x * 0.10,
      ui.Paint()..color = const Color(0x224FA9FF),
    );

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
      'JACKS OR BETTER  →  ZOMBIE HEAD',
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
      round.lastWin.toString(),
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
      RoundPhase.idle => 'PRESS DEAL',
      RoundPhase.chooseHolds => 'SELECT CARDS TO HOLD',
      RoundPhase.result => round.result == HandRank.none
          ? 'NO WIN'
          : round.result.label + '  +' + round.lastWin.toString(),
      RoundPhase.bonusReady => '10 ZOMBIE HEADS — RED OR BLACK BONUS!',
    };

    _paintText(
      canvas,
      message,
      ui.Offset(size.x * 0.5, top - size.y * 0.012),
      fontSize: size.x * 0.028,
      color: round.phase == RoundPhase.bonusReady
          ? const Color(0xFF9CFF65)
          : const Color(0xFFE5C78F),
      weight: FontWeight.w800,
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
    )..layout(maxWidth: size.x * 0.45);

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
        ui.Offset(
          (size.x - painter.width) / 2,
          (size.y - painter.height) / 2,
        ),
      );
    }

    if (held) {
      final holdRect =
          ui.Rect.fromLTWH(0, size.y * 0.79, size.x, size.y * 0.21);
      canvas.drawRect(
        holdRect,
        ui.Paint()..color = const Color(0xE80B0B0D),
      );
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
        ui.Offset(
          (size.x - painter.width) / 2,
          size.y * 0.84 - painter.height / 2,
        ),
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
          fontSize: size.x * (label.length > 5 ? 0.15 : 0.20),
        ),
      ),
      textAlign: TextAlign.center,
      textDirection: TextDirection.ltr,
      maxLines: 1,
    )..layout(maxWidth: size.x * 0.9);

    painter.paint(
      canvas,
      ui.Offset(
        (size.x - painter.width) / 2,
        (size.y - painter.height) / 2,
      ),
    );
  }
}
