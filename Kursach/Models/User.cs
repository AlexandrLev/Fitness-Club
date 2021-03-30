using System;
using System.Collections.Generic;

#nullable disable

namespace Kursach
{
    public partial class User
    {
        public User()
        {
            TrainingRegistrations = new HashSet<TrainingRegistration>();
            training = new HashSet<Training>();
        }

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

        public virtual MemberTicket MemberTicket { get; set; }
        public virtual Role Role { get; set; }
        public virtual ICollection<TrainingRegistration> TrainingRegistrations { get; set; }
        public virtual ICollection<Training> training { get; set; }
    }
}
