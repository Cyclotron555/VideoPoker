enum CardSuit { hearts, diamonds, spades, clubs }

class PlayingCard {
  const PlayingCard({
    required this.rank,
    required this.suit,
    required this.assetId,
    this.isJoker = false,
  });

  const PlayingCard.joker()
      : rank = 0,
        suit = null,
        assetId = 0,
        isJoker = true;

  final int rank; // 1 = Ace, 2..10, 11 = Jack, 12 = Queen, 13 = King
  final CardSuit? suit;
  final int assetId;
  final bool isJoker;

  String get assetPath => 'cards/$assetId.png';

  String get rankLabel => switch (rank) {
        1 => 'A',
        11 => 'J',
        12 => 'Q',
        13 => 'K',
        _ => rank.toString(),
      };

  static PlayingCard fromLegacyId(int id) {
    if (id == 0) return const PlayingCard.joker();
    if (id < 0 || id > 52) {
      throw RangeError.range(id, 0, 52, 'id');
    }

    final zeroBased = id - 1;
    final suitIndex = zeroBased ~/ 13;
    final rank = (zeroBased % 13) + 1;

    return PlayingCard(
      rank: rank,
      suit: CardSuit.values[suitIndex],
      assetId: id,
    );
  }

  @override
  String toString() => isJoker ? 'Joker' : '${rankLabel} ${suit!.name}';
}
