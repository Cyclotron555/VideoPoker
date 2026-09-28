import 'package:flutter_test/flutter_test.dart';
import 'package:red_black_poker/game/model/poker_round.dart';

void main() {
  test('idle phase exposes only valid six-button actions', () {
    final round = PokerRound(startingCredits: 20, startingWallet: 10);

    final controls = round.controls;
    expect(controls.betMinus, isFalse);
    expect(controls.betPlus, isTrue);
    expect(controls.transferToCash, isTrue);
    expect(controls.cashOut, isTrue);
    expect(controls.addMoney, isTrue);
    expect(controls.draw, isTrue);
    expect(controls.holdCards, isFalse);
  });

  test('bet limits switch only the appropriate bet button off', () {
    final round = PokerRound(startingCredits: 20, startingWallet: 10);

    round.changeBet(99);
    expect(round.bet, PokerRound.maxBet);
    expect(round.controls.betPlus, isFalse);
    expect(round.controls.betMinus, isTrue);

    round.changeBet(-99);
    expect(round.bet, PokerRound.minBet);
    expect(round.controls.betMinus, isFalse);
    expect(round.controls.betPlus, isTrue);
  });

  test('DRAW is off when CASH cannot cover the selected bet', () {
    final round = PokerRound(startingCredits: 1, startingWallet: 10);
    round.changeBet(4);

    expect(round.bet, 5);
    expect(round.controls.draw, isFalse);
    expect(round.controls.transferToCash, isTrue);
  });

  test('choose-holds phase enables only DRAW and card holds', () {
    final round = PokerRound(startingCredits: 20, startingWallet: 10);

    round.startHand();

    final controls = round.controls;
    expect(controls.betMinus, isFalse);
    expect(controls.betPlus, isFalse);
    expect(controls.transferToCash, isFalse);
    expect(controls.cashOut, isFalse);
    expect(controls.addMoney, isFalse);
    expect(controls.draw, isTrue);
    expect(controls.holdCards, isTrue);
  });

  test('win-decision phase uses DRAW as collect-and-continue', () {
    final round = PokerRound(startingCredits: 20, startingWallet: 10)
      ..phase = RoundPhase.winDecision
      ..pendingWin = 25;

    final controls = round.controls;
    expect(controls.betMinus, isFalse);
    expect(controls.betPlus, isFalse);
    expect(controls.transferToCash, isFalse);
    expect(controls.cashOut, isFalse);
    expect(controls.addMoney, isFalse);
    expect(controls.draw, isTrue);
    expect(controls.holdCards, isFalse);
  });

  test('bonus-ready phase disables all six main-game controls', () {
    final round = PokerRound(startingCredits: 20, startingWallet: 10)
      ..phase = RoundPhase.bonusReady;

    final controls = round.controls;
    expect(controls.betMinus, isFalse);
    expect(controls.betPlus, isFalse);
    expect(controls.transferToCash, isFalse);
    expect(controls.cashOut, isFalse);
    expect(controls.addMoney, isFalse);
    expect(controls.draw, isFalse);
    expect(controls.holdCards, isFalse);
  });

  test('double-up phase disables all six main-game controls', () {
    final round = PokerRound(startingCredits: 20, startingWallet: 10)
      ..phase = RoundPhase.doubleUp;

    final controls = round.controls;
    expect(controls.betMinus, isFalse);
    expect(controls.betPlus, isFalse);
    expect(controls.transferToCash, isFalse);
    expect(controls.cashOut, isFalse);
    expect(controls.addMoney, isFalse);
    expect(controls.draw, isFalse);
    expect(controls.holdCards, isFalse);
  });

  test('result phase restores idle-style controls', () {
    final round = PokerRound(startingCredits: 20, startingWallet: 10)
      ..phase = RoundPhase.result;

    final controls = round.controls;
    expect(controls.betPlus, isTrue);
    expect(controls.transferToCash, isTrue);
    expect(controls.cashOut, isTrue);
    expect(controls.addMoney, isTrue);
    expect(controls.draw, isTrue);
  });
}
