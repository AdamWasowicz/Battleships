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
        private List<List<Tuple<GridCoordinates, Ship>>> _gridsWithShips;           //<Place, Ship Ref>[...index][player1, player2]
        private List<Tuple<GridCoordinates, bool>>  _shotsMade;                     //<Place, did something got hit?>[plater1, player2]
        private List<List<Ship>> _playerShips;                                       //Refs to player's ships [player1, player2]
        private int[] _playerShipsSunk = {0, 0};                                    //[player1, player2]
        private bool _gameFinished = false;                                         //Is game finished
        private bool _gameInProgress = false;                                       //Is game in progress
        private int _currentTurn = 0;                                               //Current turn

        //Other
        private const int _maxAmountRandomShipPlacementTries = 20;                  //How many tries before brute force searchnig method 
        private const int _amountOfShipsOnOneSide = 5;                              //Number of ships for each player
        private int _verboseLevel = 0;                                              //Verbose level
        private int _maxTurns = 100;                                                //After _maxTurns exceded game will finish


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

        public int Player1ShipsSunkAmount
        {
            get { return _playerShipsSunk[0]; }
        }

        public int Player2ShipsSunkAmount
        {
            get { return _playerShipsSunk[1]; }
        }


        //Private methods
        private void NextTurnAuto()
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
                if (shotCoordinates.GetCombinedCoordinatesAsString() == _gridsWithShips[playerIndex][i].Item1.GetCombinedCoordinatesAsString())
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
            _gameFinished = true;
            Console.WriteLine("Game exceded max turns");

            throw new NotImplementedException();
        }

        private void EndGameByWin()
        {
            _gameFinished = true;
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

        public void PlaceShipsMain()
        {
            Tuple<string, int>[] shipDetails = ShipDetails.ReturnAllAsArray();

            for (int i = 0; i < 2; i++)
            {
                for (int d = 0; d < shipDetails.Length; d++)
                {
                    List<GridCoordinates> takenGrids = new List<GridCoordinates>();
                    foreach (var coord in _gridsWithShips[i])
                        takenGrids.Add(coord.Item1);

                    PlaceShipRandomlyForPlayer(shipDetails[d].Item2, takenGrids, shipDetails[d].Item1, i);
                }
            }
        }

        //Gets free grids from VerifyFreeGrids then adds them to _gridsWithShips with new Ship
        private void PlaceShipRandomlyForPlayer(int size, List<GridCoordinates> takenGrids, string shipName, int playerIndex)
        {
            const int maxCoordinateOffset = 10;
            bool placeFound = false;
            Tuple<bool, GridCoordinates[]> verificationResultTuple = new Tuple<bool, GridCoordinates[]>(false, null);


            for (int i = 0; i < _maxAmountRandomShipPlacementTries && !placeFound; i++)
            {
                int xOffset = new Random().Next(0, maxCoordinateOffset + 1);
                int yOffset = new Random().Next(1, maxCoordinateOffset + 1);

                char xCoord = Convert.ToChar(Convert.ToInt32('A') + xOffset);
                int yCoord = yOffset;
                GridCoordinates selectedCoordinates = new GridCoordinates(xCoord, yCoord);


                //First try should use random direction
                int firstRandomDirection = new Random().Next(0, 4);
                verificationResultTuple = VerifyFreeGridsAndReturnThem(takenGrids, selectedCoordinates, size, (Direction)firstRandomDirection);

                //If Random direction did not fint valid place then search in every direction
                for (int d = 0; d < 4 && !verificationResultTuple.Item1; d++)
                    verificationResultTuple = VerifyFreeGridsAndReturnThem(takenGrids, selectedCoordinates, size, (Direction)d);
                
                //If place found then set flag to true
                if (verificationResultTuple.Item1 == true)
                    placeFound = true;
            }

            if (!placeFound)
            {
                //Brute force method
            }

            //Add ships to _playerShip
            Ship newShip = new Ship(shipName, verificationResultTuple.Item2);
            _playerShips[playerIndex].Add(newShip);

            //Add new record to _gridsWithShip
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
            else                                    //Direction.BOTTOM
                yOffsetValue = -1;


            for (int i = 0; i < size; i++)
            {
                char xCoord = Convert.ToChar(Convert.ToInt32(coordinates.X) + xOffsetValue * i);
                int yCoord = Convert.ToInt32(coordinates.Y) + yOffsetValue * i;

                //Check if cordinates didn't go out of bounds
                //If taken then return false and GridCoordinates[]
                if (xCoord < 'A' || yCoord <= 0 || xCoord > 'J' || yCoord >= 11)
                    return new Tuple<bool, GridCoordinates[]>(false, newTakenGridCoordinates);

                //Check if grids overlap
                foreach (var takenGrid in takenGrids)
                {
                    //Check if Coordinate is taken
                    //If taken then return false and GridCoordinates[]
                    if (takenGrid.GetCombinedCoordinatesAsString() == new GridCoordinates(xCoord, yCoord).GetCombinedCoordinatesAsString())
                        return new Tuple<bool, GridCoordinates[]>(false, newTakenGridCoordinates);                                    
                }

                //Add new valid Coordinates
                newTakenGridCoordinates[i] = new GridCoordinates(xCoord, yCoord);
            }


            return new Tuple<bool, GridCoordinates[]>(true, newTakenGridCoordinates); ;
        }


        //Public methods
        public void StartFullGame()
        {
            PlaceShips();

            while (_currentTurn <= _maxTurns
                && _playerShipsSunk[0] < _amountOfShipsOnOneSide
                && _playerShipsSunk[1] < _amountOfShipsOnOneSide)
            {
                NextTurnAuto();
                _currentTurn++;
            }

            if (_currentTurn > _maxTurns)
                EndGameByTimeOut();

            if (_playerShipsSunk[0] == _amountOfShipsOnOneSide || _playerShipsSunk[1] == _amountOfShipsOnOneSide)
                EndGameByWin();
        }

        public void DisplayPlayerShipsWithCoordinates(int playerIndex) {

            Console.WriteLine($"Player { playerIndex + 1 } ships:");

            foreach (var ship in _playerShips[playerIndex])
            {
                Console.WriteLine(ship.ClassName);
                foreach (var grid in ship.GetShipPartsCoordinatesAsGridCoordinatesArray())
                    Console.WriteLine(grid.GetCombinedCoordinatesAsString());
                Console.WriteLine();
            }
        }

        public void DisplayPlayerTakenGridsSorted(int playerIndex)
        {
            var listTuple = _gridsWithShips[playerIndex];
            var listString = new List<string>();
            foreach (var tuple in listTuple)
                listString.Add(tuple.Item1.GetCombinedCoordinatesAsString());

            listString.Sort();

            Console.WriteLine($"Player { playerIndex + 1 } taken grids:");
            foreach (var stringValue in listString)
                Console.WriteLine(stringValue);
        }



        public GameControl(ICanPlay player1 = null, ICanPlay player2 = null, int maxTurns = 50, int verboseLevel = 0)
        {
            _player1 = player1;
            _player2 = player2;
            _maxTurns = maxTurns;
            _verboseLevel = verboseLevel;

            _shotsMade = new List<Tuple<GridCoordinates, bool>>(maxTurns);
            _playerShips = new List<List<Ship>>()
            {
                new List<Ship>(),
                new List<Ship>()
            };
            _gridsWithShips = new List<List<Tuple<GridCoordinates, Ship>>>(2) 
            { 
                new List<Tuple<GridCoordinates, Ship>>(17),
                new List<Tuple<GridCoordinates, Ship>>(17),
            };
        }
    }
}
