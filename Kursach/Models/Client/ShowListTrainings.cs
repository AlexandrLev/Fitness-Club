using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kursach.Models.Client
{
    public class ShowListTrainings
    {
        public User Client { get; set; }
        public List<TrainingRegistration> TrainingRegistrations { get; set; }
    }
}
