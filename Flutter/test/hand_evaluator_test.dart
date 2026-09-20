import 'package:flutter_test/flutter_test.dart';
import 'package:red_black_poker/game/model/hand_evaluator.dart';
import 'package:red_black_poker/game/model/hand_rank.dart';
import 'package:red_black_poker/game/model/playing_card.dart';

void main() {
  const evaluator = HandEvaluator();

  PlayingCard c(int rank, CardSuit suit) =>
      PlayingCard(rank: rank, suit: suit, assetId: -1);

  test('recognizes wheel straight A-2-3-4-5', () {
    final hand = <PlayingCard>[
      c(1, CardSuit.hearts),
      c(2, CardSuit.clubs),
      c(3, CardSuit.spades),
      c(4, CardSuit.diamonds),
      c(5, CardSuit.hearts),
    ];

    expect(evaluator.evaluate(hand), HandRank.straight);
  });

  test('recognizes natural royal flush', () {
    final hand = <PlayingCard>[
      c(10, CardSuit.hearts),
      c(11, CardSuit.hearts),
      c(12, CardSuit.hearts),
      c(13, CardSuit.hearts),
      c(1, CardSuit.hearts),
    ];

    expect(evaluator.evaluate(hand), HandRank.royalFlush);
  });

  test('Joker completes royal flush', () {
    final hand = <PlayingCard>[
      const PlayingCard.joker(),
      c(10, CardSuit.spades),
      c(11, CardSuit.spades),
      c(12, CardSuit.spades),
      c(13, CardSuit.spades),
    ];

    expect(evaluator.evaluate(hand), HandRank.royalFlush);
  });

  test('Joker produces five of a kind', () {
    final hand = <PlayingCard>[
      const PlayingCard.joker(),
      c(8, CardSuit.hearts),
      c(8, CardSuit.diamonds),
      c(8, CardSuit.spades),
      c(8, CardSuit.clubs),
    ];

    expect(evaluator.evaluate(hand), HandRank.fiveOfAKind);
  });

  test('recognizes full house independent of card positions', () {
    final hand = <PlayingCard>[
      c(9, CardSuit.hearts),
      c(4, CardSuit.clubs),
      c(9, CardSuit.spades),
      c(4, CardSuit.diamonds),
      c(9, CardSuit.clubs),
    ];

    expect(evaluator.evaluate(hand), HandRank.fullHouse);
  });

  test('recognizes two pair independent of card positions', () {
    final hand = <PlayingCard>[
      c(3, CardSuit.hearts),
      c(12, CardSuit.clubs),
      c(7, CardSuit.spades),
      c(3, CardSuit.diamonds),
      c(12, CardSuit.hearts),
    ];

    expect(evaluator.evaluate(hand), HandRank.twoPair);
  });
}
