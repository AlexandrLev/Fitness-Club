using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kursach.Models.Admin
{
    public class EditTrainingRegistration
    {
        public int ClientId { get; set; }
        public int TrainingId { get; set; }

        public SelectList? Clients { get; set; }
        public SelectList? Trainings { get; set; }
    }
}
