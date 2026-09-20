import 'dart:math';

import 'playing_card.dart';

class Deck {
  Deck({Random? random}) : _random = random ?? Random.secure() {
    reset();
  }

  final Random _random;
  final List<PlayingCard> _cards = <PlayingCard>[];

  int get remaining => _cards.length;

  void reset() {
    _cards
      ..clear()
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
