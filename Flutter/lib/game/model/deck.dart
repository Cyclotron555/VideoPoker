import 'dart:math';

import 'playing_card.dart';

class Deck {
  Deck({Random? random})
      : _random = random ?? Random.secure(),
        _fixedDrawOrder = null {
    reset();
  }

  Deck.fixed(List<PlayingCard> drawOrder)
      : assert(drawOrder.isNotEmpty),
        _random = Random(0),
        _fixedDrawOrder = List<PlayingCard>.unmodifiable(drawOrder) {
    reset();
  }

  final Random _random;
  final List<PlayingCard>? _fixedDrawOrder;
  final List<PlayingCard> _cards = <PlayingCard>[];

  int get remaining => _cards.length;

  void reset() {
    _cards.clear();

    final fixed = _fixedDrawOrder;
    if (fixed != null) {
      _cards.addAll(fixed.reversed);
      return;
    }

    _cards
      ..addAll(
        List<PlayingCard>.generate(
          53,
          PlayingCard.fromLegacyId,
          growable: false,
        ),
      )
      ..shuffle(_random);
  }

  PlayingCard draw() {
    if (_cards.isEmpty) {
      throw StateError('Cannot draw from an empty deck.');
    }
    return _cards.removeLast();
  }

  List<PlayingCard> drawMany(int count) {
    if (count < 0 || count > _cards.length) {
      throw RangeError.range(count, 0, _cards.length, 'count');
    }
    return List<PlayingCard>.generate(count, (_) => draw(), growable: false);
  }
}
