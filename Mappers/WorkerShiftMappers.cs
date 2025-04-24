using api.Models;
using api.Dtos;

namespace api.Mappers
{
    public static class WorkerShiftMappers
    {

        public static WorkerShiftDto ToWorkerShiftDto(this WorkerShift workerShift)
        {
            return new WorkerShiftDto
            {

                WorkerID = workerShift.Worker.workerID,
                WorkerName = workerShift.Worker.workerName,
                ShiftID = workerShift.Shift.shiftID,
                ShiftDate = workerShift.Shift.date,
                ShiftComment = workerShift.Shift.comment,
            };
        }
        public static WorkerShift ToWorkerShift(this WorkerShift workerShift)
        {
            return new WorkerShift
            {
                WorkerID = workerShift.WorkerID,
                ShiftID = workerShift.ShiftID,
            };
        }
    }
}
