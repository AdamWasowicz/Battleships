namespace Battleships.assets.ship
{
    public static class ShipDetails
    {
        public static Tuple<string, int> CARRIER = new Tuple<string, int>("Carrier", 5);
        public static Tuple<string, int> BATTLESHIP = new Tuple<string, int>("Battleship", 4);
        public static Tuple<string, int> DESTROYER = new Tuple<string, int>("Destroyer", 3);
        public static Tuple<string, int> SUBMARINE = new Tuple<string, int>("Submarine", 3);
        public static Tuple<string, int> PATROL_BOAT = new Tuple<string, int>("Patrol boat", 2);

        public static Tuple<string, int>[] ReturnAllAsArray()
        {
            return new Tuple<string, int>[]
            {
                new Tuple<string, int>("Carrier", 5),
                new Tuple<string, int>("Battleship", 4),
                new Tuple<string, int>("Destroyer", 3),
                new Tuple<string, int>("Submarine", 3),
                new Tuple<string, int>("Patrol boat", 2)
            };  
        }
    }
}
