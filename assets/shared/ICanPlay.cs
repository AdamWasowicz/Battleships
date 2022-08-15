using Battleships.assets.ship;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships.assets.shared
{
    public interface ICanPlay
    {
        string GetPlayerName();
        GridCoordinates MakeMove(List<Tuple<GridCoordinates, bool>> shotsTaken, List<Ship> playerShips);
        ICanPlay ReturnCopy();
    }
}
