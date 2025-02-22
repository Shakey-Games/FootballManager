using FootballManager.Classes;

namespace FootballManager
{
    internal class Program
    {
        static void Main(string[] args)
        {
            League league = new League();
            league.SimulateSeason();
            Console.ReadLine();
        }
    }
}
