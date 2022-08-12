using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Battleships.assets.shared;
using Battleships.assets.ship;


namespace Battleships.assets.old
{
    public class GameControlOLD
    {
        //Players
        private int _currentTurn = 0;
        private ICanPlay _player1;
        private ICanPlay _player2;

        //Game State
        private Tuple<GridCoordinates, Ship>[] _player1GridsWithShips;        //<Place, Ref to ship>
        private Tuple<GridCoordinates, Ship>[] _player2GridsWithShips;        //<Place, Ref to ship>
        private List<Tuple<GridCoordinates, bool>> _shotsMade;                    //<Place, did something got hit?>
        private int _player1ShipsSunkAmount = 0;                                   //How many of Player1 ships got sunk
        private int _player2ShipsSunkAmount = 0;                                    //How many of Player2 ships got sunk
        private int _maxTurns = 100;
        private int _amountOfShipsOnOneSide;


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

        public Tuple<GridCoordinates, Ship>[] Player1GridsWithShips
        {
            get { return _player1GridsWithShips.ToArray(); }
        }

        public Tuple<GridCoordinates, Ship>[] Player2GridsWithShips
        {
            get { return _player2GridsWithShips.ToArray(); }
        }

        public List<Tuple<GridCoordinates, bool>> ShotsMade
        {
            get { return _shotsMade.ToList(); }
        }

        public int Player1ShipsSunkAmount
        {
            get { return _player1ShipsSunkAmount; }
        }

        public int Player2ShipsSunkAmount
        {
            get { return _player2ShipsSunkAmount; }
        }


        //Private methods
        private void Game()
        {
            while (_currentTurn <= _maxTurns
                && _player1ShipsSunkAmount < _amountOfShipsOnOneSide
                && _player2ShipsSunkAmount < _amountOfShipsOnOneSide)
            {
                NextTurn();
            }

            if (_currentTurn > _maxTurns)
                EndGameByTimeOut();

            if (_player1ShipsSunkAmount == _amountOfShipsOnOneSide || _player2ShipsSunkAmount == _amountOfShipsOnOneSide)
                EndGameByWin();
        }

        private void NextTurn()
        {
            if (_currentTurn % 2 == 1)
            {
                bool isHit = false;
                GridCoordinates shotCoordinates = _player1.MakeMove();

                for (int i = 0; i < _player2GridsWithShips.Length; i++)
                {
                    if (shotCoordinates.GetCombinedCoordinatesAsString == _player2GridsWithShips[i].Item1.GetCombinedCoordinatesAsString)
                    {
                        isHit = _player2GridsWithShips[i].Item2.TryHitShip(shotCoordinates);

                        if (_player2GridsWithShips[i].Item2.IsSunk())
                            _player2ShipsSunkAmount++;

                        _shotsMade.Add(new Tuple<GridCoordinates, bool>(shotCoordinates, true));
                    }
                }

                if (!isHit)
                    _shotsMade.Add(new Tuple<GridCoordinates, bool>(shotCoordinates, false));

            }
        }

        private void EndGameByTimeOut()
        {
            Console.WriteLine("Game exceded max turns");
        }

        private void EndGameByWin()
        {
            throw new NotImplementedException();
        }

        private string ShowGameRecap()
        {
            throw new NotImplementedException();
        }



        //Public methods
        public void StartGame()
        {
            Game();
        }



        public GameControlOLD(ICanPlay player1, ICanPlay player2, int maxTurns = 50)
        {
            _player1 = player1;
            _player2 = player2;
            _maxTurns = maxTurns;
        }
    }
}
