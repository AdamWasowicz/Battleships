using System;
using Battleships.assets.ship;
using Battleships.assets.shared;

Ship ship2 = new Ship(ShipDetails.CARRIER.Item1,
    new GridCoordinates[]
    {
        new GridCoordinates("A", "1"),
        new GridCoordinates("A", "2"),
        new GridCoordinates("A", "3"),
        new GridCoordinates("A", "4")
    });

Console.WriteLine(ship2.TryHitShip(new GridCoordinates("A", "8")));
Console.WriteLine(ship2.DamagedGrids);
Console.WriteLine(ship2.IsSunk());

Console.WriteLine();

Console.WriteLine(ship2.TryHitShip(new GridCoordinates("A", "1")));
Console.WriteLine(ship2.DamagedGrids);
Console.WriteLine(ship2.IsSunk());
