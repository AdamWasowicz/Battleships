using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Battleships.assets.shared;
using Battleships.assets.ship;

namespace Battleships.assets.playable
{
    public class BattleshipBot : ICanPlay
    {
        private string _botName;
        private List<Tuple<GridCoordinates, bool>> _shotsTakenCopy = null;
        private const int _maxTriesNumber = 10;


        //Private methods
        private GridCoordinates MakeRandomMove(List<Tuple<GridCoordinates, bool>> shotsTaken)
        {
            const int maxCoordinateOffset = 10;
            int xOffset = new Random().Next(0, maxCoordinateOffset + 1);
            int yOffset = new Random().Next(1, maxCoordinateOffset + 1);

            char xCoord = Convert.ToChar(Convert.ToInt32('A') + xOffset);
            int yCoord = yOffset;

            GridCoordinates selectedCoordinates = new GridCoordinates(xCoord, yCoord);

            return selectedCoordinates;
        }

        private void CopyShotsTaken(List<Tuple<GridCoordinates, bool>> shotsTaken)
        {
            //If _shotsTakenCopy exists then add last record from shotsTaken
            //it contains only new record
            if (_shotsTakenCopy != null)
            {
                _shotsTakenCopy.Add(shotsTaken[shotsTaken.Count - 1]);
                return;
            }

            List<Tuple<GridCoordinates, bool>> copy = new List<Tuple<GridCoordinates, bool>>(shotsTaken.Count);

            foreach (var shot in shotsTaken)
                copy.Add(new Tuple<GridCoordinates, bool>(new GridCoordinates(shot.Item1.X, shot.Item1.Y), shot.Item2));

            _shotsTakenCopy = copy;   
        }


        public string GetPlayerName()
        {
            return _botName.ToString();
        }

        //Public methods
        public ICanPlay ReturnCopy()
        {
            return new BattleshipBot(_botName);
        }

        public GridCoordinates MakeMove(List<Tuple<GridCoordinates, bool>> shotsTaken, List<Ship> playerShips)
        {
            //Copy
            CopyShotsTaken(shotsTaken);

            GridCoordinates shot = new GridCoordinates();

            for (int i = 0; i < _maxTriesNumber; i++)
            {
                bool shotAlreadyExists = false;
                shot = MakeRandomMove(_shotsTakenCopy);

                foreach (var takenShoot in _shotsTakenCopy)
                {
                    if (shot.GetCombinedCoordinatesAsString() == takenShoot.Item1.GetCombinedCoordinatesAsString())
                    {
                        shotAlreadyExists = true;
                        break;
                    }
                }

                if (shotAlreadyExists == false)
                    return shot;
            }
            
            return MakeRandomMove(_shotsTakenCopy);
        }

        


        public BattleshipBot(string botName)
        {
            _botName = botName;
        }
    }
}
