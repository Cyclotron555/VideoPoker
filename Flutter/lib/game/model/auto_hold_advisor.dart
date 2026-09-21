import 'playing_card.dart';

class AutoHoldDecision {
  const AutoHoldDecision(this.held, this.reason);

  final List<bool> held;
  final String reason;
}

class AutoHoldAdvisor {
  const AutoHoldAdvisor();

  AutoHoldDecision recommend(List<PlayingCard> hand) {
    if (hand.length != 5) {
      throw ArgumentError.value(hand.length, 'hand.length', 'Must contain 5 cards');
    }

    // Preserve the legacy strategy hierarchy:
    // made/wild premium hands -> four-kind/full-house -> flush draw -> trips
    // -> straight draw -> two pair -> pair.
    final fiveKind = _bestFiveOfAKindMask(hand);
    if (fiveKind != null) return AutoHoldDecision(fiveKind, 'FIVE OF A KIND');

    final straightFlush = _madeStraightFlushMask(hand);
    if (straightFlush != null) {
      return AutoHoldDecision(straightFlush, 'STRAIGHT FLUSH');
    }

    final fourKind = _bestNOfKindMask(hand, 4);
    if (fourKind != null) return AutoHoldDecision(fourKind, 'FOUR OF A KIND');

    if (_isFullHouseWild(hand)) {
      return AutoHoldDecision(List<bool>.filled(5, true), 'FULL HOUSE');
    }

    final flush = _bestFlushMask(hand);
    if (flush != null) return AutoHoldDecision(flush, 'FLUSH / FLUSH DRAW');

    final trips = _bestNOfKindMask(hand, 3);
    if (trips != null) return AutoHoldDecision(trips, 'THREE OF A KIND');

    final straight = _bestStraightMask(hand);
    if (straight != null) return AutoHoldDecision(straight, 'STRAIGHT / STRAIGHT DRAW');

    final twoPair = _twoPairMask(hand);
    if (twoPair != null) return AutoHoldDecision(twoPair, 'TWO PAIR');

    final pair = _bestPairMask(hand);
    if (pair != null) return AutoHoldDecision(pair, 'PAIR');

    final jokerIndex = hand.indexWhere((card) => card.isJoker);
    if (jokerIndex >= 0) {
      final bestOther = _highestNonJokerIndex(hand);
      final held = List<bool>.filled(5, false);
      held[jokerIndex] = true;
      if (bestOther >= 0) held[bestOther] = true;
      return AutoHoldDecision(held, 'JOKER + HIGH CARD');
    }

    return AutoHoldDecision(List<bool>.filled(5, false), 'NO HOLD');
  }

  List<bool>? _bestFiveOfAKindMask(List<PlayingCard> hand) {
    final joker = hand.indexWhere((c) => c.isJoker);
    if (joker < 0) return null;

    final counts = <int, List<int>>{};
    for (var i = 0; i < hand.length; i++) {
      if (hand[i].isJoker) continue;
      counts.putIfAbsent(hand[i].rank, () => <int>[]).add(i);
    }
    for (final entry in counts.entries) {
      if (entry.value.length == 4) {
        return List<bool>.filled(5, true);
      }
    }
    return null;
  }

  List<bool>? _madeStraightFlushMask(List<PlayingCard> hand) {
    // A made natural/wild straight flush keeps all five.
    for (final suit in CardSuit.values) {
      final ranks = <int>[];
      var jokers = 0;
      for (final card in hand) {
        if (card.isJoker) {
          jokers++;
        } else if (card.suit == suit) {
          ranks.add(card.rank);
        }
      }
      if (ranks.length + jokers == 5) {
        final normalized = ranks.map((r) => r == 1 ? 14 : r).toList();
        final high = jokers == 0
            ? _naturalConsecutiveHigh(normalized)
            : _jokerConsecutiveHigh(normalized, 5);
        if (high != null) {
          return List<bool>.filled(5, true);
        }
      }
    }
    return null;
  }

