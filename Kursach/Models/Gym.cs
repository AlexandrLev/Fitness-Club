using System;
using System.Collections.Generic;

#nullable disable

namespace Kursach
{
    public partial class Gym
    {
        public Gym()
        {
            MemberTickets = new HashSet<MemberTicket>();
            training = new HashSet<Training>();
        }

        public int GymId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int PhoneNumber { get; set; }

        public virtual ICollection<MemberTicket> MemberTickets { get; set; }
        public virtual ICollection<Training> training { get; set; }
    }
}
