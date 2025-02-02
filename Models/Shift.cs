using System;
using System.Collections.Generic;

namespace api.Models
{
    public class Shift
    {
        public int shiftID { get; set; }
        public DateOnly date { get; set; }
        public int extra { get; set; }
        public string comment { get; set; } = string.Empty;

        public ICollection<WorkerShift> WorkerShifts { get; set; } = new List<WorkerShift>();
        public int ShiftTypeID { get; set; }
        public ShiftType? ShiftType { get; set; }
    }
}
