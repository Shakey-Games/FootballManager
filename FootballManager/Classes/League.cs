using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballManager.Classes
{
    internal class League
    {
        public string Name { get; set; }
        public List<Club> Clubs { get; set; }
        public League()
        {
            Name = "Premier League";
            Clubs = new List<Club>();
            InitialiseClubs();
        }

        private void InitialiseClubs()
        {
            string[] teamNames = {
            "Arsenal", "Aston Villa", "AFC Bournemouth", "Brentford", "Brighton & Hove Albion",
            "Chelsea", "Crystal Palace", "Everton", "Fulham", "Ipswich Town", "Leicester City",
            "Liverpool", "Manchester City", "Manchester United", "Newcastle United", 
            "Nottingham Forest", "Southampton", "Tottenham Hotspur", "West Ham United", "Wolverhampton Wanderers"
            };

            foreach (string teamName in teamNames)
            {
                Clubs.Add(new Club(teamName));
            }

        }
        
        public void SimulateSeason()
        {
            Console.WriteLine($"Starting {Name} season...");
            foreach (var homeTeam in Clubs)
            {
                foreach (var awayTeam in Clubs)
                {
                    if (homeTeam != awayTeam)
                    {
                        Match match = new Match(homeTeam, awayTeam);
                        match.PlayMatch();
                    }
                }
            }
        }

    }
}
