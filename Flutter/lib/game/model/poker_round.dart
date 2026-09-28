import 'deck.dart';
import 'hand_evaluator.dart';
import 'hand_rank.dart';
import 'playing_card.dart';

enum RoundPhase {
  idle,
  chooseHolds,
  result,
  winDecision,
  doubleUp,
  bonusReady,
}

class PokerRound {
  PokerRound({
    Deck? deck,
    HandEvaluator? evaluator,
    this.startingCredits = 10,
    this.startingWallet = 4412,
    this.bonusTarget = 10,
    this.bonusPrize = 25,
  })  : _deck = deck ?? Deck(),
        _evaluator = evaluator ?? const HandEvaluator(),
        credits = startingCredits,
        wallet = startingWallet;

  static const int minBet = 1;
  static const int maxBet = 10;
  static const int transferIncrement = 5;
  static const int addMoneyIncrement = 10;
  static const int highPairDisplayedPayout = 5;

  final Deck _deck;
  final HandEvaluator _evaluator;
  final int startingCredits;
  final int startingWallet;
  final int bonusTarget;
  final int bonusPrize;

  final List<PlayingCard> cards = <PlayingCard>[];
  final List<bool> held = List<bool>.filled(5, false);

  RoundPhase phase = RoundPhase.idle;
  HandRank result = HandRank.none;
  int credits;
  int wallet;
  int bet = minBet;
  int pendingWin = 0;
  int bonusProgress = 0;

  int get lastWin => pendingWin;

  // Original Red Black Poker terminology:
  // wallet = money the player owns outside the machine
  // cash   = playable machine balance
  int get cash => credits;
  int get totalFunds => wallet + credits + pendingWin;
  int get bank => totalFunds;

  bool get bonusReady => bonusProgress >= bonusTarget;

  bool get canAdjustMoney =>
      phase == RoundPhase.idle || phase == RoundPhase.result;

  bool get canTransferToCash => canAdjustMoney && wallet > 0;

  bool get canCashOut => canAdjustMoney && credits > 0;

  bool get canAddMoney => canAdjustMoney;

  bool get canChangeBet =>
      phase == RoundPhase.idle || phase == RoundPhase.result;

  // Compatibility getters retained for older tests/callers.
  bool get canInsertCoins => canTransferToCash;
  bool get canRefillWallet => canAdjustMoney && wallet == 0;

  bool get canStartHand =>
      (phase == RoundPhase.idle || phase == RoundPhase.result) &&
      credits >= bet;

  bool get canDrawReplacement => phase == RoundPhase.chooseHolds;

  bool get canPressMainDraw => canStartHand || canDrawReplacement;

  bool get hasPendingWin =>
      phase == RoundPhase.winDecision && pendingWin > 0;

  int payoutFor(HandRank rank) => rank.basePayout * bet;

  int get highPairPaytableValue => highPairDisplayedPayout * bet;

  void changeBet(int delta) {
    if (!canChangeBet) return;
    bet = (bet + delta).clamp(minBet, maxBet);
  }

  int transferToCash([int amount = transferIncrement]) {
    if (!canTransferToCash || amount <= 0) return 0;
    final transfer = amount > wallet ? wallet : amount;
    wallet -= transfer;
    credits += transfer;
    return transfer;
  }

  // Legacy name from the first Flutter pass.
  int insertCoins([int amount = transferIncrement]) => transferToCash(amount);

  int addMoney([int amount = addMoneyIncrement]) {
    if (!canAddMoney || amount <= 0) return 0;
    wallet += amount;
    return amount;
  }

  int cashOut() {
    if (!canCashOut) return 0;
    final amount = credits;
    wallet += amount;
    credits = 0;
    return amount;
  }

  int refillWallet() {
    if (!canRefillWallet) return 0;
    wallet = startingWallet;
    return wallet;
  }

  void startHand() {
    if (!canStartHand) return;

    credits -= bet;
    pendingWin = 0;
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
    if (!canDrawReplacement || index < 0 || index >= held.length) return;
    held[index] = !held[index];
  }

  void drawReplacement() {
    if (!canDrawReplacement) return;

    for (var i = 0; i < cards.length; i++) {
      if (!held[i]) {
        cards[i] = _deck.draw();
      }
    }

    final evaluation = _evaluator.evaluateDetailed(cards);
    result = evaluation.rank;
    pendingWin = payoutFor(result);

    if (evaluation.qualifyingHighPair) {
      bonusProgress = (bonusProgress + 1).clamp(0, bonusTarget);
    }

    if (bonusReady) {
      phase = RoundPhase.bonusReady;
    } else if (pendingWin > 0) {
      phase = RoundPhase.winDecision;
    } else {
      phase = RoundPhase.result;
    }
  }

  int collectWin() {
    if (!hasPendingWin) return 0;
    final amount = pendingWin;
    credits += amount;
    pendingWin = 0;
    phase = RoundPhase.result;
    return amount;
  }

  int beginDoubleUp() {
    if (!hasPendingWin) {
      throw StateError('No pending win is available to double.');
    }
    final amount = pendingWin;
    pendingWin = 0;
    phase = RoundPhase.doubleUp;
    return amount;
  }

  void finishDoubleUp(int collectedAmount) {
    if (phase != RoundPhase.doubleUp) {
      throw StateError('No Red/Black double-up session is active.');
    }
    if (collectedAmount > 0) {
      credits += collectedAmount;
    }
    phase = RoundPhase.result;
  }

  int beginBonus() {
    if (!bonusReady || phase != RoundPhase.bonusReady) {
      throw StateError('Bonus is not ready.');
    }
    bonusProgress = 0;
    phase = RoundPhase.doubleUp;
    return bonusPrize;
  }
}
