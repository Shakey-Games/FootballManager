using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballManager.Classes
{
    internal class Coach : Person
    {
        public Club Club { get; set; }

        public Coach(string firstName, string lastName, int age, string nationality, string club)
            : base(firstName, lastName, age, nationality)
        {

        }

        public void ShowClub()
        {
            Console.WriteLine($"{Name} is the manager of {Club.Name}.");
        }

    }
}
