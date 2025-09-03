using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballManager.Classes
{
    public class Email
    {
        public string Subject { get; set; }
        public string Sender { get; set; }
        public string Body { get; set; }
        public bool IsRead { get; set; }

        public Email(string subject, string sender, string body)
        {
            Subject = subject;
            Body = body;
            Sender = sender;
            IsRead = false;
        }

        public override string ToString()
        {
            return $"{(IsRead ? "[READ] " : "[NEW] ")}{Subject} - {Sender}";
        }
    }
}
