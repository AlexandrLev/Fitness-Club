using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kursach.Models.Coach
{
    public class ShowListClass
    {
        public Training Training { get; set; }
        public List<TrainingRegistration> TrainingRegistrations { get; set; }
    }
}
