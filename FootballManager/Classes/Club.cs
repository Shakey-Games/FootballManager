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
        public Club(string name)
        {
            Name = name;
            Players = new List<Player>();
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
            if(Manager != null)
            {
                Manager.Club = null;
            } 
            Manager = manager;
            manager.Club = this;
        }

        public void ShowSquad()
        {
            Console.WriteLine($"The squad of {Name} is:");
            foreach (Player player in Players)
            {
                Console.WriteLine(player.Name);
            }
        }

    }
}
