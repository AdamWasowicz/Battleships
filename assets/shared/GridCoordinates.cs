namespace Battleships.assets.shared
{
    public struct GridCoordinates
    {
        private string _x;
        private string _y;


        public string X
        {
            get { return _x; }
        }

        public string Y
        {
            get { return _y; }
        }


        //Private methods
        private static void ValidateParams(string x, string y)
        {
            //X
            if (x.Length == 0)
                throw new InvalidDataException("Invalid Coordinates, X cannot be an empty string");
            if (x.Length > 1)
                throw new InvalidDataException("Invalid Coordinates, X must be max one character long");

            //Y
            if (y.Length == 0)
                throw new InvalidDataException("Invalid Coordinates, Y cannot be an empty string");
            if (y.Length > 2)
                throw new InvalidDataException("Invalid Coordinates, X must be max two character long");
        }


        //Public methods
        public string GetCombinedCoordinatesAsString()
        {
            return _x + _y;
        }

        public string GetCombinedCoordinatesAsStringReverse()
        {
            return _y + _x;
        }


        public GridCoordinates(string x, string y)
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
            _x = "A";
            _y = "1";
        }
    }
}
