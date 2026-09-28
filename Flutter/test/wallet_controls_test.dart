import 'package:flutter_test/flutter_test.dart';
import 'package:red_black_poker/game/model/hand_rank.dart';
import 'package:red_black_poker/game/model/poker_round.dart';

void main() {
  test('transfer to cash moves value from wallet to cash without changing total funds', () {
    final round = PokerRound(startingCredits: 100, startingWallet: 4412);

    expect(round.bank, 4512);
    expect(round.transferToCash(), 5);
    expect(round.cash, 105);
    expect(round.wallet, 4407);
    expect(round.bank, 4512);
  });

  test('add money adds ten to wallet by default', () {
    final round = PokerRound(startingCredits: 100, startingWallet: 550);

    expect(round.addMoney(), 10);
    expect(round.wallet, 560);
    expect(round.cash, 100);
  });

  test('cash out moves all machine cash back to wallet', () {
    final round = PokerRound(startingCredits: 100, startingWallet: 4412);

    expect(round.cashOut(), 100);
    expect(round.cash, 0);
    expect(round.wallet, 4512);
    expect(round.bank, 4512);
  });

  test('refill wallet appears only when wallet is empty', () {
    final round = PokerRound(startingCredits: 25, startingWallet: 50);

    expect(round.canRefillWallet, isFalse);
    round.transferToCash(50);
    expect(round.wallet, 0);
    expect(round.canRefillWallet, isTrue);

    expect(round.refillWallet(), 50);
    expect(round.wallet, 50);
    expect(round.canRefillWallet, isFalse);
  });

  test('bet controls stay within one through ten', () {
    final round = PokerRound();

    round.changeBet(-10);
    expect(round.bet, PokerRound.minBet);

    round.changeBet(99);
    expect(round.bet, PokerRound.maxBet);
  });

  test('paytable values equal base payout times current bet', () {
    final round = PokerRound();

    round.changeBet(1);
    expect(round.bet, 2);
    expect(round.payoutFor(HandRank.fiveOfAKind), 10000);
    expect(round.payoutFor(HandRank.royalFlush), 2000);
    expect(round.payoutFor(HandRank.twoPair), 10);
    expect(round.highPairPaytableValue, 10);

    round.changeBet(8);
    expect(round.bet, 10);
    expect(round.payoutFor(HandRank.fiveOfAKind), 50000);
    expect(round.payoutFor(HandRank.threeOfAKind), 100);
    expect(round.highPairPaytableValue, 50);
  });

  test('bet cannot change while a hand is active', () {
    final round = PokerRound(startingCredits: 100);

    round.changeBet(4);
    expect(round.bet, 5);

    round.startHand();
    round.changeBet(3);

    expect(round.bet, 5);
  });
}