  List<bool>? _bestNOfKindMask(List<PlayingCard> hand, int target) {
    final jokerIndexes = <int>[
      for (var i = 0; i < hand.length; i++)
        if (hand[i].isJoker) i,
    ];

    final byRank = <int, List<int>>{};
    for (var i = 0; i < hand.length; i++) {
      if (hand[i].isJoker) continue;
      byRank.putIfAbsent(hand[i].rank, () => <int>[]).add(i);
    }

    List<int>? best;
    for (final indexes in byRank.values) {
      final combined = <int>[...indexes, ...jokerIndexes];
      if (combined.length >= target) {
        final candidate = combined.take(target).toList();
        if (best == null || candidate.length > best.length) best = candidate;
      }
    }

    if (best == null) return null;
    final mask = List<bool>.filled(5, false);
    for (final i in best) {
      mask[i] = true;
    }
    return mask;
  }

  bool _isFullHouseWild(List<PlayingCard> hand) {
    // Exhaustively assign the single joker to every rank and test 3+2.
    final joker = hand.indexWhere((c) => c.isJoker);
    if (joker < 0) return _isNaturalFullHouse(hand.map((c) => c.rank).toList());

    for (var rank = 1; rank <= 13; rank++) {
      final ranks = <int>[
        for (var i = 0; i < hand.length; i++) i == joker ? rank : hand[i].rank,
      ];
      if (_isNaturalFullHouse(ranks)) return true;
    }
    return false;
  }

  bool _isNaturalFullHouse(List<int> ranks) {
    final counts = <int, int>{};
    for (final rank in ranks) {
      counts.update(rank, (v) => v + 1, ifAbsent: () => 1);
    }
    final values = counts.values.toList()..sort();
    return values.length == 2 && values[0] == 2 && values[1] == 3;
  }

  List<bool>? _bestFlushMask(List<PlayingCard> hand) {
    final jokerIndexes = <int>[
      for (var i = 0; i < hand.length; i++)
        if (hand[i].isJoker) i,
    ];

    List<int>? best;
    for (final suit in CardSuit.values) {
      final indexes = <int>[
        for (var i = 0; i < hand.length; i++)
          if (!hand[i].isJoker && hand[i].suit == suit) i,
        ...jokerIndexes,
      ];
      if (indexes.length >= 4 && (best == null || indexes.length > best.length)) {
        best = indexes;
      }
    }

    if (best == null) return null;
    final mask = List<bool>.filled(5, false);
    for (final i in best) {
      mask[i] = true;
    }
    return mask;
  }

  List<bool>? _bestStraightMask(List<PlayingCard> hand) {
    // RBP rule:
    // - Without a Joker, auto-hold only if at least 3 cards are already
    //   consecutive after sorting.
    // - With a Joker, the Joker may only occupy a rank directly inside or
    //   adjacent to that run. It cannot "jump" over an extra missing rank.
    //
    // Prefer the longest valid run, then the higher run.
    List<int>? bestIndexes;
    var bestLength = 0;
    var bestHigh = -1;

    final jokerIndex = hand.indexWhere((c) => c.isJoker);

    for (var mask = 1; mask < (1 << 5); mask++) {
      final indexes = <int>[
        for (var i = 0; i < 5; i++)
          if ((mask & (1 << i)) != 0) i,
      ];

      final nonJokerRanks = <int>[];
      var includesJoker = false;

      for (final i in indexes) {
        final card = hand[i];
        if (card.isJoker) {
          includesJoker = true;
        } else {
          nonJokerRanks.add(card.rank == 1 ? 14 : card.rank);
        }
      }

      final cardCount = indexes.length;
      if (cardCount < 4) continue;

      final high = includesJoker
          ? _jokerConsecutiveHigh(nonJokerRanks, cardCount)
          : _naturalConsecutiveHigh(nonJokerRanks);

      if (high == null) continue;

      if (cardCount > bestLength ||
          (cardCount == bestLength && high > bestHigh)) {
        bestLength = cardCount;
        bestHigh = high;
        bestIndexes = indexes;
      }
    }

    if (bestIndexes == null) return null;

    final mask = List<bool>.filled(5, false);
    for (final i in bestIndexes) {
      mask[i] = true;
    }
    return mask;
  }

