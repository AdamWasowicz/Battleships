using System;
using Battleships.assets.ship;
using Battleships.assets.shared;
using Battleships.assets.gameControl;
using Battleships.assets.playable;


BattleshipBot bot1 = new BattleshipBot("Adam");
BattleshipBot bot2 = new BattleshipBot("Bob");
BattleshipsPlayer player1 = new BattleshipsPlayer("Kacper");

//If you want simulation then pass two Battleships objects
//If not then pass player and bot OR two players
//There is no good PvP Mode but i will add it soon
GameControl gameControl = new GameControl(bot1, bot2, 230, 1);
gameControl.StartFullGame();
