enum GameThemeId {
  classic,
  halloween,
  christmas,
}

class GameThemeDefinition {
  const GameThemeDefinition({
    required this.id,
    required this.displayName,
    required this.bonusTokenName,
    required this.bonusTarget,
    required this.redBlackTransitionStyle,
  });

  final GameThemeId id;
  final String displayName;
  final String bonusTokenName;
  final int bonusTarget;
  final String redBlackTransitionStyle;
}

const halloweenTheme = GameThemeDefinition(
  id: GameThemeId.halloween,
  displayName: 'Halloween',
  bonusTokenName: 'Zombie Head',
  bonusTarget: 10,
  redBlackTransitionStyle: 'zombie-head-release-stone-doors',
);

const classicTheme = GameThemeDefinition(
  id: GameThemeId.classic,
  displayName: 'Classic',
  bonusTokenName: 'Bonus Point',
  bonusTarget: 10,
  redBlackTransitionStyle: 'classic-red-black',
);