  int? _naturalConsecutiveHigh(List<int> ranks) {
    if (ranks.length < 4) return null;
    final sorted = ranks.toSet().toList()..sort();
    if (sorted.length != ranks.length) return null;

    var consecutive = true;
    for (var i = 1; i < sorted.length; i++) {
      if (sorted[i] != sorted[i - 1] + 1) {
        consecutive = false;
        break;
      }
    }
    if (consecutive) return sorted.last;

    // Ace-low run, e.g. A-2-3 or A-2-3-4.
    final aceLow = sorted.map((r) => r == 14 ? 1 : r).toList()..sort();
    for (var i = 1; i < aceLow.length; i++) {
      if (aceLow[i] != aceLow[i - 1] + 1) return null;
    }
    return aceLow.last;
  }

  int? _jokerConsecutiveHigh(List<int> ranks, int totalCards) {
    // The Joker must account for exactly one rank in the final contiguous run.
    // If the natural cards already require two or more missing ranks, reject it.
    if (totalCards < 4 || ranks.length != totalCards - 1) return null;

    final normalized = ranks.toSet().toList()..sort();
    if (normalized.length != ranks.length) return null;

    // Try every legal Joker rank in a standard straight window.
    for (var jokerRank = 1; jokerRank <= 14; jokerRank++) {
      final trial = <int>[...normalized, jokerRank];

      final naturalHigh = _naturalConsecutiveHigh(trial);
      if (naturalHigh != null) return naturalHigh;
    }

    return null;
  }

  List<bool>? _twoPairMask(List<PlayingCard> hand) {
    if (hand.any((c) => c.isJoker)) return null;
    final byRank = <int, List<int>>{};
    for (var i = 0; i < hand.length; i++) {
      byRank.putIfAbsent(hand[i].rank, () => <int>[]).add(i);
    }
    final pairs = byRank.values.where((v) => v.length == 2).toList();
    if (pairs.length < 2) return null;

    final mask = List<bool>.filled(5, false);
    for (final pair in pairs.take(2)) {
      for (final i in pair) {
        mask[i] = true;
      }
    }
    return mask;
  }

  List<bool>? _bestPairMask(List<PlayingCard> hand) {
    final jokerIndexes = <int>[
      for (var i = 0; i < hand.length; i++)
        if (hand[i].isJoker) i,
    ];
    final byRank = <int, List<int>>{};
    for (var i = 0; i < hand.length; i++) {
      if (hand[i].isJoker) continue;
      byRank.putIfAbsent(hand[i].rank, () => <int>[]).add(i);
    }

    List<int>? best;
    int bestRank = -1;
    for (final entry in byRank.entries) {
      final indexes = <int>[...entry.value, ...jokerIndexes];
      if (indexes.length >= 2) {
        final highRank = entry.key == 1 ? 14 : entry.key;
        if (highRank > bestRank) {
          bestRank = highRank;
          best = indexes.take(2).toList();
        }
      }
    }

    if (best == null) return null;
    final mask = List<bool>.filled(5, false);
    for (final i in best) {
      mask[i] = true;
    }
    return mask;
  }

  int _highestNonJokerIndex(List<PlayingCard> hand) {
    var bestIndex = -1;
    var bestRank = -1;
    for (var i = 0; i < hand.length; i++) {
      if (hand[i].isJoker) continue;
      final rank = hand[i].rank == 1 ? 14 : hand[i].rank;
      if (rank > bestRank) {
        bestRank = rank;
        bestIndex = i;
      }
    }
    return bestIndex;
  }
}
