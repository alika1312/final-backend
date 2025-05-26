using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace api.Dtos
{



    public class WorkerShiftRequestDto
    {
        
        public List<int>? WorkerID  { get; set; }
        [Required]
        public int shiftID { get; set; }

    }
}

