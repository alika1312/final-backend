using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace api.Dtos
{
    public class ShiftRequestDto
    {
        public int shiftID { get; set; }
        public DateOnly date { get; set; }

        public int extra { get; set; }
        public string comment { get; set; } = string.Empty;



        public int ShiftTypeID { get; set; }
        public int branchID { get; set; }
    }
   public class ShiftWorkerRequestDto
{
    public int shiftID { get; set; }
    public DateTime date { get; set; }
    public int extra { get; set;  }
    public int ShiftTypeID { get; set; }
    public int branchID { get; set; }
  public string comment { get; set; } = string.Empty;
    public List<int> WorkerIDs { get; set; } = new();
}

public class ShiftWorkersAssignmentDto
{
    public int ShiftID { get; set; }
    public List<int> WorkerIds { get; set; } = new();
}


}
