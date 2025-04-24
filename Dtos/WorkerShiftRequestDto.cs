using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace api.Dtos
{



    public class WorkerShiftRequestDto
    {
        [Required]
        public int workerID { get; set; }
        [Required]
        public int shiftID { get; set; }

    }
}

