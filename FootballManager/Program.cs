using FootballManager.Classes;
using System;
using System.Runtime.Intrinsics.X86;

namespace FootballManager
{
    internal class Program
    {
        static League league = new League();
        public static Coach user;
        static int GameWeek = 1;
        static int Season = 2024;

        static void Main(string[] args)
        {
            Console.WriteLine("UNFINISHED");
            Console.WriteLine("Welcome to Football Manager!");
            Console.WriteLine("Press any key to play...");
            Console.ReadKey();
            Console.Clear();
            UserDetails();
            league.CreateSchedule();
            PlaySeason();
            //while (GameWeek <= 38)
            //{
            //    //Console.Clear();
            //    //Menu();
            //    //Console.WriteLine($"Game Week {GameWeek}");
            //    //league.SimulateGameWeek(GameWeek);
            //    //Console.WriteLine("\nTRANSFER NEWS:\n");
            //    //ProcessAITransfers();
            //    //Console.WriteLine("Press any key to continue...");
            //    //Console.ReadKey();
            //    //GameWeek++;
            //}
            Console.ReadLine();
        }

        static void PlaySeason()
        {
            for (int week = 0; week <= 38; week++)
            {
                if (league.IsTransferWindowOpen(GameWeek)) { league.ProcessAITransfers(); }
                WeeklyEmailCheck();
                Menu();
                league.SimulateGameWeek(GameWeek);
                if (league.IsTransferWindowOpen(GameWeek))
                {
                    Console.WriteLine("\nTRANSFER NEWS:\n");
                    foreach (var offer in league.CompletedTransfers)
                    {
                        Console.WriteLine(offer);
                    }
                }
                league.CompletedTransfers.Clear();
                Console.ReadLine();
                GameWeek++;
                //if !(GameWeek == 25 || GameWeek == 5) { league.ActiveOffers.Clear(); }
            }
            Console.WriteLine("\nSeason has ended!");
            league.ShowLeagueTable();
        }

        static void Menu()
        {
            bool stayInMenu = true;

            while (stayInMenu)
            {
                Console.Clear();
                Console.WriteLine($"{user.Club.Name.ToUpper()}\n==========\n--- Week {GameWeek} ---\n");

                Console.WriteLine("1. Inbox");
                Console.WriteLine("2. View Squad");
                Console.WriteLine("3. Transfer Market");
                Console.WriteLine("4. View League Table");
                Console.ForegroundColor = ConsoleColor.Yellow; Console.WriteLine("0. CONTINUE"); Console.ResetColor();

                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        ShowInbox();
                        break;
                    case "2":
                        Console.Clear();
                        user.Club.ShowSquad();
                        Console.ReadLine();
                        break;
                    case "3":
                        TransferMarket();
                        break;
                    case "4":
                        Console.Clear();
                        league.ShowLeagueTable();
                        Console.ReadLine();
                        break;
                    case "0":
                        stayInMenu = false;
                        break;
                    default:
                        Console.WriteLine("Invalid choice, try again.");
                        break;
                }
            }
        }

        static void ShowInbox()
        {
            Console.Clear();

            if (user.Inbox.Count == 0)
            {
                Console.WriteLine("Inbox is empty.");
                Console.ReadLine();
                return;
            }

            Console.WriteLine("--- Inbox ---");
            for (int i = 0; i < user.Inbox.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {user.Inbox[i]}");
            }

            Console.WriteLine("Enter the number of an email to read, or 0 to go back:");

            string strChoice = Console.ReadLine();
            int choice = 0;
            if (strChoice != "") { choice = int.Parse(strChoice); } else { return; } 

            if (choice > 0 && choice <= user.Inbox.Count)
            {
                Console.Clear();
                var email = user.Inbox[choice - 1];
                email.IsRead = true;

                Console.ForegroundColor = ConsoleColor.Yellow;  Console.WriteLine($"{email.Subject.ToUpper()}\n==========\nFrom: {email.Sender}\n"); Console.ResetColor();
                Console.WriteLine($"{email.Body}\n");

                Console.ForegroundColor = ConsoleColor.Yellow; Console.WriteLine("Press Enter to return to Inbox..."); Console.ResetColor();
                Console.ReadLine();
                ShowInbox();
            }
        }

