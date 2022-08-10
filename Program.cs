using System;
using Battleships.assets.ship;
using Battleships.assets.shared;

ShipSimple ship2 = new ShipSimple(ShipClassNames.DESTROYER,
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
