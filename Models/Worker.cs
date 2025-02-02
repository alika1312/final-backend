using System;
using System.Collections.Generic;

namespace api.Models
{
    public class Worker
    {
        public int workerID { get; set; }
        public string workerName { get; set; } = string.Empty;

        public int branchID { get; set; }
        public Branch? Branch { get; set; }

        public ICollection<WorkerShift> WorkerShifts { get; set; } = new List<WorkerShift>();
        public ICollection<WorkerProfession> WorkerProfessions { get; set; } = new List<WorkerProfession>();
    }
}
