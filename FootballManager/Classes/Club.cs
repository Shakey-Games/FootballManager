using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballManager.Classes
{


    internal class Club
    {
        public string Name { get; set; }
        public string Stadium { get; set; }
        public List<Player> Players { get; set; }
        public Coach Manager { get; set; }
        public Tactic Tactic { get; set; }
        public int Points { get; set; }
        public int Budget { get; set; }
        public Club(string name)
        {
            Name = name;
            Players = new List<Player>();
            Tactic = Tactic.Balanced;
            Points = 0;
            Budget = 100000000; // Initial budget of 1 million
        }

        public void AddPlayer(Player player)
        {
            Players.Add(player);
            player.Club = this;
        }

        public void RemovePlayer(Player player)
        {
            Players.Remove(player);
        }

        public void SetManager(Coach manager)
        {
            if (Manager != null)
            {
                Manager.Club = null;
            }
            Manager = manager;
            manager.Club = this;
        }

        public void ShowTactic()
        {
            Console.WriteLine($"{Name} are playing with a {Tactic} tactic.");
            Console.WriteLine("1. Change Tactic");
            Console.WriteLine("2. Return");
            string input = Console.ReadLine();

            if (input == "1")
            {
                Console.Clear();
                Console.WriteLine("Select a new tactic:");
                Console.WriteLine("1. Defensive");
                Console.WriteLine("2. Balanced");
                Console.WriteLine("3. Attacking");
                string tacticInput = Console.ReadLine();

                switch (tacticInput)
                {
                    case "1":
                        ChangeTactic(Tactic.Defensive);
                        break;
                    case "2":
                        ChangeTactic(Tactic.Balanced);
                        break;
                    case "3":
                        ChangeTactic(Tactic.Attacking);
                        break;
                    default:
                        Console.WriteLine("Invalid selection. Returning to previous menu.");
                        break;
                }
            }
        }

        public void ChangeTactic(Tactic newTactic)
        {
            Tactic = newTactic;
            Console.WriteLine($"{Name} has changed their tactic to {newTactic}.");
        }
        public void ShowSquad()
        {
            var positionOrder = new Dictionary<string, int>
            {
                { "GK", 1 },
                { "CB", 2 },
                { "FB", 3 },
                { "MF", 4 },
                { "WI", 5 },
                { "ST", 6 }
            };

            var sortedPlayers = Players
                .OrderBy(player => positionOrder.ContainsKey(player.Position) ? positionOrder[player.Position] : int.MaxValue)
                .ThenByDescending(player => player.Rating)
                .ToList();

            Players = sortedPlayers;

            Console.WriteLine($"{Name.ToUpper()} squad:");
            for (int i = 0; i < sortedPlayers.Count; i++)
            {
                var player = sortedPlayers[i];
                Console.WriteLine($"{i + 1}.\t[{player.Position}] {player.Name}  - (Rating: {player.Rating})");
            }
        }

        public bool BuyPlayer(Player player, Club sellingClub)
        {
            if (player == null || sellingClub == null) return false;

            if (Budget >= player.Value)
            {
                // Transfer money
                Budget -= player.Value;
                sellingClub.Budget += player.Value;

                // Move player
                sellingClub.RemovePlayer(player);
                AddPlayer(player);

                Console.WriteLine($"{Name} bought {player.Name} from {sellingClub.Name} for £{player.Value:N0}.");
                return true;
            }

            Console.WriteLine($"{Name} cannot afford {player.Name}.");
            return false;
        }

        public bool SellPlayer(Player player, Club buyingClub)
        {
            if (player == null || buyingClub == null) return false;
            return buyingClub.BuyPlayer(player, this);
        }



    }

    enum Tactic
    {
        Defensive,
        Balanced,
        Attacking
    }
}
