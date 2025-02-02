using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class WorkerShift
    {


        public int WorkerID { get; set; }
        public Worker Worker { get; set; } = new Worker();
        public int ShiftID { get; set; }
        public Shift Shift { get; set; } = new Shift();


    }
}