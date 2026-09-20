import 'dart:math';

enum RedBlackChoice { red, black }

enum RedBlackPhase { choosing, busted, collected, jackpot }

class RedBlackTurn {
  const RedBlackTurn({
    required this.cardId,
    required this.color,
    required this.correct,
    required this.winAfterTurn,
  });

  final int cardId;
  final RedBlackChoice color;
  final bool correct;
  final int winAfterTurn;
}

class RedBlackSession {
  RedBlackSession({
    required int startingWin,
    Random? random,
    this.maxRounds = 6,
  })  : assert(startingWin > 0),
        currentWin = startingWin,
        _random = random ?? Random.secure() {
    _resetDeck();
  }

  final Random _random;
  final int maxRounds;
  final List<int> _deck = <int>[];

  int currentWin;
  int roundsWon = 0;
  RedBlackPhase phase = RedBlackPhase.choosing;
  final List<RedBlackTurn> history = <RedBlackTurn>[];

  bool get canGuess => phase == RedBlackPhase.choosing;
  bool get canCollect => phase == RedBlackPhase.choosing && currentWin > 0;

  RedBlackTurn guess(RedBlackChoice choice) {
    if (!canGuess) {
      throw StateError('Red/Black session is not accepting guesses.');
    }

    if (_deck.isEmpty) _resetDeck();
    final cardId = _deck.removeLast();
    final color = _colorForCard(cardId);
    final correct = choice == color;

    if (!correct) {
      currentWin = 0;
      phase = RedBlackPhase.busted;
    } else {
      roundsWon += 1;
      currentWin *= 2;
      if (roundsWon >= maxRounds) {
        phase = RedBlackPhase.jackpot;
      }
    }

    final turn = RedBlackTurn(
      cardId: cardId,
      color: color,
      correct: correct,
      winAfterTurn: currentWin,
    );
    history.add(turn);
    return turn;
  }

  int collect() {
    if (!canCollect) {
      throw StateError('Nothing can be collected right now.');
    }
    phase = RedBlackPhase.collected;
    return currentWin;
  }

  void _resetDeck() {
    _deck
      ..clear()
      ..addAll(List<int>.generate(52, (index) => index + 1))
      ..shuffle(_random);
  }

  RedBlackChoice _colorForCard(int legacyCardId) {
    // Legacy deck IDs: Hearts 1-13, Diamonds 14-26,
    // Spades 27-39, Clubs 40-52.
    return legacyCardId <= 26 ? RedBlackChoice.red : RedBlackChoice.black;
  }
}
