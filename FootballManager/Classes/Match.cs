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
            int homeStrength = HomeClub.Players.Sum(p => p.Rating);
            int awayStrength = AwayClub.Players.Sum(p => p.Rating);

            Random random = new Random();
            double totalStrength = homeStrength + awayStrength;

            // Calculate the probability of scoring based on strength
            double homeProbability = homeStrength / totalStrength;
            double awayProbability = awayStrength / totalStrength;

            // Simulate goals for home team
            HomeGoals = 0;
            for (int i = 0; i < 7; i++) // Simulate 7 scoring opportunities
            {
                if (random.NextDouble() < homeProbability * 0.5) // Moderate probability to score
                {
                    HomeGoals++;
                }
            }

            // Simulate goals for away team
            AwayGoals = 0;
            for (int i = 0; i < 7; i++) // Simulate 7 scoring opportunities
            {
                if (random.NextDouble() < awayProbability * 0.5) // Moderate probability to score
                {
                    AwayGoals++;
                }
            }

            Console.WriteLine($"{HomeClub.Name} {HomeGoals} - {AwayGoals} {AwayClub.Name}");
        }



    }
}
