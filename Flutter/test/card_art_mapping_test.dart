import 'package:flutter_test/flutter_test.dart';
import 'package:red_black_poker/game/layout/card_art_mapping.dart';

void main() {
  test('horror atlas keeps 9 and 10 in their real visual columns', () {
    expect(horrorAtlasColumnForRank(9), 4);
    expect(horrorAtlasColumnForRank(10), 5);
  });

  test('face-card atlas mapping remains stable', () {
    expect(horrorAtlasColumnForRank(1), 0);
    expect(horrorAtlasColumnForRank(13), 1);
    expect(horrorAtlasColumnForRank(12), 2);
    expect(horrorAtlasColumnForRank(11), 3);
  });
}
