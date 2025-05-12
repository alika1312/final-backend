using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("api/worker")]
    [ApiController]
    public class WorkerController : ControllerBase
    {
        private readonly ApplicationDBContext _context;

        public WorkerController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var workers = await _context.Worker.ToListAsync();
            return Ok(workers);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var worker = await _context.Worker.FindAsync(id);
            if (worker == null) return NotFound();
            return Ok(worker);
        }
[HttpPost("workers-by-branches")]
public async Task<IActionResult> GetWorkersByBranchIds([FromBody] List<int> branchIds)
{
    if (branchIds == null || !branchIds.Any())
        return BadRequest("No branch IDs provided.");

    var workers = await _context.Worker.ToListAsync(); 
    var filteredWorkers = workers.Where(w => branchIds.Contains(w.branchID)).ToList(); 

    return Ok(filteredWorkers);
}


      [HttpPost]
public async Task<IActionResult> CreateWorker([FromBody] CreateWorkerDto createWorkerDto)
{
    if (string.IsNullOrWhiteSpace(createWorkerDto.workerName))
    {
        return BadRequest("Worker name is required.");
    }

  
    var branch = await _context.Branch.FindAsync(createWorkerDto.branchID);
    if (branch == null)
    {
        return BadRequest("Invalid branchID. The branch does not exist.");
    }

    var worker = new Worker
    {
        workerName = createWorkerDto.workerName,
        branchID = createWorkerDto.branchID 
    };

    _context.Worker.Add(worker);
    await _context.SaveChangesAsync();

    return CreatedAtAction(nameof(GetById), new { id = worker.workerID }, worker);
}


        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteWorker(int id)
        {
            var worker = await _context.Worker.FindAsync(id);
            if (worker == null) return NotFound();

            _context.Worker.Remove(worker);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateWorker(int id, [FromBody] CreateWorkerDto createWorkerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var worker = await _context.Worker.FindAsync(id);
            if (worker == null)
            {
                return NotFound($"Worker with ID {id} not found.");
            }

            worker.workerName = createWorkerDto.workerName;

            await _context.SaveChangesAsync();

            var updatedWorker = new WorkerDto
            {
                workerID = worker.workerID,
                workerName = worker.workerName
            };

            return Ok(updatedWorker);
        }
 [HttpPost("bulk-create-workers-with-professions")]
public async Task<IActionResult> CreateWorkersWithProfessions([FromBody] List<CreateWorkerWithProfessionsDto> dtos)
{
    if (dtos == null || !dtos.Any())
    {
        return BadRequest("No worker data provided.");
    }

    
    var branchIds = dtos.Select(d => d.branchID).Distinct().ToList();


    var validBranches = _context.Branch
        .AsEnumerable()
        .Where(b => branchIds.Contains(b.branchID))
        .Select(b => b.branchID)
        .ToList();

    var invalidBranches = branchIds.Except(validBranches).ToList();
    if (invalidBranches.Any())
    {
        return BadRequest($"Invalid branch IDs: {string.Join(", ", invalidBranches)}");
    }

    var createdWorkers = new List<Worker>();
    var createdWorkerProfessions = new List<WorkerProfession>();

    foreach (var dto in dtos)
    {
        if (string.IsNullOrWhiteSpace(dto.workerName))
        {
            return BadRequest("Worker name is required for all entries.");
        }

        var worker = new Worker
        {
            workerName = dto.workerName,
            branchID = dto.branchID
        };

        _context.Worker.Add(worker);
        createdWorkers.Add(worker);
    }

    await _context.SaveChangesAsync();

    for (int i = 0; i < dtos.Count; i++)
    {
        var worker = createdWorkers[i];
        var professions = dtos[i].professionIDs;

        if (professions != null)
        {
            foreach (var professionID in professions)
            {
                createdWorkerProfessions.Add(new WorkerProfession
                {
                    workerID = worker.workerID,
                    professionID = professionID
                });
            }
        }
    }

    _context.WorkerProfession.AddRange(createdWorkerProfessions);
    await _context.SaveChangesAsync();

    return Ok(new
    {
        createdWorkers,
        createdWorkerProfessions
    });
}
[HttpPatch("{id:int}/name")]
public async Task<IActionResult> UpdateWorkerName(int id, [FromBody] string newWorkerName)
{
    if (string.IsNullOrWhiteSpace(newWorkerName))
    {
        return BadRequest("Worker name cannot be empty.");
    }

    var worker = await _context.Worker.FindAsync(id);
    if (worker == null)
    {
        return NotFound($"Worker with ID {id} not found.");
    }

    worker.workerName = newWorkerName;
    await _context.SaveChangesAsync();

    return Ok(new
    {
        worker.workerID,
        worker.workerName
    });
}

    }
}
