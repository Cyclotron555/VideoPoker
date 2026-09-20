# Red Black Poker — Flutter + Flame port

The original WPF game remains untouched under `Poker/`.

This directory contains the new cross-platform Flutter + Flame implementation.

## Port rules

- Reuse the original 53 card-face PNGs (Joker + 52 cards).
- Do **not** reuse the old backgrounds. New backgrounds and table presentation will be designed for the Flutter version.
- Keep poker rules in plain Dart, separate from Flame rendering.
- Preserve the original Red Black Poker pay table and one-Joker deck.
- Replace the old positional Auto Hold code with exhaustive hold-mask analysis so valid combinations cannot disappear because a hand-written permutation was missed.

## Legacy card asset numbering

- `0.png` = Joker
- `1..13` = Hearts (Ace through King)
- `14..26` = Diamonds
- `27..39` = Spades
- `40..52` = Clubs

## Phase 1

- Flutter/Flame shell
- Original card art
- 53-card deck model
- Joker-aware hand evaluator
- Original pay table
- regression tests for hand recognition

Next: expected-value Auto Hold, animations, sounds, new background/table design, and Red/Black double-up.