        static void WeeklyEmailCheck()
        {
            user.Inbox.RemoveAll(email => email.IsRead);

            switch (GameWeek)
            {
                case 1:
                    user.ReceiveEmail("Welcome", "Assistant Manager", $"Welcome to your first season in charge of {user.Club.Name}! I'm your" +
                        $" assistant manager, and I'm here to support and advise you throughout your tenure here at this great football club." +
                        $"\n\n" +
                        $"Now, the previous manager got sacked right at the beginning of the league, so you have no time to settle in with a" +
                        $"preseason. You've got to set off running from the get-go." +
                        $"\n\n" +
                        $"The transfer window closes at the end of the fourth gameweek. So, make sure you conclude any transfer business by " +
                        $"then, since you've fortunately arrived just in time to make a few signings or sales." +
                        $"\n\n" +
                        $"I wish you good luck and a fruitful stay here.");
                    break;
                case 5:
                    user.ReceiveEmail("Summer Transfer Window Closed", "Assistant Manager", $"The summer transfer window has now closed. You can no longer make transfers until the winter window opens in week 20." +
                        $"\n\n" +
                        $"I hope you managed to strengthen the squad to your satisfaction. Now, let's focus on getting some good results on the pitch!" +
                        $"\n\n" +
                        $"Good luck!");
                    break;
                case 20:
                    user.ReceiveEmail("Winter Transfer Window", "Assistant Manager", $"The winter transfer window is now open. You can make transfers until the end of week 24." +
                        $"\n\n" +
                        $"It's a good opportunity to strengthen the squad for the second half of the season, so consider your options carefully." +
                        $"\n\n" +
                        $"Good luck!");
                    break;
                    break;
                case 25:
                    user.ReceiveEmail("Winter Transfer Window Closed", "Assistant Manager", $"The winter transfer window has now closed. You can no longer make transfers until the next summer window opens." +
                        $"\n\n" +
                        $"I hope you managed to make some good signings to help the team in the latter stages of the season." +
                        $"\n\n" +
                        $"Let's focus on finishing the season strongly!");
                    break;
            }
        }

        static void TransferMarket()
        {
            Console.Clear();
            Console.WriteLine($"Transfer Market\n===============\nBudget: £{user.Club.Budget.ToString("N0")}\n");
            if (!league.IsTransferWindowOpen(GameWeek))
            {
                Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine("The transfer window is currently closed.\n"); Console.ResetColor();
                Console.WriteLine("You can only make transfers during:\n- The first 4 weeks of the season (Summer Window).\n- Between weeks 20-24 (Winter Window).");
                Console.ReadLine();
                return;
            }
            Console.WriteLine("1. Buy Player");
            Console.WriteLine("2. Sell Player");
            Console.WriteLine("3. Review Incoming Offers");
            Console.WriteLine("4. Return to Menu");
            int input = int.Parse(Console.ReadLine());
            switch (input)
            {
                case 1:
                    BuyPlayer();
                    break;
                case 2:
                    SellPlayer();
                    break;
                case 3:
                    ReviewOffers();
                    break;
                case 4:
                    return;
                default:
                    Console.WriteLine("Invalid input. Please try again.");
                    break;
            }
        }

