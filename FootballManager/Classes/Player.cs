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
        public int Rating { get; set; }
        public int Pace { get; set; }
        public int Shooting { get; set; }
        public int Strength { get; set; }
        public int Passing { get; set; }
        public int Goalkeeping { get; set; }
        public string Position { get; set; }

        public Player(string firstName, string lastName, int age, string club, string country, int shooting, int pace, int
            strength, int passing, int goalkeeping, string position, int contract)
            : base(firstName, lastName, age, country)
        {
            Shooting = shooting;
            Pace = pace;
            Strength = strength;
            Passing = passing;
            Goalkeeping = goalkeeping;
            Position = position;
            Rating = (Shooting + Pace + Strength + Passing + Goalkeeping) / 5;
            //Position = Enum.Parse<Position>(position);

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

    public enum Position
    {
        GK,
        CB,
        FB,
        MF,
        WI,
        ST
    }
}
