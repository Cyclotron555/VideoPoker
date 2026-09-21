import 'package:flutter_test/flutter_test.dart';
import 'package:red_black_poker/game/model/poker_round.dart';

void main() {
  test('insert coins moves value from wallet to credits without changing bank', () {
    final round = PokerRound(startingCredits: 100, startingWallet: 4412);

    expect(round.bank, 4512);
    expect(round.insertCoins(10), 10);
    expect(round.credits, 110);
    expect(round.wallet, 4402);
    expect(round.bank, 4512);
  });

  test('cash out moves machine credits back to wallet', () {
    final round = PokerRound(startingCredits: 100, startingWallet: 4412);

    expect(round.cashOut(), 100);
    expect(round.credits, 0);
    expect(round.wallet, 4512);
    expect(round.bank, 4512);
  });

  test('refill wallet appears only when wallet is empty', () {
    final round = PokerRound(startingCredits: 25, startingWallet: 50);

    expect(round.canRefillWallet, isFalse);
    round.insertCoins(50);
    expect(round.wallet, 0);
    expect(round.canRefillWallet, isTrue);

    expect(round.refillWallet(), 50);
    expect(round.wallet, 50);
    expect(round.canRefillWallet, isFalse);
  });

  test('bet controls stay within one through five', () {
    final round = PokerRound();

    round.changeBet(-10);
    expect(round.bet, 1);

    round.changeBet(99);
    expect(round.bet, 5);
  });
}
