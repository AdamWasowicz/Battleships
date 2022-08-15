namespace Battleships.assets.shared
{
    public struct GridCoordinates
    {
        private char _x;
        private int _y;


        public char X
        {
            get { return _x; }
        }

        public int Y
        {
            get { return _y; }
        }


        //Public methods
        public static bool ValidateParams(char x, int y)
        {
            //X
            if (x.ToString().Length == 0)
                throw new InvalidDataException("Invalid Coordinates, X cannot be an empty string");
            if (x.ToString().Length > 1)
                throw new InvalidDataException("Invalid Coordinates, X must be max one character long");

            //Y
            if (y.ToString().Length == 0)
                throw new InvalidDataException("Invalid Coordinates, Y cannot be an empty string");
            if (y.ToString().Length > 2)
                throw new InvalidDataException("Invalid Coordinates, X must be max two character long");
            

            return true;
        }

        public string GetCombinedCoordinatesAsString()
        {
            return _x.ToString() + _y.ToString();
        }


        public GridCoordinates(char x, int y)
        {
            ValidateParams(x, y);

            _x = x;
            _y = y;
        }

        public GridCoordinates(GridCoordinates gridCoordinates)
        {
            _x = gridCoordinates.X;
            _y = gridCoordinates.Y;
        }

        public GridCoordinates()
        {
            _x = 'A';
            _y = 1;
        }
    }
}
