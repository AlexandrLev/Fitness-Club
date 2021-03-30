using System;
using System.Collections.Generic;

#nullable disable

namespace Kursach
{
    public partial class Training
    {
        public Training()
        {
            TrainingRegistrations = new HashSet<TrainingRegistration>();
        }

        public int TrainingId { get; set; }
        public string Name { get; set; }
        public int GymId { get; set; }
        public int CoachId { get; set; }
        public DateTime TimeOfStarting { get; set; }
        public int TrainingDuration { get; set; }

        public virtual User Coach { get; set; }
        public virtual Gym Gym { get; set; }
        public virtual ICollection<TrainingRegistration> TrainingRegistrations { get; set; }
    }
}
