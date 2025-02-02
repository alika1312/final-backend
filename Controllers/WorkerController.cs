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

        [HttpPost]
        public async Task<IActionResult> CreateWorker([FromBody] CreateWorkerDto createWorkerDto)
        {
            if (string.IsNullOrWhiteSpace(createWorkerDto.workerName))
            {
                return BadRequest("Worker name is required.");
            }

            var worker = new Worker
            {
                workerName = createWorkerDto.workerName
            };

            _context.Worker.Add(worker);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = worker.workerID }, worker);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWorker(int id)
        {
            var worker = await _context.Worker.FindAsync(id);
            if (worker == null) return NotFound();

            _context.Worker.Remove(worker);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("{id}")]
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
    }
}
