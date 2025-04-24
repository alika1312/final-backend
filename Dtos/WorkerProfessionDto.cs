using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos
{

    public class WorkerProfessionDto
    {
        public int workerID { get; set; }
        public int professionID { get; set; }
    }
    public class CreateWorkerProfessionDto
    {
        [Required]
        public int workerID { get; set; }
        [Required]
        public int professionID { get; set; }
    }

}