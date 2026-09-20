import 'deck.dart';
import 'hand_evaluator.dart';
import 'hand_rank.dart';
import 'playing_card.dart';

enum RoundPhase { idle, chooseHolds, result, bonusReady }

class PokerRound {
  PokerRound({
    Deck? deck,
    HandEvaluator? evaluator,
    this.startingCredits = 10,
    this.bonusTarget = 10,
  })  : _deck = deck ?? Deck(),
        _evaluator = evaluator ?? const HandEvaluator(),
        credits = startingCredits;

  final Deck _deck;
  final HandEvaluator _evaluator;
  final int startingCredits;

  /// Halloween uses ten zombie heads. This is deliberately configurable so
  /// another theme can visualize the same mechanic differently.
  final int bonusTarget;

  final List<PlayingCard> cards = <PlayingCard>[];
  final List<bool> held = List<bool>.filled(5, false);

  RoundPhase phase = RoundPhase.idle;
  HandRank result = HandRank.none;
  int credits;
  int bet = 1;
  int lastWin = 0;
  int bonusProgress = 0;

  bool get canDeal =>
      phase != RoundPhase.chooseHolds && phase != RoundPhase.bonusReady && credits >= bet;
  bool get canDraw => phase == RoundPhase.chooseHolds;
  bool get bonusReady => bonusProgress >= bonusTarget;

  void changeBet(int delta) {
    if (phase == RoundPhase.chooseHolds || phase == RoundPhase.bonusReady) return;
    bet = (bet + delta).clamp(1, 100);
  }

  void deal() {
    if (!canDeal) return;

    credits -= bet;
    lastWin = 0;
    result = HandRank.none;
    _deck.reset();

    cards
      ..clear()
      ..addAll(_deck.drawMany(5));

    for (var i = 0; i < held.length; i++) {
      held[i] = false;
    }

    phase = RoundPhase.chooseHolds;
  }

  void toggleHold(int index) {
    if (!canDraw || index < 0 || index >= held.length) return;
    held[index] = !held[index];
  }

  void draw() {
    if (!canDraw) return;

    for (var i = 0; i < cards.length; i++) {
      if (!held[i]) {
        cards[i] = _deck.draw();
      }
    }

    final evaluation = _evaluator.evaluateDetailed(cards);
    result = evaluation.rank;
    lastWin = result.basePayout * bet;
    credits += lastWin;

    if (evaluation.qualifyingHighPair) {
      bonusProgress = (bonusProgress + 1).clamp(0, bonusTarget);
    }

    phase = bonusReady ? RoundPhase.bonusReady : RoundPhase.result;
  }

  /// Called after the Red-or-Black bonus session has been entered.
  void consumeBonus() {
    if (!bonusReady) return;
    bonusProgress = 0;
    phase = RoundPhase.result;
  }
}
