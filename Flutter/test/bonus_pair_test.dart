import 'package:flutter_test/flutter_test.dart';
import 'package:red_black_poker/game/model/hand_evaluator.dart';
import 'package:red_black_poker/game/model/hand_rank.dart';
import 'package:red_black_poker/game/model/playing_card.dart';

void main() {
  const evaluator = HandEvaluator();

  PlayingCard c(int rank, CardSuit suit) =>
      PlayingCard(rank: rank, suit: suit, assetId: -1);

  test('pair of Jacks earns a bonus token but no normal payout', () {
    final hand = <PlayingCard>[
      c(11, CardSuit.hearts),
      c(11, CardSuit.spades),
      c(3, CardSuit.clubs),
      c(6, CardSuit.diamonds),
      c(9, CardSuit.hearts),
    ];

    final result = evaluator.evaluateDetailed(hand);
    expect(result.rank, HandRank.none);
    expect(result.qualifyingHighPair, isTrue);
  });

  test('pair of Queens earns a bonus token', () {
    final hand = <PlayingCard>[
      c(12, CardSuit.hearts),
      c(12, CardSuit.clubs),
      c(2, CardSuit.spades),
      c(7, CardSuit.diamonds),
      c(10, CardSuit.hearts),
    ];

    expect(evaluator.evaluateDetailed(hand).qualifyingHighPair, isTrue);
  });

  test('pair of Kings earns a bonus token', () {
    final hand = <PlayingCard>[
      c(13, CardSuit.hearts),
      c(13, CardSuit.clubs),
      c(2, CardSuit.spades),
      c(7, CardSuit.diamonds),
      c(10, CardSuit.hearts),
    ];

    expect(evaluator.evaluateDetailed(hand).qualifyingHighPair, isTrue);
  });

  test('pair of Aces earns a bonus token', () {
    final hand = <PlayingCard>[
      c(1, CardSuit.hearts),
      c(1, CardSuit.clubs),
      c(2, CardSuit.spades),
      c(7, CardSuit.diamonds),
      c(10, CardSuit.hearts),
    ];

    expect(evaluator.evaluateDetailed(hand).qualifyingHighPair, isTrue);
  });

  test('low pair does not earn a bonus token', () {
    final hand = <PlayingCard>[
      c(10, CardSuit.hearts),
      c(10, CardSuit.clubs),
      c(2, CardSuit.spades),
      c(7, CardSuit.diamonds),
      c(4, CardSuit.hearts),
    ];

    expect(evaluator.evaluateDetailed(hand).qualifyingHighPair, isFalse);
  });

  test('two pair is paid normally and does not also award bonus token', () {
    final hand = <PlayingCard>[
      c(12, CardSuit.hearts),
      c(12, CardSuit.clubs),
      c(7, CardSuit.spades),
      c(7, CardSuit.diamonds),
      c(4, CardSuit.hearts),
    ];

    final result = evaluator.evaluateDetailed(hand);
    expect(result.rank, HandRank.twoPair);
    expect(result.qualifyingHighPair, isFalse);
  });

  test('Joker plus a natural pair becomes three of a kind, not bonus pair', () {
    final hand = <PlayingCard>[
      c(4, CardSuit.clubs),
      c(10, CardSuit.hearts),
      const PlayingCard.joker(),
      c(10, CardSuit.clubs),
      c(11, CardSuit.spades),
    ];

    final result = evaluator.evaluateDetailed(hand);
    expect(result.rank, HandRank.threeOfAKind);
    expect(result.qualifyingHighPair, isFalse);
  });

  test('Joker can complete a qualifying high pair', () {
    final hand = <PlayingCard>[
      const PlayingCard.joker(),
      c(13, CardSuit.clubs),
      c(2, CardSuit.spades),
      c(7, CardSuit.diamonds),
      c(10, CardSuit.hearts),
    ];

    final result = evaluator.evaluateDetailed(hand);
    expect(result.rank, HandRank.none);
    expect(result.qualifyingHighPair, isTrue);
  });
}