        static void BuyPlayer()
        {
            Console.Clear();
            Console.WriteLine("Transfer Search Options:");
            Console.WriteLine("1. Search by Name");
            Console.WriteLine("2. Search by Position");
            Console.WriteLine("3. Show All");
            Console.WriteLine("0. Return to Transfer Market");
            string searchOption = Console.ReadLine();

            if (searchOption == "0") return;

            List<Player> availablePlayers = league.Players.Where(p => p.Club != user.Club).ToList();
            List<Player> filteredPlayers = new List<Player>();
            Console.Clear();
            switch (searchOption)
            {
                case "1":
                    Console.WriteLine("Enter player name (partial or full):");
                    string nameQuery = Console.ReadLine().ToLower();
                    filteredPlayers = availablePlayers
                        .Where(p => p.Name.ToLower().Contains(nameQuery))
                        .OrderByDescending(p => p.Rating)
                        .ToList();
                    break;
                case "2":
                    Console.WriteLine("Enter position (GK, CB, FB, MF, WI, ST):");
                    string positionQuery = Console.ReadLine().ToUpper();
                    filteredPlayers = availablePlayers
                        .Where(p => p.Position.ToUpper() == positionQuery)
                        .OrderByDescending(p => p.Rating)
                        .ToList();
                    break;
                case "3":
                    filteredPlayers = availablePlayers
                        .OrderByDescending(p => p.Rating)
                        .ToList();
                    break;
                default:
                    filteredPlayers = availablePlayers
                        .OrderByDescending(p => p.Rating)
                        .ToList();
                    break;
            }

            if (filteredPlayers.Count == 0)
            {
                Console.WriteLine("No players found matching your criteria.");
                return;
            }

            Console.WriteLine("Available Players for Transfer:");
            for (int i = 0; i < filteredPlayers.Count; i++)
            {
                var p = filteredPlayers[i];
                Console.WriteLine($"{i + 1}. [{p.Position}] {p.Name} - {p.Club.Name} - Rating: {p.Rating} (£{p.Value:N0})");
            }
            Console.WriteLine("Enter the number of the player you want to submit an offer for, or 0 to cancel:");
            int playerChoice = int.Parse(Console.ReadLine());
            if (playerChoice == 0) return;

            if (playerChoice < 1 || playerChoice > filteredPlayers.Count)
            {
                Console.WriteLine("Invalid selection.");
                return;
            }

            Player selectedPlayer = filteredPlayers[playerChoice - 1];

            Console.Clear();
            // Create the transfer offer
            Console.WriteLine($"You are about to make an offer for {selectedPlayer.Name} from {selectedPlayer.Club.Name}.");
            Console.WriteLine($"Player Value: £{selectedPlayer.Value:N0}");
            Console.WriteLine($"Your Budget: £{user.Club.Budget:N0}");
            Console.WriteLine("\nEnter your offer amount (or 0 to cancel):");
            int input = 1;
            bool aiAccepts = false;
            TransferOffer offer = null;
            while (input != 0 && input <= user.Club.Budget)
            {
                Random rand = new Random();
                input = int.Parse(Console.ReadLine());

                var proposedOffer = new TransferOffer(user.Club, selectedPlayer.Club, selectedPlayer, input);
                league.ActiveOffers.Add(proposedOffer);
                offer = proposedOffer;
                if (input > selectedPlayer.Value) { aiAccepts = true; }
                else if (input == selectedPlayer.Value) { aiAccepts = rand.Next(100) < 80; } // 80% chance AI accepts
                else if (input >= selectedPlayer.Value * 0.8) { aiAccepts = rand.Next(100) < 50; } // 50% chance AI accepts
                else if (input >= selectedPlayer.Value * 0.5) { aiAccepts = rand.Next(100) < 15; } // 15% chance AI accepts
                else { aiAccepts = false; }
                break;
            }

            if (aiAccepts)
            {
                Console.WriteLine($"{selectedPlayer.Club.Name} accepted your offer for {selectedPlayer.Name}!");
                league.CompletedTransfers.Add($"{offer.FromClub.Name} have signed {offer.Player.Name} from {offer.ToClub.Name} for a fee of £{offer.Fee:N0}");
                league.CompleteTransfer(offer);
                league.ActiveOffers.Remove(offer);
            }
            else
            {
                Console.WriteLine($"{selectedPlayer.Club.Name} rejected your offer for {selectedPlayer.Name}.");
                league.ActiveOffers.Remove(offer);
            }

            Console.ReadLine();
        }


        static void SellPlayer()
        {
            user.Club.ShowSquad();
            Console.WriteLine("Enter the number of the player you want to sell:");
            int playerChoice = int.Parse(Console.ReadLine());

            if (playerChoice < 1 || playerChoice > user.Club.Players.Count)
            {
                Console.WriteLine("Invalid selection.");
                return;
            }

            Player selectedPlayer = user.Club.Players[playerChoice - 1];

            // Pick random buying club
            Random random = new Random();
            Club randomClub;
            do
            {
                randomClub = league.Clubs[random.Next(league.Clubs.Count)];
            } while (randomClub == user.Club || randomClub.Budget < selectedPlayer.Value);

            // Calculate a realistic offer fee based on player value, rating, and some randomness
            Random rand = new Random();
            double ratingFactor = 1 + (selectedPlayer.Rating - 70) * 0.01; // Slight premium for higher ratings
            double negotiationFactor = 0.9 + rand.NextDouble() * 0.3; // Offers between 90% and 120% of value

            int offeredFee = (int)(selectedPlayer.Value * ratingFactor * negotiationFactor);

            // Fix: buying club is FromClub, your club is ToClub
            TransferOffer offer = new TransferOffer(randomClub, user.Club, selectedPlayer, offeredFee);
            league.ActiveOffers.Add(offer);

            Console.Clear();
            Console.WriteLine($"{randomClub.Name} have approached us to sign {selectedPlayer.Name}.\nThey propose a fee of £{offer.Fee:N0}");
            Console.WriteLine($"Player Value: £{selectedPlayer.Value:N0}");
            Console.WriteLine("Do you want to accept this offer? (y/n)");
            string response = Console.ReadLine().ToLower();
            if (response == "y")
            {
                Console.WriteLine($"{selectedPlayer.Name} has been sold to {randomClub.Name} for £{offer.Fee:N0}.");
                league.CompletedTransfers.Add($"{offer.FromClub.Name} have signed {offer.Player.Name} from {offer.ToClub.Name} for a fee of £{offer.Fee:N0}");
                league.CompleteTransfer(offer);
                league.ActiveOffers.Remove(offer); // Remove after completion
            }
            else
            {
                Console.WriteLine("You rejected the offer.");
                league.ActiveOffers.Remove(offer); // Remove after rejection
            }
            Console.ReadLine();
        }


