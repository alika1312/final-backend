using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class WorkerProfession
    {
        public int workerID { get; set; }
        public Worker? Worker { get; set; }

        public int professionID { get; set; }
        public Profession? Profession { get; set; }
    }
}