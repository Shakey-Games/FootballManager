using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballManager.Classes
{
    internal class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Nationality { get; set; }

        public Person(string firstName, string lastName, int age, string nationality)
        {
            FirstName = firstName;
            LastName = lastName;
            Age = age;
            Nationality = nationality;
            if(lastName != "")
            {
                Name = firstName + " " + lastName;
            } else { Name = firstName; }
        }

    }
}
