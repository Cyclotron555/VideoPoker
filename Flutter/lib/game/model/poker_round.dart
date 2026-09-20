import 'deck.dart';
import 'hand_evaluator.dart';
import 'hand_rank.dart';
import 'playing_card.dart';

enum RoundPhase { idle, chooseHolds, result }

class PokerRound {
  PokerRound({
    Deck? deck,
    HandEvaluator? evaluator,
    this.startingCredits = 10,
  })  : _deck = deck ?? Deck(),
        _evaluator = evaluator ?? const HandEvaluator(),
        credits = startingCredits;

  final Deck _deck;
  final HandEvaluator _evaluator;
  final int startingCredits;

  final List<PlayingCard> cards = <PlayingCard>[];
  final List<bool> held = List<bool>.filled(5, false);

  RoundPhase phase = RoundPhase.idle;
  HandRank result = HandRank.none;
  int credits;
  int bet = 1;
  int lastWin = 0;

  bool get canDeal => phase != RoundPhase.chooseHolds && credits >= bet;
  bool get canDraw => phase == RoundPhase.chooseHolds;

  void changeBet(int delta) {
    if (phase == RoundPhase.chooseHolds) return;
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

    result = _evaluator.evaluate(cards);
    lastWin = result.basePayout * bet;
    credits += lastWin;
    phase = RoundPhase.result;
  }
}
