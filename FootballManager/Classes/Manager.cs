using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballManager.Classes
{
    internal class Manager : Person
    {
        public Club Club { get; set; }

        public Manager(string firstName, string lastName, int age, string nationality, Club club)
            : base(firstName, lastName, age, nationality)
        {
            Club = club;
        }

        public void ShowClub()
        {
            Console.WriteLine($"{Name} is the manager of {Club.Name}.");
        }

    }
}
