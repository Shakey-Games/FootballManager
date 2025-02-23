using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballManager.Classes
{
    internal class Player : Person
    {
        public Club Club { get; set; }


        // Attributes
        public int Pace { get; set; }
        public int Shooting { get; set; }
        public int Strength { get; set; }
        public int Passing { get; set; }


        public Player(string firstName, string lastName, int age, string nationality, string club)
            : base(firstName, lastName, age, nationality)
        {

        }

        //public Player(string firstName, string lastName, int age, string nationality, int pace, int shooting,
        //    int strength, int passing)
        //    : base(firstName, lastName, age, nationality)
        //{
        //    Pace = pace;
        //    Shooting = shooting;
        //    Strength = strength;
        //    Passing = passing;
        //}


        public void ShowInfo()
        {
            Console.WriteLine($"{Name} is {Age} years old and plays for {Club.Name}.");
        }
    }
}
