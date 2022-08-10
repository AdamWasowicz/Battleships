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


        //Public methods
        public string GetCombinedCoordinatesAsString()
        {
            return _x + _y;
        }

        public string GetCombinedCoordinatesAsStringReverse()
        {
            return _y + _x;
        }

        public string[] GetCombinedCoordinatesAsArray()
        {
            return new string[] { _x, _y };
        }


        public GridCoordinates(string x, string y)
        {
            _x = x;
            _y = y;
        }

        public GridCoordinates(GridCoordinates gridCoordinates)
        {
            _x = gridCoordinates.X;
            _y = gridCoordinates.Y;
        }
    }
}
