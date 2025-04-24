using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api.Models;
using api.Dtos;
using api.Data;
using api.Mappers;
using System.Linq;
using System.Threading.Tasks;

namespace api.Controllers
{
    [Route("api/workershift")]
    [ApiController]
    public class WorkerShiftController : ControllerBase
    {
        private readonly ApplicationDBContext _context;

        public WorkerShiftController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var workerShifts = await _context.WorkerShift
                .Include(ws => ws.Worker)
                .Include(ws => ws.Shift)
                .ToListAsync();

            if (!workerShifts.Any())
            {
                return NotFound("No worker-shift associations found.");
            }

            var workerShiftDtos = workerShifts.Select(ws => ws.ToWorkerShiftDto()).ToList();

            return Ok(workerShiftDtos);
        }

        [HttpGet("{workerID:int}/{shiftID:int}")]
        public async Task<IActionResult> GetById(int workerID, int shiftID)
        {
            var workerShift = await _context.WorkerShift
                .Include(ws => ws.Worker)
                .Include(ws => ws.Shift)
                .FirstOrDefaultAsync(ws => ws.WorkerID == workerID && ws.ShiftID == shiftID);

            if (workerShift == null)
            {
                return NotFound($"No association found for WorkerID {workerID} and ShiftID {shiftID}.");
            }

            return Ok(workerShift.ToWorkerShiftDto());
        }

        [HttpPost]
        public async Task<IActionResult> AddWorkerToShift([FromBody] WorkerShiftRequestDto workerShiftRequest)
        {
            var worker = await _context.Worker.FirstOrDefaultAsync(w => w.workerID == workerShiftRequest.workerID);
            if (worker == null)
            {
                return BadRequest("Invalid WorkerID. Worker does not exist.");
            }

            var shift = await _context.Shift.FirstOrDefaultAsync(s => s.shiftID == workerShiftRequest.shiftID);
            if (shift == null)
            {
                return BadRequest("Invalid ShiftID. Shift does not exist.");
            }

            var workerShift = new WorkerShift
            {
                WorkerID = worker.workerID,
                ShiftID = shift.shiftID,
                Shift = shift,
                Worker = worker
            };

            _context.WorkerShift.Add(workerShift);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetById),
                new { workerID = workerShift.WorkerID, shiftID = workerShift.ShiftID },
                workerShift.ToWorkerShiftDto()
            );
        }

        [HttpDelete("{workerID:int}/{shiftID:int}")]
        public async Task<IActionResult> DeleteWorkerShift(int workerID, int shiftID)
        {
            var workerShift = await _context.WorkerShift
                .FirstOrDefaultAsync(ws => ws.WorkerID == workerID && ws.ShiftID == shiftID);

            if (workerShift == null)
            {
                return NotFound("Worker-Shift association not found.");
            }

            _context.WorkerShift.Remove(workerShift);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