        static void ReviewOffers()
        {
            var myOffers = league.ActiveOffers.Where(o => o.ToClub == user.Club).ToList();

            if (myOffers.Count == 0)
            {
                Console.WriteLine("You have no active offers.");
                Console.ReadLine();
                return;
            }

            Console.WriteLine("--- Incoming Transfer Offers ---");
            for (int i = 0; i < myOffers.Count; i++)
            {
                var o = myOffers[i];
                Console.WriteLine($"{i + 1}. {o.FromClub.Name} want {o.Player.Name} for £{o.Fee:N0} ({o.Player.Club.Name})");
            }

            Console.WriteLine("Enter the number of an offer to respond, or 0 to cancel:");
            int choice = int.Parse(Console.ReadLine());
            if (choice == 0) return;

            if (choice < 1 || choice > myOffers.Count)
            {
                Console.WriteLine("Invalid selection.");
                return;
            }

            var selectedOffer = myOffers[choice - 1];

            Console.WriteLine($"Accept {selectedOffer.FromClub.Name}'s offer for {selectedOffer.Player.Name}? (y/n)");
            string response = Console.ReadLine().ToLower();

            if (response == "y")
            {
                league.CompletedTransfers.Add($"{selectedOffer.FromClub.Name} have signed {selectedOffer.Player.Name} from {selectedOffer.ToClub.Name} for a fee of £{selectedOffer.Fee:N0}");
                league.CompleteTransfer(selectedOffer);
                league.ActiveOffers.Remove(selectedOffer);
                Console.WriteLine("Transfer completed.");
            }
            else
            {
                league.ActiveOffers.Remove(selectedOffer);
                Console.WriteLine("Offer rejected.");
            }

            Console.ReadLine();
        }


        //static void ProcessAITransfers()
        //{
        //    Random rand = new Random();
        //    foreach (var club in league.Clubs.Where(c => c != user.Club))
        //    {
        //        if (rand.NextDouble() < 0.2) // 20% chance each week
        //        {
        //            Player target = league.Players
        //                .Where(p => p.Club != club)
        //                .OrderBy(x => rand.Next())
        //                .FirstOrDefault();

        //            if (target != null)
        //            {
        //                club.BuyPlayer(target, target.Club);
        //            }
        //        }
        //    }
        //}


        static void UserDetails()
        {
            // test
            //Console.WriteLine("Please enter your first name:");
            //string firstName = Console.ReadLine();
            //Console.WriteLine("Please enter your last name:");
            //string lastName = Console.ReadLine();
            //Console.WriteLine("Please enter your age:");
            //int age = int.Parse(Console.ReadLine());
            //Console.WriteLine("Please enter your nationality");
            //string nationality = Console.ReadLine();
            Console.Clear();
            int i = 1;
            foreach (Club club in league.Clubs)
            {
                Console.WriteLine($"{i}. {club.Name}");
                i++;
            }
            Console.WriteLine("\nPlease enter the club you want to manage:");
            int clubChoice = int.Parse(Console.ReadLine());
            Club selectedClub = league.Clubs[clubChoice - 1];
            //user = new Coach(firstName, lastName, age, nationality);
            user = new Coach("John", "Smith", 40, "England");
            selectedClub.SetManager(user);
            league.Coaches.Add(user);
            Console.Clear();
            Console.WriteLine($"You have successfully created your manager profile!\n\nName: {user.Name}\nAge: {user.Age}\n" +
                $"Nationality: {user.Nationality}\nClub: {user.Club.Name}");
            Console.ReadLine();
        }
    }
}
