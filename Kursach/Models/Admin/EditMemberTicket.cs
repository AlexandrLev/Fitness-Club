using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kursach.Models.Admin
{
    public class EditMemberTicket
    {
        public int MemberTicketId { get; set; }
        public string Name { get; set; }
        public int? Cost { get; set; }
        public int ValidityPeriod { get; set; }
        public int GymId { get; set; }

        public SelectList? Gyms { get; set; }
    }
}
