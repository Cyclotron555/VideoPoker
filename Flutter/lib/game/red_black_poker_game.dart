import 'dart:ui';

import 'package:flame/components.dart';
import 'package:flame/events.dart';
import 'package:flame/game.dart';
import 'package:flutter/material.dart';

import 'model/poker_round.dart';

class RedBlackPokerGame extends FlameGame {
  final PokerRound round = PokerRound();
  final List<_CardView> _cardViews = <_CardView>[];

  @override
  Color backgroundColor() => const Color(0xFF08130E);

  @override
  Future<void> onLoad() async {
    await super.onLoad();

    for (var i = 0; i < 5; i++) {
      final view = _CardView(
        index: i,
        onTap: () {
          round.toggleHold(i);
          _syncCards();
        },
      );
      _cardViews.add(view);
      await add(view);
    }

    _layoutCards();
  }

  @override
  void onGameResize(Vector2 canvasSize) {
    super.onGameResize(canvasSize);
    if (isLoaded) _layoutCards();
  }

  void _layoutCards() {
    if (_cardViews.isEmpty) return;

    final gap = (size.x * 0.012).clamp(6.0, 16.0);
    final width = ((size.x * 0.94) - gap * 4) / 5;
    final cardWidth = width.clamp(70.0, 220.0);
    final cardHeight = cardWidth * 1.42;
    final total = cardWidth * 5 + gap * 4;
    final x = (size.x - total) / 2;
    final y = (size.y * 0.30).clamp(120.0, 280.0);

    for (var i = 0; i < _cardViews.length; i++) {
      _cardViews[i]
        ..size = Vector2(cardWidth, cardHeight)
        ..position = Vector2(x + i * (cardWidth + gap), y);
    }
  }

  void _syncCards() {
    for (var i = 0; i < _cardViews.length; i++) {
      _cardViews[i]
        ..cardAssetPath = i < round.cards.length ? round.cards[i].assetPath : null
        ..held = round.held[i];
    }
  }
}

class _CardView extends PositionComponent with TapCallbacks {
  _CardView({
    required this.index,
    required this.onTap,
  });

  final int index;
  final VoidCallback onTap;

  String? cardAssetPath;
  bool held = false;

  @override
  void onTapUp(TapUpEvent event) => onTap();

  @override
  void render(Canvas canvas) {
    super.render(canvas);

    // Temporary shell only. Original PNG card faces are now in assets/cards/
    // and will replace this placeholder when the animated deal component lands.
    final rect = Rect.fromLTWH(0, 0, size.x, size.y);
    canvas.drawRRect(
      RRect.fromRectAndRadius(rect, Radius.circular(size.x * .07)),
      Paint()..color = const Color(0xFFF8F5EA),
    );

    final painter = TextPainter(
      text: TextSpan(
        text: cardAssetPath ?? 'RED\nBLACK',
        style: TextStyle(
          color: const Color(0xFF111111),
          fontWeight: FontWeight.w800,
          fontSize: size.x * .10,
        ),
      ),
      textAlign: TextAlign.center,
      textDirection: TextDirection.ltr,
    )..layout(maxWidth: size.x * .9);

    painter.paint(
      canvas,
      Offset(
        (size.x - painter.width) / 2,
        (size.y - painter.height) / 2,
      ),
    );

    if (held) {
      final holdRect = Rect.fromLTWH(0, size.y * .82, size.x, size.y * .18);
      canvas.drawRect(
        holdRect,
        Paint()..color = const Color(0xFFC1122F),
      );
    }
  }
}
