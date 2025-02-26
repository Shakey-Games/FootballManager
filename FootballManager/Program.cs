using FootballManager.Classes;

namespace FootballManager
{
    internal class Program
    {
        static League league = new League();
        static Coach user;
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
            while (GameWeek <= 38)
            {
                Console.Clear();
                Menu();
                Console.WriteLine($"Game Week {GameWeek}");
                league.SimulateGameWeek(GameWeek);
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                GameWeek++;
            }
            Console.ReadLine();
        }

        static void Menu()
        {
            Console.Clear();
            Console.WriteLine($"{user.Club.Name.ToUpper()}\n==========\nSeason {Season}-{Season + 1}\nWeek: {GameWeek}\n");
            Console.WriteLine("1. League Table");
            Console.WriteLine("2. Squad");
            Console.WriteLine("3. Tactics");
            Console.WriteLine("4. Transfer Market");
            Console.WriteLine("5. | CONTINUE |");
            int input = int.Parse(Console.ReadLine());
            switch (input)
            {
                case 1:
                    Console.Clear();
                    league.ShowLeagueTable();
                    Console.ReadLine();
                    Menu();
                    break;
                case 2:
                    Console.Clear();
                    user.Club.ShowSquad();
                    Console.ReadLine();
                    Menu();
                    break;
                case 3:
                    Console.Clear();
                    user.Club.ShowTactic();
                    Menu();
                    break;
                case 4:
                    Console.Clear();
                    TransferMarket();
                    Console.ReadLine();
                    Menu();
                    break;
                case 5:
                    Console.Clear();
                    break;
                default:
                    Console.WriteLine("Invalid input. Please try again.");
                    Console.ReadLine();
                    break;
            }
        }

        static void TransferMarket()
        {
            Console.WriteLine("Transfer Market\n===============");
            Console.WriteLine("1. Buy Player");
            Console.WriteLine("2. Sell Player");
            Console.WriteLine("3. Return to Menu");
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
                    return;
                default:
                    Console.WriteLine("Invalid input. Please try again.");
                    break;
            }
        }

        static void BuyPlayer()
        {
            Console.WriteLine("Available Players for Transfer:");
            List<Player> availablePlayers = league.Players.Where(p => p.Club != user.Club).ToList();
            for (int i = 0; i < availablePlayers.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {availablePlayers[i].Name} - {availablePlayers[i].Club.Name} - Rating: {availablePlayers[i].Rating}");
            }
            Console.WriteLine("Enter the number of the player you want to buy:");
            int playerChoice = int.Parse(Console.ReadLine());
            Player selectedPlayer = availablePlayers[playerChoice - 1];
            Console.WriteLine($"You have successfully bought {selectedPlayer.Name} from {selectedPlayer.Club.Name}.");
            user.Club.AddPlayer(selectedPlayer);
        }

        static void SellPlayer()
        {
            Console.WriteLine("Your Squad:");
            List<Player> squad = user.Club.Players;
            for (int i = 0; i < squad.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {squad[i].Name} - Rating: {squad[i].Rating}");
            }
            Console.WriteLine("Enter the number of the player you want to sell:");
            int playerChoice = int.Parse(Console.ReadLine());
            Player selectedPlayer = squad[playerChoice - 1];
            user.Club.RemovePlayer(selectedPlayer);
            selectedPlayer.Club = null;
            // select a random club and assign the sold player to that club
            Random random = new Random();
            Club randomClub;
            do
            {
                randomClub = league.Clubs[random.Next(league.Clubs.Count)];
            } while (randomClub == user.Club);

            randomClub.AddPlayer(selectedPlayer);
            selectedPlayer.Club = randomClub;
            Console.WriteLine($"{selectedPlayer.Name} has been transferred to {randomClub.Name}.");
        }

        static void UserDetails()
        {
            Console.WriteLine("Please enter your first name:");
            string firstName = Console.ReadLine();
            Console.WriteLine("Please enter your last name:");
            string lastName = Console.ReadLine();
            Console.WriteLine("Please enter your age:");
            int age = int.Parse(Console.ReadLine());
            Console.WriteLine("Please enter your nationality");
            string nationality = Console.ReadLine();
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
            user = new Coach(firstName, lastName, age, nationality, null);
            selectedClub.SetManager(user);
            league.Coaches.Add(user);
            Console.Clear();
            Console.WriteLine($"You have successfully created your manager profile!\n\nName: {user.Name}\nAge: {user.Age}\n" +
                $"Nationality: {user.Nationality}\nClub: {user.Club.Name}");
            Console.ReadLine();
        }
    }
}
