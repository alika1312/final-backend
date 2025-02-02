using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos;
using api.Models;

namespace api.Mappers
{
    public static class WorkerMappers
    {
        public static WorkerDto ToWorkerDto(this Worker worker)
        {
            return new WorkerDto
            {
                workerID = worker.workerID,
                workerName = worker.workerName
            };
        }
    }
}