import 'package:flutter_test/flutter_test.dart';
import 'package:red_black_poker/game/model/auto_hold_advisor.dart';
import 'package:red_black_poker/game/model/playing_card.dart';

void main() {
  const advisor = AutoHoldAdvisor();

  PlayingCard c(int rank, CardSuit suit) =>
      PlayingCard(rank: rank, suit: suit, assetId: -1);

  test('does not jump a gap for 5-6-8-9', () {
    final hand = <PlayingCard>[
      c(5, CardSuit.hearts),
      c(6, CardSuit.clubs),
      c(8, CardSuit.spades),
      c(9, CardSuit.diamonds),
      c(2, CardSuit.hearts),
    ];

    final d = advisor.recommend(hand);
    expect(d.held, isNot(<bool>[true, true, true, true, false]));
  });

  test('does not auto hold only three consecutive cards', () {
    final hand = <PlayingCard>[
      c(7, CardSuit.hearts),
      c(8, CardSuit.clubs),
      c(9, CardSuit.spades),
      c(11, CardSuit.diamonds),
      c(3, CardSuit.hearts),
    ];

    final d = advisor.recommend(hand);
    expect(d.held, isNot(<bool>[true, true, true, false, false]));
  });

  test('holds four consecutive cards', () {
    final hand = <PlayingCard>[
      c(7, CardSuit.hearts),
      c(8, CardSuit.clubs),
      c(9, CardSuit.spades),
      c(10, CardSuit.diamonds),
      c(3, CardSuit.hearts),
    ];

    final d = advisor.recommend(hand);
    expect(d.held, <bool>[true, true, true, true, false]);
  });

  test('joker may fill exactly one adjacent missing rank', () {
    final hand = <PlayingCard>[
      c(7, CardSuit.hearts),
      c(8, CardSuit.clubs),
      const PlayingCard.joker(),
      c(10, CardSuit.spades),
      c(2, CardSuit.hearts),
    ];

    final d = advisor.recommend(hand);
    expect(d.held, <bool>[true, true, true, true, false]);
  });

  test('joker does not bridge two missing ranks', () {
    final hand = <PlayingCard>[
      c(7, CardSuit.hearts),
      c(8, CardSuit.clubs),
      const PlayingCard.joker(),
      c(11, CardSuit.spades),
      c(2, CardSuit.hearts),
    ];

    final d = advisor.recommend(hand);
    expect(d.held, isNot(<bool>[true, true, true, true, false]));
  });

  test('holds four suited cards regardless of positions', () {
    final hand = <PlayingCard>[
      c(2, CardSuit.hearts),
      c(11, CardSuit.hearts),
      c(7, CardSuit.clubs),
      c(6, CardSuit.hearts),
      c(9, CardSuit.hearts),
    ];

    final d = advisor.recommend(hand);
    expect(d.held, <bool>[true, true, false, true, true]);
  });

  test('holds a low pair like the original auto hold', () {
    final hand = <PlayingCard>[
      c(4, CardSuit.hearts),
      c(9, CardSuit.clubs),
      c(4, CardSuit.spades),
      c(12, CardSuit.diamonds),
      c(2, CardSuit.hearts),
    ];

    final d = advisor.recommend(hand);
    expect(d.held, <bool>[true, false, true, false, false]);
  });

  test('joker plus a pair holds all three as three of a kind', () {
    final hand = <PlayingCard>[
      c(4, CardSuit.hearts),
      c(10, CardSuit.clubs),
      const PlayingCard.joker(),
      c(10, CardSuit.spades),
      c(2, CardSuit.hearts),
    ];

    final d = advisor.recommend(hand);
    expect(d.held, <bool>[false, true, true, true, false]);
  });

  test('two pair holds both pairs', () {
    final hand = <PlayingCard>[
      c(8, CardSuit.hearts),
      c(4, CardSuit.clubs),
      c(8, CardSuit.spades),
      c(4, CardSuit.diamonds),
      c(13, CardSuit.hearts),
    ];

    final d = advisor.recommend(hand);
    expect(d.held, <bool>[true, true, true, true, false]);
  });
}
