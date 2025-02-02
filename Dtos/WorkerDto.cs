using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos
{
    public class WorkerDto
    {
        public int workerID { get; set; }
        public string workerName { get; set; } = string.Empty;
    }

    public class CreateWorkerDto
    {
        public string workerName { get; set; } = string.Empty;
    }
}