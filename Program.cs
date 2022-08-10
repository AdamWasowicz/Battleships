using System;
using Battleships.assets.ship;

Ship ship2 = new Ship("Small boat",
    new string[]
    {
        "A2",
        "A3",
        "A4"
    });

Console.WriteLine(ship2.TryHitShip("B2"));
Console.WriteLine(ship2.DamagedGrids);
Console.WriteLine(ship2.IsSunk());

Console.WriteLine();

Console.WriteLine(ship2.TryHitShip("A2"));
Console.WriteLine(ship2.DamagedGrids);
Console.WriteLine(ship2.IsSunk());
