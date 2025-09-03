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
        public List<Email> Inbox { get; set; } = new List<Email>();


        public Coach(string firstName, string lastName, int age, string nationality)
            : base(firstName, lastName, age, nationality)
        {
        }

        public void ShowClub()
        {
            if (Club != null)
            {
                Console.WriteLine($"{Name} is the manager of {Club.Name}.");
            }
            else
            {
                Console.WriteLine($"{Name} is currently unemployed.");
            }
        }

        public void ReceiveEmail(string subject, string sender, string body)
        {
            Inbox.Add(new Email(subject, sender, body));
        }


    }

}
