using Battleships.assets.shared;


namespace Battleships.assets.ship
{
    public class Ship
    {
        private int _size;                                  //Size of ship
        private int _damagedGrids = 0;                      //How many of ship's parts got damaged
        private string _className;                          //Name of ship class
        private bool[] _partsHit;                           //Indicates what coordinates of _gridCoordinates got hit
        private GridCoordinates[] _gridCoordinates;         //Indicates where ship is located



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
        //When ship part got hit
        private void PartHit(int index)
        {
            _partsHit[index] = true;
            _damagedGrids++;
        }

        //Validates if passed coordinates are valid
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
            if (_damagedGrids >= _size)
                return true;

            return false;
        }

        //Return true if coordinates overlap with ship part; that means ship got hit
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



        public Ship(Ship ship)
        {
            _size = ship._size;
            _damagedGrids = ship._size;
            _className = ship._className.Substring(0);
            _partsHit = ship._partsHit.ToArray();

            _gridCoordinates = new GridCoordinates[_size];
            for (int i = 0; i < _size; i++)
                _gridCoordinates[i] = new GridCoordinates(ship._gridCoordinates[i]);
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
