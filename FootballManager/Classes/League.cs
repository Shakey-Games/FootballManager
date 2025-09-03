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
            public List<Match> matches { get; set; }
            public List<TransferOffer> ActiveOffers { get; set; } = new List<TransferOffer>();
            public List<String> CompletedTransfers { get; set; } = new List<String>();

        public League()
            {
                Name = "Premier League";
                Clubs = new List<Club>();
                Players = new List<Player>();
                matches = new List<Match>();
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
                        // coachData: [0]=firstName, [1]=lastName, [2]=age, [3]=nationality, [4]=clubName
                        Coach coach = new Coach(coachData[0], coachData[1], int.Parse(coachData[2]), coachData[3]);
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


            public void ShowLeagueTable()
            {
                var sortedClubs = Clubs.OrderByDescending(club => club.Points).ToList();

                Console.WriteLine("League Table:\n");
                Console.WriteLine("{0,-10} | {1,-25} | {2,6}\n", "Position", "Club", "Points");

                for (int i = 0; i < sortedClubs.Count; i++)
                {
                    Console.WriteLine("{0,-10} | {1,-25} | {2,6}", i + 1, sortedClubs[i].Name, sortedClubs[i].Points);
                }
            }

            public void CreateSchedule()
            {
                // Shuffle the clubs
                Random rng = new Random();
                Clubs = Clubs.OrderBy(x => rng.Next()).ToList();

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
                List<Match> reverseMatches = new List<Match>();
                foreach (var match in matches)
                {
                    reverseMatches.Add(new Match(match.AwayClub, match.HomeClub));
                }
                matches.AddRange(reverseMatches);
            }

            public bool IsTransferWindowOpen(int week)
            {
                if (week < 5)
                {
                    return true;
                }
                else if (week >= 20 && week <= 24)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

        public void ProcessAITransfers()
        {
            Random rand = new Random();

            foreach (var club in Clubs)
            {
                if (club == Program.user.Club) continue; // don’t let AI control user’s club

                if (rand.Next(100) < 35) // ~35% chance each week
                {
                    var targetClub = Clubs[rand.Next(Clubs.Count)];
                    if (targetClub == club) continue;

                    var affordablePlayers = targetClub.Players
                        .Where(p => club.Budget >= p.Value)
                        .ToList();

                    if (affordablePlayers.Count > 0)
                    {
                        var player = affordablePlayers[rand.Next(affordablePlayers.Count)];

                        // Calculate a realistic offer fee based on player value, rating, and some randomness
                        Random random = new Random();
                        double ratingFactor = 1 + (player.Rating - 70) * 0.01; // Slight premium for higher ratings
                        double negotiationFactor = 0.9 + rand.NextDouble() * 0.3; // Offers between 90% and 120% of value

                        int offerFee = (int)(player.Value * ratingFactor * negotiationFactor);

                        var offer = new TransferOffer(club, targetClub, player, offerFee);
                        ActiveOffers.Add(offer);

                        if (targetClub == Program.user.Club)
                        {
                            Program.user.ReceiveEmail(
                                $"Transfer Offer for {player.Name}", "Assistant Manager",
                                $"{club.Name} have offered £{offerFee:N0} for {player.Name}. " +
                                $"You can accept or reject this offer in the Transfer Market.\n\nYou only have the week this transfer was sent to deal with the offer,\notherwise it will be automatically rejected."
                            );
                        }
                        else
                        {
                            // AI automatically accepts/rejects
                            if (rand.Next(100) < 50) // ~50% chance to accept
                            {
                                CompletedTransfers.Add($"{offer.FromClub.Name} have signed {offer.Player.Name} from {offer.ToClub.Name} for a fee of £{offer.Fee:N0}");
                                CompleteTransfer(offer);
                            } else
                            {
                                //ActiveOffers.Remove(offer);
                            }
                        }

                    }
                }
            }
        }


        public void CompleteTransfer(TransferOffer offer)
        {
            // Buyer pays seller
            offer.FromClub.Budget -= offer.Fee;
            offer.ToClub.Budget += offer.Fee;

            // Move player
            offer.ToClub.RemovePlayer(offer.Player);
            offer.FromClub.AddPlayer(offer.Player);
        }


        public void SimulateGameWeek(int week)

            {
                Console.Clear();
                int numClubs = Clubs.Count;


                // Simulate each game week


                Console.WriteLine($"Game Week {week}:");

                for (int i = 0; i < numClubs / 2; i++)
                {
                    int matchIndex = (week - 1) * (numClubs / 2) + i;
                    if (matchIndex < matches.Count)
                    {
                        Match match = matches[matchIndex];
                        match.PlayMatch();
                    }
                }

            }
        }
    }
