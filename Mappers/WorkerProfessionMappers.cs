using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos;
using api.Models;

namespace api.Mappers
{
    public static class WorkerProfessionMappers
    {
        public static WorkerProfessionDto ToWorkerProfessionDto(this WorkerProfession workerProfession)
        {
            return new WorkerProfessionDto
            {
                workerID = workerProfession.workerID,
                professionID = workerProfession.professionID
            };
        }

        public static WorkerProfession ToWorkerProfession(this CreateWorkerProfessionDto createWorkerProfessionDto)
        {
            return new WorkerProfession
            {
                workerID = createWorkerProfessionDto.workerID,
                professionID = createWorkerProfessionDto.professionID
            };
        }
    }
}