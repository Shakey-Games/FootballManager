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
                playersFile.ReadLine();
                string line;
                while ((line = playersFile.ReadLine()) != null)
                {
                    string[] playerData = line.Split(',');
                    Player player = new Player(playerData[0], playerData[1], int.Parse(playerData[2]), playerData[4], playerData[3]);
                    Players.Add(player);
                    foreach (Club club in Clubs)
                    {
                        if (club.Name == playerData[3])
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
            foreach (Player player in Players)
            {
                Console.WriteLine($"{player.Name} plays for {player.Club.Name}.");
            }
            foreach (Coach coach in Coaches)
            {
                Console.Write(coach.Name);
                if (coach.Club != null)
                {
                    Console.Write(" - " + coach.Club.Name);
                }
                else
                {
                    Console.Write(" - No club assigned.");
                }
            }

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
