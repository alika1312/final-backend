using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace api.Dtos.Shift
{
    public class ShiftDto
    {
        public int shiftID { get; set; }
        public DateOnly date { get; set; }
        public int extra { get; set; }
        public string comment { get; set; } = string.Empty;
        public int ShiftTypeID { get; set; }
        public string ShiftTypeName { get; set; } = string.Empty;
    }
}