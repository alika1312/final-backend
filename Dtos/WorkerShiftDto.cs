using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos
{

    public class WorkerShiftDto
    {
        public int WorkerID { get; set; }
        public string WorkerName { get; set; } = string.Empty;
        public int ShiftID { get; set; }
        public DateOnly ShiftDate { get; set; }

        public string ShiftComment { get; set; } = string.Empty;
    }
}

