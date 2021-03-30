using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kursach.Models.Admin
{
    public class EditDBModel
    {
        public List<User> Users { get; set; }
        public List<Gym> Gyms { get; set; }
        public List<MemberTicket> MemberTickets { get; set; }
        public List<Training> Trainings { get; set; }
        public List<TrainingRegistration> TrainingRegistrations { get; set; }

    }
}
