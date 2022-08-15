﻿using System;
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
        private List<List<Tuple<GridCoordinates, Ship>>> _gridsWithShips;               //<Place, Ship Ref>[...index][player1, player2]
        private List<List<Tuple<GridCoordinates, bool>>>  _shotsMade;                   //<Place, did something got hit?>[plater1, player2]
        private List<List<Ship>> _playerShips;                                          //Refs to player's ships [player1, player2]
        private int[] _playerShipsSunk = {0, 0};                                        //[player1, player2]
        private bool _gameFinished = false;                                             //Is game finished
        private bool _gameInProgress = false;                                           //Is game in progress
        private int _currentTurn = 0;                                                   //Current turn

        //Other
        private const int _maxAmountRandomShipPlacementTries = 20;                      //How many tries before brute force searchnig method 
        private int _amountOfShipsOnOneSide = ShipDetails.ReturnAllAsArray().Length;    //Number of ships for each player
        private int _verboseLevel = 0;                                                  //Verbose level
        private int _maxTurns = 100;                                                    //After _maxTurns exceded game will finish


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
            int playerIndex = _currentTurn % 2;
            int enemyIndex = (_currentTurn + 1) % 2;
            bool isHit = false;
            string playerName = "";
            GridCoordinates shotCoordinates = new GridCoordinates();


            if (playerIndex == 0)
            {
                shotCoordinates = _player1.MakeMove(_shotsMade[playerIndex], _playerShips[playerIndex]);
                playerName = _player1.GetPlayerName();
            }
            else
            {
                shotCoordinates = _player2.MakeMove(_shotsMade[playerIndex], _playerShips[playerIndex]);
                playerName = _player2.GetPlayerName();
            }


            for (int i = 0; i < _gridsWithShips[playerIndex].Count; i++)
            {
                //Hit
                if (shotCoordinates.GetCombinedCoordinatesAsString() == _gridsWithShips[enemyIndex][i].Item1.GetCombinedCoordinatesAsString())
                {
                    isHit = _gridsWithShips[enemyIndex][i].Item2.TryHitShip(shotCoordinates);

                    if (_gridsWithShips[enemyIndex][i].Item2.IsSunk())
                        _playerShipsSunk[enemyIndex]++;

                    //Verbose
                    if (_verboseLevel >= 1)
                        Console.WriteLine($"Player {playerName} at {shotCoordinates.GetCombinedCoordinatesAsString()}, it was a hit");

                    _shotsMade[playerIndex].Add(new Tuple<GridCoordinates, bool>(shotCoordinates, true));
                }
            }

            //Miss
            if (!isHit)
            {
                //Verbose
                if (_verboseLevel >= 1)
                    Console.WriteLine($"Player {playerName} at {shotCoordinates.GetCombinedCoordinatesAsString()}, it was a miss");

                _shotsMade[playerIndex].Add(new Tuple<GridCoordinates, bool>(shotCoordinates, false));
            }
        }

        private void EndGameByTimeOut()
        {
            _gameFinished = true;
            Console.WriteLine("Game exceded max turns");

            if (_playerShipsSunk[0] < _playerShipsSunk[1])
                Console.WriteLine($"!!! Player {_player1.GetPlayerName()} Won by having more ships !!!");
            else if (_playerShipsSunk[0] > _playerShipsSunk[1])
                Console.WriteLine($"!!! Player {_player2.GetPlayerName()} Won by having more ships !!!");
            else
                Console.WriteLine($"!!! TIE !!!");

            ShowGameRecap();
        }

        private void EndGameByWin()
        {
            _gameFinished = true;
            Console.WriteLine("Game finished");

            if (_playerShipsSunk[0] < _amountOfShipsOnOneSide)
                Console.WriteLine($"!!! Player {_player1.GetPlayerName()} Won !!!");
            else
                Console.WriteLine($"!!! Player {_player2.GetPlayerName()} Won !!!");

            ShowGameRecap();
        }

        private void ShowGameRecap()
        {
            for (int i = 0; i < 2; i++)
            {
                string playerName = i % 2 == 0 ? _player1.GetPlayerName() : _player2.GetPlayerName();
                Console.WriteLine($"Player {playerName}: ");

                int miss = 0;
                int hit = 0;

                foreach (var shot in _shotsMade[i])
                {
                    if (shot.Item2 == true)
                        hit++;
                    else
                        miss++;
                }

                float acc = (hit * 100) / (hit + miss);

                Console.WriteLine($"Accuracy: {acc}%");
                Console.WriteLine($"Hit: {hit} times");
                Console.WriteLine($"Miss: {miss} times");
                Console.WriteLine($"Ships left: {_amountOfShipsOnOneSide - _playerShipsSunk[i]}");
                Console.WriteLine();
            }
        }

        //Places each ship for each player
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
            Tuple<bool, GridCoordinates[]> verificationResultTuple = new Tuple<bool, GridCoordinates[]>(false, new GridCoordinates[] {});


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
            PlaceShipsMain();

            while (_currentTurn <= _maxTurns
                && _playerShipsSunk[0] < _amountOfShipsOnOneSide
                && _playerShipsSunk[1] < _amountOfShipsOnOneSide)
            {
                NextTurnAuto();
                _currentTurn++;
            }

            if (_playerShipsSunk[0] == _amountOfShipsOnOneSide || _playerShipsSunk[1] == _amountOfShipsOnOneSide)
                EndGameByWin();

            if (_currentTurn > _maxTurns)
                EndGameByTimeOut();
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



        public GameControl(ICanPlay player1, ICanPlay player2, int maxTurns = 50, int verboseLevel = 1)
        {
            _player1 = player1;
            _player2 = player2;
            _maxTurns = maxTurns;
            _verboseLevel = verboseLevel;

            _shotsMade = new List<List<Tuple<GridCoordinates, bool>>>(2)
            {
                new List<Tuple<GridCoordinates, bool>>(maxTurns),
                new List<Tuple<GridCoordinates, bool>>(maxTurns)
            };
            _playerShips = new List<List<Ship>>(2)
            {
                new List<Ship>(5),
                new List<Ship>(5)
            };
            _gridsWithShips = new List<List<Tuple<GridCoordinates, Ship>>>(2) 
            { 
                new List<Tuple<GridCoordinates, Ship>>(17),
                new List<Tuple<GridCoordinates, Ship>>(17),
            };
        }
    }
}
