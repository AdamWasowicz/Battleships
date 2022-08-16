using Battleships.assets.ship;


namespace Battleships.assets.shared
{
    public interface ICanPlay
    {
        string GetPlayerName();
        GridCoordinates MakeMove(List<Tuple<GridCoordinates, bool>> shotsTaken, List<Ship> playerShips);
        ICanPlay ReturnCopy();
    }
}
