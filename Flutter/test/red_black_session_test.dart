import 'package:flutter_test/flutter_test.dart';
import 'package:red_black_poker/game/model/red_black_session.dart';

void main() {
  test('six correct guesses grow stake to exactly 64x and hit jackpot', () {
    final session = RedBlackSession.withDeck(
      startingWin: 10,
      drawOrder: const <int>[1, 27, 2, 28, 3, 29],
    );

    final choices = <RedBlackChoice>[
      RedBlackChoice.red,
      RedBlackChoice.black,
      RedBlackChoice.red,
      RedBlackChoice.black,
      RedBlackChoice.red,
      RedBlackChoice.black,
    ];

    for (var i = 0; i < choices.length; i++) {
      final turn = session.guess(choices[i]);
      expect(turn.correct, isTrue);
      expect(session.multiplier, 1 << (i + 1));
      expect(session.currentWin, 10 * (1 << (i + 1)));
    }

    expect(session.currentWin, 640);
    expect(session.multiplier, 64);
    expect(session.roundsWon, 6);
    expect(session.phase, RedBlackPhase.jackpot);
    expect(session.canGuess, isFalse);
    expect(session.canCollect, isFalse);
  });

  test('collect banks current live amount and closes session', () {
    final session = RedBlackSession.withDeck(
      startingWin: 25,
      drawOrder: const <int>[1],
    );

    expect(session.collect(), 25);
    expect(session.phase, RedBlackPhase.collected);
    expect(session.canGuess, isFalse);
  });

  test('player can collect after any successful guess before jackpot', () {
    final session = RedBlackSession.withDeck(
      startingWin: 10,
      drawOrder: const <int>[27, 1],
    );

    final turn = session.guess(RedBlackChoice.black);
    expect(turn.correct, isTrue);
    expect(session.currentWin, 20);
    expect(session.canCollect, isTrue);

    expect(session.collect(), 20);
    expect(session.phase, RedBlackPhase.collected);
    expect(session.canGuess, isFalse);
  });

  test('wrong guess busts live win to zero immediately', () {
    final session = RedBlackSession.withDeck(
      startingWin: 20,
      drawOrder: const <int>[1],
    );

    final turn = session.guess(RedBlackChoice.black);

    expect(turn.correct, isFalse);
    expect(session.phase, RedBlackPhase.busted);
    expect(session.currentWin, 0);
    expect(session.canGuess, isFalse);
    expect(session.canCollect, isFalse);
  });

  test('each correct guess doubles from the original stake', () {
    final session = RedBlackSession.withDeck(
      startingWin: 15,
      drawOrder: const <int>[1, 27, 2],
    );

    session.guess(RedBlackChoice.red);
    expect(session.currentWin, 30);
    expect(session.multiplier, 2);

    session.guess(RedBlackChoice.black);
    expect(session.currentWin, 60);
    expect(session.multiplier, 4);

    session.guess(RedBlackChoice.red);
    expect(session.currentWin, 120);
    expect(session.multiplier, 8);
  });
}
