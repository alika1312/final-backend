using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api.Data;
using api.Dtos;
using api.Mappers;
using api.Models;
using System.Linq;
using System.Threading.Tasks;

namespace api.Controllers
{
    [Route("api/worker-profession")]
    [ApiController]
    public class WorkerProfessionController : ControllerBase
    {
        private readonly ApplicationDBContext _context;

        public WorkerProfessionController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWorkerProfessions()
        {
            var workerProfessions = await _context.WorkerProfession
                .Include(wp => wp.Worker)
                .Include(wp => wp.Profession)
                .Select(wp => wp.ToWorkerProfessionDto())
                .ToListAsync();

            return Ok(workerProfessions);
        }

        [HttpGet("{workerID}/{professionID}")]
        public async Task<IActionResult> GetWorkerProfessionById(int workerID, int professionID)
        {
            var workerProfession = await _context.WorkerProfession
                .Include(wp => wp.Worker)
                .Include(wp => wp.Profession)
                .FirstOrDefaultAsync(wp => wp.workerID == workerID && wp.professionID == professionID);

            if (workerProfession == null)
            {
                return NotFound("Worker profession not found.");
            }

            return Ok(workerProfession.ToWorkerProfessionDto());
        }

        [HttpPost]
        public async Task<IActionResult> CreateWorkerProfession([FromBody] CreateWorkerProfessionDto createWorkerProfessionDto)
        {
            var workerProfession = new WorkerProfession
            {
                workerID = createWorkerProfessionDto.workerID,
                professionID = createWorkerProfessionDto.professionID
            };

            _context.WorkerProfession.Add(workerProfession);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetWorkerProfessionById),
                new { workerID = workerProfession.workerID, professionID = workerProfession.professionID },
                workerProfession.ToWorkerProfessionDto());
        }

        [HttpDelete("{workerID}/{professionID}")]
        public async Task<IActionResult> DeleteWorkerProfession(int workerID, int professionID)
        {
            var workerProfession = await _context.WorkerProfession
                .FirstOrDefaultAsync(wp => wp.workerID == workerID && wp.professionID == professionID);

            if (workerProfession == null)
            {
                return NotFound("Worker profession not found.");
            }

            _context.WorkerProfession.Remove(workerProfession);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
