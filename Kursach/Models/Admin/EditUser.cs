using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kursach.Models.Admin
{
    public class EditUser
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Patronymic { get; set; }
        public int PassportData { get; set; }
        public int? MemberTicketId { get; set; }
        public DateTime? ConclusionDate { get; set; }
        public SelectList? MemberTickets { get; set; }

    }
}
