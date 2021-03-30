using System;
using System.Collections.Generic;

#nullable disable

namespace Kursach
{
    public partial class MemberTicket
    {
        public MemberTicket()
        {
            Users = new HashSet<User>();
        }

        public int MemberTicketId { get; set; }
        public string Name { get; set; }
        public int? Cost { get; set; }
        public int ValidityPeriod { get; set; }
        public int GymId { get; set; }

        public virtual Gym Gym { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
