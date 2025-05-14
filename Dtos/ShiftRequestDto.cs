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
        

        [Required]
        public int ShiftTypeID { get; set; }
        public int branchID { get; set; }
    }
}
