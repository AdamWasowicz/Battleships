using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Battleships.assets.shared;
using Battleships.assets.ship;


namespace Battleships.assets.gameControl
{
    public enum Direction
    {
        LEFT,
        TOP,
        RIGHT,
        BOTTOM
    }

    public class GameControl
    {
        //Players
        private ICanPlay _player1;
        private ICanPlay _player2;

        //Game State
        private List<List<Tuple<GridCoordinates, Ship>>> _gridsWithShips;          //<Place, Ship Ref>[...index][player1, player2]
        private List<Tuple<GridCoordinates, bool>>  _shotsMade;                    //<Place, did something got hit?>[plater1, player2]
        private int[] _playerShipsSunk = {0, 0};                                   //[player1, player2]
        private int _maxTurns = 100;
        private int _amountOfShipsOnOneSide;

        //Other
        private int _currentTurn = 0;
        private int _verboseLevel = 0;
        private bool _gameFinished = false;


        //GET
        public int CurrentTurn
        {
            get { return _currentTurn; }
        }

        public ICanPlay Player1
        {
            get { return _player1.ReturnCopy(); }
        }

        public ICanPlay Player2
        {
            get { return _player2.ReturnCopy(); }
        }

        public List<Tuple<GridCoordinates, bool>> ShotsMade
        {
            get { return _shotsMade.ToList(); }
        }

        public int Player1ShipsSunkAmount
        {
            get { return _playerShipsSunk[0]; }
        }

        public int Player2ShipsSunkAmount
        {
            get { return _playerShipsSunk[1]; }
        }


        //Private methods
        private void Game()
        {
            PlaceShips();

            while (_currentTurn <= _maxTurns 
                && _playerShipsSunk[0] < _amountOfShipsOnOneSide 
                && _playerShipsSunk[1] < _amountOfShipsOnOneSide)
            {
                NextTurn();
                _currentTurn++;
            }

            if (_currentTurn > _maxTurns)
                EndGameByTimeOut();

            if (_playerShipsSunk[0] == _amountOfShipsOnOneSide || _playerShipsSunk[1] == _amountOfShipsOnOneSide)
                EndGameByWin();
        }

        private void NextTurn()
        {
            //Select Player
            int playerIndex = 0;
            if (_currentTurn % 2 == 0)
                playerIndex = 1;
            else
                playerIndex = 0;

            
            bool isHit = false;
            GridCoordinates shotCoordinates = _player1.MakeMove();

            for (int i = 0; i < _gridsWithShips[playerIndex].Count; i++)
            {
                if (shotCoordinates.GetCombinedCoordinatesAsString == _gridsWithShips[playerIndex][i].Item1.GetCombinedCoordinatesAsString)
                {
                    isHit = _gridsWithShips[playerIndex][i].Item2.TryHitShip(shotCoordinates);

                    if (_gridsWithShips[playerIndex][i].Item2.IsSunk())
                        _playerShipsSunk[playerIndex]++;

                    _shotsMade.Add(new Tuple<GridCoordinates, bool>(shotCoordinates, true));
                }
            }

            if (!isHit)
                _shotsMade.Add(new Tuple<GridCoordinates, bool>(shotCoordinates, false));
        }

        private void EndGameByTimeOut()
        {
            Console.WriteLine("Game exceded max turns");
        }

        private void EndGameByWin()
        {
            ShowGameRecap();
            throw new NotImplementedException();
        }

        private string ShowGameRecap()
        {
            throw new NotImplementedException();
        }

        private void PlaceShips()
        {
            throw new NotImplementedException();
        }

        private void PlaceShipsMain()
        {
            Tuple<string, int>[] shipDetails = ShipDetails.ReturnAllAsArray();

            //CONTINUE HERE
        }

        //Gets free grids from VerifyFreeGrids then adds them to _gridsWithShips with new Ship
        private void PlaceShipRandomlyForPlayer(int size, List<GridCoordinates> takenGrids, string shipName, int playerIndex)
        {
            const int maxTry = 5;
            const int maxCoordinateOffset = 10;
            bool placeFound = false;
            Tuple<bool, GridCoordinates[]> verificationResultTuple = new Tuple<bool, GridCoordinates[]>(false, null);


            for (int i = 0; i < maxTry; i++)
            {
                int xOffset = new Random().Next(0, maxCoordinateOffset + 1);
                int yOffset = new Random().Next(0, maxCoordinateOffset + 1);

                string xCoord = Convert.ToChar(Convert.ToInt32('A') + xOffset) + "";
                string yCoord = Convert.ToString(yOffset);

                GridCoordinates selectedCoordinates = new GridCoordinates(xCoord, yCoord);


                verificationResultTuple = VerifyFreeGridsAndReturnThem(takenGrids, selectedCoordinates, size, Direction.LEFT);

                if (verificationResultTuple.Item1 == false)
                    verificationResultTuple = VerifyFreeGridsAndReturnThem(takenGrids, selectedCoordinates, size, Direction.TOP);

                else if (verificationResultTuple.Item1 == false)
                    verificationResultTuple = VerifyFreeGridsAndReturnThem(takenGrids, selectedCoordinates, size, Direction.RIGHT);

                else
                    verificationResultTuple = VerifyFreeGridsAndReturnThem(takenGrids, selectedCoordinates, size, Direction.BOTTOM);

                if (verificationResultTuple.Item1 == true)
                    placeFound = true;
            }

            if (!placeFound)
            {
                //Brute force method
            }

            //Add new gridsWithShips
            Ship newShip = new Ship(shipName, verificationResultTuple.Item2);
            foreach (var coord in verificationResultTuple.Item2)
                _gridsWithShips[playerIndex].Add(new Tuple<GridCoordinates, Ship>(coord, newShip));
            
        }

        private Tuple<bool, GridCoordinates[]> VerifyFreeGridsAndReturnThem(List<GridCoordinates> takenGrids, GridCoordinates coordinates, int size, Direction direction)
        {
            GridCoordinates[] newTakenGridCoordinates = new GridCoordinates[size];
            int xOffsetValue = 0;
            int yOffsetValue = 0;


            if (direction == Direction.LEFT)
                xOffsetValue = -1;
            else if (direction == Direction.TOP)
                yOffsetValue = -1;
            else if (direction == Direction.RIGHT)
                xOffsetValue = 1;
            else
                yOffsetValue = -1;


            for (int i = 0; i < size; i++)
            {
                string xCoord = Convert.ToChar(coordinates.X + xOffsetValue * i) + "";
                string yCoord = Convert.ToChar(coordinates.Y + yOffsetValue * i) + "";

                foreach (var takenGrid in takenGrids)
                {
                    if (takenGrid.GetCombinedCoordinatesAsString == new GridCoordinates(xCoord, yCoord).GetCombinedCoordinatesAsString) 
                        return new Tuple<bool, GridCoordinates[]>(false, newTakenGridCoordinates);
                    
                    else
                        newTakenGridCoordinates[i] = new GridCoordinates(xCoord, yCoord);
                }
            }

            return new Tuple<bool, GridCoordinates[]>(true, newTakenGridCoordinates); ;
        }
      

        //Public methods
        public void StartGame()
        {
            Game();
        }


        public GameControl(ICanPlay player1, ICanPlay player2, int maxTurns = 50, int verboseLevel = 0)
        {
            _player1 = player1;
            _player2 = player2;
            _maxTurns = maxTurns;
            _verboseLevel = verboseLevel;

            _shotsMade = new List<Tuple<GridCoordinates, bool>>(maxTurns);
            _gridsWithShips = new List<List<Tuple<GridCoordinates, Ship>>>(2);
        }
    }
}
