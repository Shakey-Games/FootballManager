using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballManager.Classes
{
    internal class Match
    {
        public Club HomeClub { get; set; }
        public Club AwayClub { get; set; }
        public int HomeGoals { get; set; }
        public int AwayGoals { get; set; }
        public Match(Club homeClub, Club awayClub)
        {
            HomeClub = homeClub;
            AwayClub = awayClub;
        }
        public void PlayMatch()
        {
            Random random = new Random();
            HomeGoals = random.Next(0, 6);
            AwayGoals = random.Next(0, 6);
            Console.WriteLine($"{HomeClub.Name} {HomeGoals} - {AwayGoals} {AwayClub.Name}");
        }
    }
}
