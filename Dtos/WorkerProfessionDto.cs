using System;
using System.Collections.Generic;
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
        public int workerID { get; set; }
        public int professionID { get; set; }
    }

}