import 'hand_rank.dart';
import 'playing_card.dart';

class HandEvaluator {
  const HandEvaluator();

  HandRank evaluate(List<PlayingCard> hand) {
    if (hand.length != 5) {
      throw ArgumentError.value(hand.length, 'hand.length', 'Must contain 5 cards');
    }

    final jokerIndexes = <int>[
      for (var i = 0; i < hand.length; i++)
        if (hand[i].isJoker) i,
    ];

    if (jokerIndexes.isEmpty) {
      return _evaluateNatural(hand);
    }

    // Red Black Poker has one Joker. Evaluate every legal rank/suit identity
    // the Joker could assume and keep the highest-paying resulting hand.
    if (jokerIndexes.length != 1) {
      throw StateError('Red Black Poker uses exactly one Joker.');
    }

    var best = HandRank.none;
    final jokerIndex = jokerIndexes.single;

    for (final suit in CardSuit.values) {
      for (var rank = 1; rank <= 13; rank++) {
        final candidate = List<PlayingCard>.of(hand);
        candidate[jokerIndex] = PlayingCard(
          rank: rank,
          suit: suit,
          assetId: -1,
        );

        final result = _evaluateNatural(candidate);
        if (result.basePayout > best.basePayout) {
          best = result;
        }
      }
    }

    return best;
  }

  HandRank _evaluateNatural(List<PlayingCard> hand) {
    final ranks = hand.map(_highRank).toList()..sort();
    final counts = <int, int>{};

    for (final rank in ranks) {
      counts.update(rank, (value) => value + 1, ifAbsent: () => 1);
    }

    final multiplicities = counts.values.toList()..sort();
    final flush = hand.every((card) => card.suit == hand.first.suit);
    final straight = _isStraight(ranks);
    final royal = ranks.toSet().containsAll(<int>{10, 11, 12, 13, 14});

    if (multiplicities.contains(5)) return HandRank.fiveOfAKind;
    if (flush && royal) return HandRank.royalFlush;
    if (flush && straight) return HandRank.straightFlush;
    if (multiplicities.contains(4)) return HandRank.fourOfAKind;

    if (multiplicities.length == 2 &&
        multiplicities[0] == 2 &&
        multiplicities[1] == 3) {
      return HandRank.fullHouse;
    }

    if (flush) return HandRank.flush;
    if (straight) return HandRank.straight;
    if (multiplicities.contains(3)) return HandRank.threeOfAKind;

    if (multiplicities.where((count) => count == 2).length == 2) {
      return HandRank.twoPair;
    }

    return HandRank.none;
  }

  int _highRank(PlayingCard card) => card.rank == 1 ? 14 : card.rank;

  bool _isStraight(List<int> sortedHighRanks) {
    final unique = sortedHighRanks.toSet().toList()..sort();
    if (unique.length != 5) return false;

    if (unique.last - unique.first == 4) return true;

    // A-2-3-4-5 wheel.
    return unique[0] == 2 &&
        unique[1] == 3 &&
        unique[2] == 4 &&
        unique[3] == 5 &&
        unique[4] == 14;
  }
}
