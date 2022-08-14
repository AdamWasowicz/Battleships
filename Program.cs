using System;
using Battleships.assets.ship;
using Battleships.assets.shared;
using Battleships.assets.gameControl;


GameControl gameControl = new GameControl();
gameControl.PlaceShipsMain();

gameControl.DisplayPlayerShipsWithCoordinates(0);
gameControl.DisplayPlayerTakenGridsSorted(0);

Console.WriteLine();

gameControl.DisplayPlayerShipsWithCoordinates(1);
gameControl.DisplayPlayerTakenGridsSorted(1);
