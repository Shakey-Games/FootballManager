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
            // Use only top 11 players (sorted by rating)
            int homeStrength = HomeClub.Players.OrderByDescending(p => p.Rating).Take(11).Sum(p => p.Rating);
            int awayStrength = AwayClub.Players.OrderByDescending(p => p.Rating).Take(11).Sum(p => p.Rating);

            Random random = new Random();

            // Home advantage bonus
            homeStrength = (int)(homeStrength * 1.1);

            double totalStrength = homeStrength + awayStrength;

            // Calculate probabilities
            double homeProbability = homeStrength / totalStrength;
            double awayProbability = awayStrength / totalStrength;

            HomeGoals = 0;
            AwayGoals = 0;

            // Factor in tactics
            int homeChances = GetChancesBasedOnTactic(HomeClub.Tactic);
            int awayChances = GetChancesBasedOnTactic(AwayClub.Tactic);

            // Simulate scoring
            for (int i = 0; i < homeChances; i++)
            {
                if (random.NextDouble() < homeProbability * 0.5)
                    HomeGoals++;
            }
            for (int i = 0; i < awayChances; i++)
            {
                if (random.NextDouble() < awayProbability * 0.5)
                    AwayGoals++;
            }
            if(HomeClub == Program.user.Club || AwayClub == Program.user.Club) { Console.ForegroundColor = ConsoleColor.Yellow; }
            Console.WriteLine($"{HomeClub.Name} {HomeGoals} - {AwayGoals} {AwayClub.Name}");
            Console.ResetColor();

            if (HomeGoals > AwayGoals) HomeClub.Points += 3;
            else if (HomeGoals < AwayGoals) AwayClub.Points += 3;
            else { HomeClub.Points++; AwayClub.Points++; }
        }

        private int GetChancesBasedOnTactic(Tactic tactic)
        {
            switch (tactic)
            {
                case Tactic.Attacking: return 9;
                case Tactic.Defensive: return 5;
                default: return 7;
            }
        }




    }
}
