import 'package:flame/game.dart';
import 'package:flutter/material.dart';

import 'game/red_black_poker_game.dart';

void main() {
  WidgetsFlutterBinding.ensureInitialized();
  runApp(const RedBlackPokerApp());
}

class RedBlackPokerApp extends StatelessWidget {
  const RedBlackPokerApp({super.key});

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      debugShowCheckedModeBanner: false,
      title: 'Red Black Poker',
      theme: ThemeData.dark(useMaterial3: true),
      home: Scaffold(
        body: SafeArea(
          child: GameWidget.controlled(
            gameFactory: RedBlackPokerGame.new,
          ),
        ),
      ),
    );
  }
}
