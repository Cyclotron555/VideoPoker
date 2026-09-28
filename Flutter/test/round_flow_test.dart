import 'package:flutter_test/flutter_test.dart';
import 'package:red_black_poker/game/model/deck.dart';
import 'package:red_black_poker/game/model/hand_rank.dart';
import 'package:red_black_poker/game/model/playing_card.dart';
import 'package:red_black_poker/game/model/poker_round.dart';

void main() {
  PlayingCard c(int rank, CardSuit suit) =>
      PlayingCard(rank: rank, suit: suit, assetId: -1);

  test('DRAW starts a hand, deducts the current bet, and locks money controls', () {
    final deck = Deck.fixed(<PlayingCard>[
      c(2, CardSuit.hearts),
      c(3, CardSuit.spades),
      c(4, CardSuit.clubs),
      c(5, CardSuit.diamonds),
      c(7, CardSuit.hearts),
    ]);
    final round = PokerRound(
      deck: deck,
      startingCredits: 100,
      startingWallet: 50,
    );

    round.changeBet(4);
    expect(round.bet, 5);

    round.startHand();

    expect(round.phase, RoundPhase.chooseHolds);
    expect(round.cash, 95);
    expect(round.cards, hasLength(5));
    expect(round.canTransferToCash, isFalse);
    expect(round.canCashOut, isFalse);
    expect(round.canAddMoney, isFalse);
    expect(round.canChangeBet, isFalse);
  });

  test('second DRAW evaluates held cards and creates the correct pending win', () {
    final deck = Deck.fixed(<PlayingCard>[
      c(6, CardSuit.hearts),
      c(6, CardSuit.clubs),
      c(6, CardSuit.spades),
      c(7, CardSuit.diamonds),
      c(4, CardSuit.hearts),
    ]);
    final round = PokerRound(deck: deck, startingCredits: 100);

    round.changeBet(2);
    expect(round.bet, 3);

    round.startHand();
    for (var i = 0; i < 5; i++) {
      round.toggleHold(i);
    }
    round.drawReplacement();

    expect(round.result, HandRank.threeOfAKind);
    expect(round.pendingWin, 30);
    expect(round.phase, RoundPhase.winDecision);
    expect(round.hasPendingWin, isTrue);
  });

  test('collect moves a pending poker win into CASH', () {
    final deck = Deck.fixed(<PlayingCard>[
      c(6, CardSuit.hearts),
      c(6, CardSuit.clubs),
      c(6, CardSuit.spades),
      c(7, CardSuit.diamonds),
      c(4, CardSuit.hearts),
    ]);
    final round = PokerRound(deck: deck, startingCredits: 100);

    round.startHand();
    for (var i = 0; i < 5; i++) {
      round.toggleHold(i);
    }
    round.drawReplacement();

    expect(round.pendingWin, 10);
    expect(round.cash, 99);

    expect(round.collectWin(), 10);
    expect(round.cash, 109);
    expect(round.pendingWin, 0);
    expect(round.phase, RoundPhase.result);
  });

  test('a qualifying high pair fills the tenth zombie slot and opens bonus', () {
    final deck = Deck.fixed(<PlayingCard>[
      c(11, CardSuit.hearts),
      c(11, CardSuit.spades),
      c(3, CardSuit.clubs),
      c(6, CardSuit.diamonds),
      c(9, CardSuit.hearts),
    ]);
    final round = PokerRound(
      deck: deck,
      startingCredits: 100,
      bonusTarget: 10,
      bonusPrize: 25,
    )..bonusProgress = 9;

    round.startHand();
    for (var i = 0; i < 5; i++) {
      round.toggleHold(i);
    }
    round.drawReplacement();

    expect(round.result, HandRank.none);
    expect(round.pendingWin, 0);
    expect(round.bonusProgress, 10);
    expect(round.bonusReady, isTrue);
    expect(round.phase, RoundPhase.bonusReady);

    expect(round.beginBonus(), 25);
    expect(round.bonusProgress, 0);
    expect(round.phase, RoundPhase.doubleUp);
  });
}
