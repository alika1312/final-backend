using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class ShiftType
    {
        public int shiftTypeID { get; set; }
        public string shiftTypeName { get; set; } = string.Empty;

        public int companyID { get; set; }
        public Company Company { get; set; }
        public ICollection<Shift> Shifts { get; set; } = new List<Shift>();
    }
}