using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballManager.Classes
{
    internal class TransferOffer
    {
        public Club FromClub { get; set; }
        public Club ToClub { get; set; }
        public Player Player { get; set; }
        public int Fee { get; set; }
        public bool IsUserInvolved => FromClub == Program.user.Club || ToClub == Program.user.Club;

        public TransferOffer(Club fromClub, Club toClub, Player player, int fee)
        {
            FromClub = fromClub;
            ToClub = toClub;
            Player = player;
            Fee = fee;
        }

        public override string ToString()
        {
            return $"{FromClub.Name} -> {ToClub.Name}: {Player.Name} for £{Fee:N0}";
        }

    }
}
