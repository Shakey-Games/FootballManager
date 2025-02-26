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
            CalculateRating();
        }

        private void CalculateRating()
        {
            switch (Position)
            {
                case "GK":
                    Rating = Goalkeeping;
                    break;
                case "CB":
                    Rating = (Strength + Passing + Pace) / 3;
                    break;
                case "FB":
                    Rating = (Pace + Strength + Passing) / 3;
                    break;
                case "MF":
                    Rating = (Shooting + Pace + Passing) / 3;
                    break;
                case "WI":
                    Rating = (Pace + Shooting + Passing) / 3;
                    break;
                case "ST":
                    Rating = (Shooting + Pace + Strength) / 3;
                    break;
                default:
                    Rating = (Shooting + Pace + Strength + Passing + Goalkeeping) / 5;
                    break;
            }
        }

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
