using Battleships.assets.shared;
using Battleships.assets.ship;


namespace Battleships.assets.playable
{
    public class BattleshipsPlayer : ICanPlay
    {
        private string _playerName;



        public GridCoordinates MakeMove(List<Tuple<GridCoordinates, bool>> shotsTaken, List<Ship> playerShips)
        {
            bool validInput = false;
            string stringCoordinates = "";

            while (!validInput)
            {
                Console.Write($"Player {_playerName} move: ");
                stringCoordinates = Console.ReadLine()!;

                validInput = GridCoordinates.ValidateParams(stringCoordinates);

                if (!validInput)
                    Console.WriteLine($"Input {stringCoordinates} is invalid, try again");
            }

            GridCoordinates coordinates = new GridCoordinates(
                stringCoordinates.Substring(0, 1)[0],
                Convert.ToInt32(stringCoordinates.Substring(1))
                );

            return coordinates;
        }

        //Public methods
        public string GetPlayerName()
        {
            return _playerName.ToString();
        }

        public ICanPlay ReturnCopy()
        {
            return new BattleshipsPlayer(_playerName);
        }



        public BattleshipsPlayer(string playerName)
        {
            _playerName = playerName;
        }
    }
}
