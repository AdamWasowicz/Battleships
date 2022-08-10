using Battleships.assets.shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships.assets.ship
{
    public struct ShipPart
    {
        private GridCoordinates _partCoordinates;
        private bool _isHit = false;


        public GridCoordinates PartCoordinates
        {
            get { return _partCoordinates; }
        }

        public bool IsHit
        {
            get { return _isHit; }
        }


        public bool TryHitShipPart(string partCoordinates)
        {
            if (partCoordinates == _partCoordinates.GetCombinedCoordinatesAsString()
                ||
                partCoordinates == _partCoordinates.GetCombinedCoordinatesAsStringReverse())
            {
                _isHit = true;
                return true;
            }

            return false;
        }


        public ShipPart(GridCoordinates partCoordinates)
        {
            _partCoordinates = new GridCoordinates(partCoordinates);
        }

        public ShipPart(string partCoordinates)
        {
            string[] splitCoordinates = { partCoordinates.Substring(0, 1), partCoordinates.Substring(1) };
            _partCoordinates = new GridCoordinates(splitCoordinates[0], splitCoordinates[1]);
        }
    }


    public class Ship
    {
        private int _size;
        private int _damagedGrids = 0;
        private string _className;
        private ShipPart[] _shipParts;


        public int Size
        {
            get { return _size; }
        }

        public string ClassName
        {
            get { return _className; }
        }

        public int DamagedGrids
        {
            get { return _damagedGrids; }
        }


        //Private methods
        private void IncreaseDamagedGrids()
        {
            _damagedGrids++;
        }


        //Public methods
        public bool IsSunk()
        {
            if (_damagedGrids == _size)
                return true;

            return false;
        }

        public bool TryHitShip(string coordinates)
        {
            foreach (var part in _shipParts)
            {
                if (part.TryHitShipPart(coordinates))
                {
                    IncreaseDamagedGrids();
                    return true;
                }
            }

            return false;
        }


        public Ship(string className, GridCoordinates[] gridCoordinates)
        {
            //Create ship parts
            ShipPart[] parts = new ShipPart[gridCoordinates.Length];
            for (int i = 0; i < gridCoordinates.Length; i++)
                parts[i] = new ShipPart(gridCoordinates[i]);

            _shipParts = parts;
            _size = gridCoordinates.Length;
            _className = className;
        }

        public Ship(string className, string[] coordinates)
        {
            ShipPart[] parts = new ShipPart[coordinates.Length];
            for (int i = 0; i < coordinates.Length; i++)
                parts[i] = new ShipPart(coordinates[i]);

            _shipParts = parts;
            _size = coordinates.Length;
            _className = className;
        }
    }
}
