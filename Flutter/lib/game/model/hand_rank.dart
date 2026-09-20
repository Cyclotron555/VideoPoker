enum HandRank {
  none('NO WIN', 0),
  twoPair('TWO PAIR', 5),
  threeOfAKind('THREE OF A KIND', 10),
  straight('STRAIGHT', 15),
  flush('FLUSH', 20),
  fullHouse('FULL HOUSE', 25),
  fourOfAKind('FOUR OF A KIND', 100),
  straightFlush('STRAIGHT FLUSH', 250),
  royalFlush('ROYAL FLUSH', 1000),
  fiveOfAKind('FIVE OF A KIND', 5000);

  const HandRank(this.label, this.basePayout);

  final String label;
  final int basePayout;
}
