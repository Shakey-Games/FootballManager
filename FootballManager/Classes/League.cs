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
        public List<Player> Players { get; set; }
        public List<Coach> Coaches { get; set; }
        public League()
        {
            Name = "Premier League";
            Clubs = new List<Club>();
            Players = new List<Player>();
            Coaches = new List<Coach>();
            InitialiseClubs();
            InitialisePlayers();
            InitialiseCoaches();
        }

        private void InitialiseClubs()
        {
            using (StreamReader clubsFile = new StreamReader("Clubs.csv"))
            {
                clubsFile.ReadLine();
                string line;
                while ((line = clubsFile.ReadLine()) != null)
                {
                    Clubs.Add(new Club(line));
                }
            }
        }

        private void InitialisePlayers()
        {
            using (StreamReader playersFile = new StreamReader("Players.csv"))
            {
                playersFile.ReadLine(); // Skip header line
                string line;
                while ((line = playersFile.ReadLine()) != null)
                {
                    string[] data = line.Split(',');
                    Player player = new Player(
                        data[0], // firstName
                        data[1], // lastName
                        int.Parse(data[2]), // age
                        data[3], // club
                        data[4], // country
                        int.Parse(data[5]), // shooting
                        int.Parse(data[6]), // pace
                        int.Parse(data[7]), // strength
                        int.Parse(data[8]), // passing
                        int.Parse(data[9]), // goalkeeping
                        data[10], // position
                        int.Parse(data[11]) // contract
                    );
                    Players.Add(player);
                    foreach (Club club in Clubs)
                    {
                        if (club.Name == data[3])
                        {
                            club.AddPlayer(player);
                        }
                    }
                }
            }
        }

        private void InitialiseCoaches()
        {
            using (StreamReader coachesFile = new StreamReader("Coaches.csv"))
            {
                coachesFile.ReadLine();
                string line;
                while ((line = coachesFile.ReadLine()) != null)
                {
                    string[] coachData = line.Split(',');
                    Coach coach = new Coach(coachData[0], coachData[1], int.Parse(coachData[2]), coachData[3], coachData[4]);
                    Coaches.Add(coach);
                    foreach (Club club in Clubs)
                    {
                        if (club.Name == coachData[4])
                        {
                            club.SetManager(coach);
                        }
                    }
                }
            }
        }

        public void SimulateSeason()
        {
            Console.WriteLine($"Starting {Name} season...");

            List<Match> matches = new List<Match>();

            // Create a round-robin schedule
            int numClubs = Clubs.Count;
            for (int round = 0; round < numClubs - 1; round++)
            {
                for (int i = 0; i < numClubs / 2; i++)
                {
                    int home = (round + i) % (numClubs - 1);
                    int away = (numClubs - 1 - i + round) % (numClubs - 1);

                    if (i == 0)
                    {
                        away = numClubs - 1;
                    }

                    matches.Add(new Match(Clubs[home], Clubs[away]));
                }
            }

            // Add reverse fixtures for the second half of the season
            int totalWeeks = 38;
            List<Match> reverseMatches = new List<Match>();
            foreach (var match in matches)
            {
                reverseMatches.Add(new Match(match.AwayClub, match.HomeClub));
            }
            matches.AddRange(reverseMatches);

            // Simulate each game week
            for (int week = 1; week <= totalWeeks; week++)
            {
                Console.WriteLine($"\nGame Week {week}:");

                for (int i = 0; i < numClubs / 2; i++)
                {
                    int matchIndex = (week - 1) * (numClubs / 2) + i;
                    if (matchIndex < matches.Count)
                    {
                        Match match = matches[matchIndex];
                        match.PlayMatch();
                    }
                }

                Console.WriteLine("\nPress any key to proceed to the next game week...");
                Console.ReadKey();
            }
        }
    }
}
