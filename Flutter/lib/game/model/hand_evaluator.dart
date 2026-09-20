import 'hand_rank.dart';
import 'playing_card.dart';

class HandEvaluation {
  const HandEvaluation({
    required this.rank,
    required this.qualifyingHighPair,
  });

  final HandRank rank;

  /// True only when the final hand is exactly one pair of Jacks, Queens,
  /// Kings, or Aces. Higher poker hands do not also award a bonus token.
  final bool qualifyingHighPair;
}

class HandEvaluator {
  const HandEvaluator();

  HandRank evaluate(List<PlayingCard> hand) => evaluateDetailed(hand).rank;

  HandEvaluation evaluateDetailed(List<PlayingCard> hand) {
    if (hand.length != 5) {
      throw ArgumentError.value(hand.length, 'hand.length', 'Must contain 5 cards');
    }

    final jokerIndexes = <int>[
      for (var i = 0; i < hand.length; i++)
        if (hand[i].isJoker) i,
    ];

    if (jokerIndexes.isEmpty) {
      return _evaluateNaturalDetailed(hand);
    }

    if (jokerIndexes.length != 1) {
      throw StateError('Red Black Poker uses exactly one Joker.');
    }

    final jokerIndex = jokerIndexes.single;
    var bestRank = HandRank.none;
    var highPair = false;

    // The single Joker is wild. Try every rank/suit identity and keep the
    // strongest paid result. If no paid hand exists, remember whether any
    // substitution creates exactly one high pair.
    for (final suit in CardSuit.values) {
      for (var rank = 1; rank <= 13; rank++) {
        final candidate = List<PlayingCard>.of(hand);
        candidate[jokerIndex] = PlayingCard(
          rank: rank,
          suit: suit,
          assetId: -1,
        );

        final natural = _evaluateNaturalDetailed(candidate);
        if (natural.rank.basePayout > bestRank.basePayout) {
          bestRank = natural.rank;
        }
        highPair = highPair || natural.qualifyingHighPair;
      }
    }

    return HandEvaluation(
      rank: bestRank,
      qualifyingHighPair: bestRank == HandRank.none && highPair,
    );
  }

  HandEvaluation _evaluateNaturalDetailed(List<PlayingCard> hand) {
    final ranks = hand.map(_highRank).toList()..sort();
    final counts = <int, int>{};

    for (final rank in ranks) {
      counts.update(rank, (value) => value + 1, ifAbsent: () => 1);
    }

    final multiplicities = counts.values.toList()..sort();
    final flush = hand.every((card) => card.suit == hand.first.suit);
    final straight = _isStraight(ranks);
    final royal = ranks.toSet().containsAll(<int>{10, 11, 12, 13, 14});

    HandRank rank;
    if (multiplicities.contains(5)) {
      rank = HandRank.fiveOfAKind;
    } else if (flush && royal) {
      rank = HandRank.royalFlush;
    } else if (flush && straight) {
      rank = HandRank.straightFlush;
    } else if (multiplicities.contains(4)) {
      rank = HandRank.fourOfAKind;
    } else if (multiplicities.length == 2 &&
        multiplicities[0] == 2 &&
        multiplicities[1] == 3) {
      rank = HandRank.fullHouse;
    } else if (flush) {
      rank = HandRank.flush;
    } else if (straight) {
      rank = HandRank.straight;
    } else if (multiplicities.contains(3)) {
      rank = HandRank.threeOfAKind;
    } else if (multiplicities.where((count) => count == 2).length == 2) {
      rank = HandRank.twoPair;
    } else {
      rank = HandRank.none;
    }

    final pairRanks = counts.entries
        .where((entry) => entry.value == 2)
        .map((entry) => entry.key)
        .toList(growable: false);

    final qualifyingHighPair = rank == HandRank.none &&
        pairRanks.length == 1 &&
        const <int>{11, 12, 13, 14}.contains(pairRanks.single);

    return HandEvaluation(
      rank: rank,
      qualifyingHighPair: qualifyingHighPair,
    );
  }

  int _highRank(PlayingCard card) => card.rank == 1 ? 14 : card.rank;

  bool _isStraight(List<int> sortedHighRanks) {
    final unique = sortedHighRanks.toSet().toList()..sort();
    if (unique.length != 5) return false;

    if (unique.last - unique.first == 4) return true;

    return unique[0] == 2 &&
        unique[1] == 3 &&
        unique[2] == 4 &&
        unique[3] == 5 &&
        unique[4] == 14;
  }
}
