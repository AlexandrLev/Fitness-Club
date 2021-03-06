using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kursach.Models.Admin
{
    public class EditTraining
    {
        public int TrainingId { get; set; }
        public string Name { get; set; }
        public int GymId { get; set; }
        public int CoachId { get; set; }
        public DateTime TimeOfStarting { get; set; }
        public DateTime Time { get; set; }
        public int TrainingDuration { get; set; }

        public SelectList? Gyms { get; set; }
        public SelectList? Coaches { get; set; }
    }
}
