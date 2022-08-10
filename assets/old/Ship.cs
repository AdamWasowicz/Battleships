using Battleships.assets.shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Battleships.assets.old
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


        //Private methods
        private static void ValidateArgument(string partCoordinates)
        {
            if (partCoordinates.Length == 0)
                throw new InvalidDataException("Invalid Coordinates, Coordinates cannot be an empty string");
            if (partCoordinates.Length < 2)
                throw new InvalidDataException("Invalid Coordinates, Coordinates must have at least two characters");
        }


        //Public methods
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
            ValidateArgument(partCoordinates);

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

        private void ValidateCoordinates(GridCoordinates[] gridCoordinates)
        {
            ValidateCoordinatesWithGridCoordinates(gridCoordinates);
        }

        private void ValidateCoordinates(string[] coordinates)
        {

        }

        private void ValidateCoordinatesWithGridCoordinates(GridCoordinates[] gridCoordinates)
        {
            List<string> arrayOfX = new List<string>(gridCoordinates.Length);
            List<int> arrayOfY = new List<int>(gridCoordinates.Length);
            bool inRowX = true;
            bool inRowY = true;
            bool validPlacementX = true;
            bool validPlacementY = true;

            foreach (var gridCoords in gridCoordinates)
            {
                arrayOfX.Add(gridCoords.X);
                arrayOfY.Add(int.Parse(gridCoords.Y));
            }

            arrayOfX.Sort();
            arrayOfY.Sort();

            //inRow
            for (int i = 0; i < arrayOfX.Count - 1; i++)
            {
                if (arrayOfX[i] != arrayOfX[i + 1])
                    inRowX = false;

                if (arrayOfY[i] != arrayOfY[i + 1])
                    inRowY = false;
            }

            if (inRowX && inRowY)
                throw new InvalidDataException("Ship cannot be placed at an angle");

            if (!inRowX && !inRowY)
                throw new InvalidDataException("This ship position in invalid");


            if (inRowX)
            {
                for (int i = 0; i < arrayOfY.Count - 1; i++)
                {
                    if (arrayOfY[i] - arrayOfY[i + 1] != -1)
                    {
                        validPlacementY = false;
                        break;
                    }
                }

                if (!validPlacementY)
                    throw new InvalidDataException("Invalid ship placement, Y position is invalid");
            }


            if (inRowY)
            {
                for (int i = 0; i < arrayOfX.Count - 1; i++)
                {
                    if (arrayOfX[i][0] - arrayOfX[i + 1][0] != -1)
                    {
                        validPlacementX = false;
                        break;
                    }
                }

                if (!validPlacementX)
                    throw new InvalidDataException("Invalid ship placement, X position is invalid");
            }
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

        public GridCoordinates[] GetShipPartsCoordinatesAsGridCoordinatesArray()
        {
            GridCoordinates[] gridCoordinates = new GridCoordinates[_size];
            for (int i = 0; i < _size; i++)
                gridCoordinates[i] = _shipParts[i].PartCoordinates;

            return gridCoordinates;
        }

        public string[] GetShipPartsCoordinatesAsStringArray()
        {
            string[] stringCoordinates = new string[_size];
            for (int i = 0; i < _size; i++)
                stringCoordinates[i] = _shipParts[i].PartCoordinates.GetCombinedCoordinatesAsString();

            return stringCoordinates;
        }


        public Ship(string className, GridCoordinates[] gridCoordinates)
        {
            ValidateCoordinates(gridCoordinates);

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
            //Create ship parts
            ShipPart[] parts = new ShipPart[coordinates.Length];
            for (int i = 0; i < coordinates.Length; i++)
                parts[i] = new ShipPart(coordinates[i]);

            _shipParts = parts;
            _size = coordinates.Length;
            _className = className;
        }
    }
}
