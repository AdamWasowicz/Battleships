using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Battleships.assets.shared;
using Battleships.assets.ship;


namespace Battleships.assets.playable
{
    public class BattleshipsPlayer : ICanPlay
    {
        private string _playerName;
        private List<Tuple<GridCoordinates, bool>> _shotsTakenCopy = null;


        //Private methods
        private bool ValidateInput(string stringCoordinates)
        {
            char X;
            int Y;


            if (stringCoordinates.Length == 0)
                return false;
            if (stringCoordinates.Length > 3)
                return false;


            //X
            try { X = Convert.ToChar(stringCoordinates.Substring(0, 1)[0]); }
            catch { return false; }

            //Y
            try { Y = Convert.ToInt32(stringCoordinates.Substring(1)); }
            catch { return false; }

            //GridCoordinates
            try { GridCoordinates.ValidateParams(X, Y); }
            catch { return false; }


            return true;
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


        public GridCoordinates MakeMove(List<Tuple<GridCoordinates, bool>> shotsTaken, List<Ship> playerShips)
        {
            //Make Copy
            CopyShotsTaken(shotsTaken);


            bool validInput = false;
            string stringCoordinates = "";

            while (!validInput)
            {
                Console.Write($"Player {_playerName} move: ");
                stringCoordinates = Console.ReadLine();

                validInput = ValidateInput(stringCoordinates);

                if (!validInput)
                    Console.WriteLine($"Input {stringCoordinates} is invalid, try again");
            }

            GridCoordinates coordinates = new GridCoordinates(
                stringCoordinates.Substring(0, 1)[0],
                Convert.ToInt32(stringCoordinates.Substring(1))
                );

            return coordinates;
        }

        //Public methods
        public string GetPlayerName()
        {
            return _playerName.ToString();
        }

        public ICanPlay ReturnCopy()
        {
            return new BattleshipsPlayer(_playerName);
        }



        public BattleshipsPlayer(string playerName)
        {
            _playerName = playerName;
        }
    }
}
