using FootballManager.Classes;

namespace FootballManager
{
    internal class Program
    {
        static League league = new League();

        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Football Manager!");
            Console.WriteLine("Press any key to play...");
            Console.ReadKey();
            Console.Clear();
            UserDetails();
            league.SimulateSeason();
            Console.ReadLine();
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
            Coach user = new Coach(firstName, lastName, age, nationality, null);
            selectedClub.SetManager(user);
            league.Coaches.Add(user);
            Console.Clear();
            Console.WriteLine($"You have successfully created your manager profile!\n\nName: {user.Name}\nAge: {user.Age}\n" +
                $"Nationality: {user.Nationality}\nClub: {user.Club.Name}");
            Console.ReadLine();


        }
    }
}
