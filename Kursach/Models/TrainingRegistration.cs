using System;
using System.Collections.Generic;

#nullable disable

namespace Kursach
{
    public partial class TrainingRegistration
    {
        public int ClientId { get; set; }
        public int TrainingId { get; set; }

        public virtual User Client { get; set; }
        public virtual Training Training { get; set; }
    }
}
