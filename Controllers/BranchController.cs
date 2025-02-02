using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("api/branch")]
    [ApiController]
    public class BranchController : ControllerBase
    {
        private readonly ApplicationDBContext _context;

        public BranchController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var branches = await _context.Branch
                                         .Select(s => s.ToBranchDto())
                                         .ToListAsync();
            return Ok(branches);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var branch = await _context.Branch
                                       .Where(s => s.branchID == id)
                                       .Include(s => s.ManagerID)
                                       .FirstOrDefaultAsync();
            if (branch == null) return NotFound();
            return Ok(branch);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BranchDto branchDto)
        {
            if (branchDto.ManagerID.HasValue)
            {
                var manager = await _context.Users.FindAsync(branchDto.ManagerID.Value);
                if (manager == null)
                {
                    return BadRequest("Manager does not exist.");
                }

                var existingBranch = await _context.Branch
                                                   .FirstOrDefaultAsync(b => b.ManagerID == branchDto.ManagerID.Value);
                if (existingBranch != null)
                {
                    return BadRequest("Manager is already assigned to another branch.");
                }
            }

            var branchModel = new Branch
            {
                branchName = branchDto.branchName,
                ManagerID = branchDto.ManagerID
            };

            _context.Branch.Add(branchModel);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = branchModel.branchID }, branchModel);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var branch = await _context.Branch.FindAsync(id);
            if (branch == null) return NotFound();
            _context.Branch.Remove(branch);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] BranchDto branchDto)
        {
            var branch = await _context.Branch.FindAsync(id);
            if (branch == null) return NotFound();

            if (branchDto.ManagerID.HasValue)
            {
                var manager = await _context.Users.FindAsync(branchDto.ManagerID.Value);
                if (manager == null)
                {
                    return BadRequest("Manager does not exist.");
                }

                var existingBranch = await _context.Branch
                                                   .FirstOrDefaultAsync(b => b.ManagerID == branchDto.ManagerID.Value);
                if (existingBranch != null && existingBranch.branchID != id)
                {
                    return BadRequest("Manager is already assigned to another branch.");
                }
            }

            branch.branchName = branchDto.branchName;
            branch.ManagerID = branchDto.ManagerID;
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
