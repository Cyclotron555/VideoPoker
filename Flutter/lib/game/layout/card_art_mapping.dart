import '../model/playing_card.dart';

/// Maps the real logical card identity to the horror-card atlas cell.
///
/// The atlas places 9 before 10. Keeping this mapping explicit prevents the
/// artwork from showing a different rank than the one evaluated by the game.
int horrorAtlasColumnForRank(int rank) {
  return switch (rank) {
    1 => 0,
    13 => 1,
    12 => 2,
    11 => 3,
    9 => 4,
    10 => 5,
    8 => 6,
    7 => 7,
    6 => 8,
    5 => 9,
    4 => 10,
    3 => 11,
    2 => 12,
    _ => 0,
  };
}

int horrorAtlasRowForSuit(CardSuit? suit) {
  return switch (suit) {
    CardSuit.hearts => 0,
    CardSuit.diamonds => 1,
    CardSuit.clubs => 2,
    CardSuit.spades => 3,
    null => 0,
  };
}
