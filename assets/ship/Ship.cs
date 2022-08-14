using Battleships.assets.shared;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships.assets.ship
{
    public class Ship
    {
        private int _size;
        private int _damagedGrids = 0;
        private string _className;
        private bool[] _partsHit;
        private GridCoordinates[] _gridCoordinates;


        //GET
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

        public bool[] PartsHit
        {
            get { return _partsHit.ToArray(); }
        }

        public GridCoordinates[] GridCoordinates
        {
            get
            {
                GridCoordinates[] retArray = new GridCoordinates[_gridCoordinates.Length];
                for (int i = 0; i < _gridCoordinates.Length; i++)
                    retArray[i] = new GridCoordinates(_gridCoordinates[i]);
                
                return retArray;
            }
        }


        //Private methods
        private void PartHit(int index)
        {
            _partsHit[index] = true;
            _damagedGrids++;
        }

        private void ValidateCoordinates(GridCoordinates[] gridCoordinates)
        {
            List<char> arrayOfX = new List<char>(gridCoordinates.Length);
            List<int> arrayOfY = new List<int>(gridCoordinates.Length);
            bool inRowX = true;
            bool inRowY = true;
            bool validPlacementX = true;
            bool validPlacementY = true;


            //Coords sequence
            foreach (var gridCoords in gridCoordinates)
            {
                arrayOfX.Add(gridCoords.X);
                arrayOfY.Add(gridCoords.Y);
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
                    if (arrayOfX[i] - arrayOfX[i + 1] != -1)
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

        public bool TryHitShip(GridCoordinates gridCoordinates)
        {
            for (int i = 0; i < _gridCoordinates.Length; i++)
{
                if (gridCoordinates.GetCombinedCoordinatesAsString() == _gridCoordinates[i].GetCombinedCoordinatesAsString())
                {
                    PartHit(i);
                    return true;
                }
            }

            return false;
        }

        public GridCoordinates[] GetShipPartsCoordinatesAsGridCoordinatesArray()
        {
            return GridCoordinates;
        }


        public Ship(string className, GridCoordinates[] gridCoordinates)
        {
            ValidateCoordinates(gridCoordinates);

            _gridCoordinates = gridCoordinates;
            _partsHit = new bool[gridCoordinates.Length];
            _size = gridCoordinates.Length;
            _className = className;
        }
    }
}
