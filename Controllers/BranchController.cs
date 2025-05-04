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

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var branch = await _context.Branch
                            .Where(s => s.branchID == id)
                            .Include(s => s.Manager)  // Use the navigation property here
                            .FirstOrDefaultAsync();

            if (branch == null) return NotFound();
            return Ok(branch);
        }
        [HttpGet("company/{companyid:int}")]
public async Task<IActionResult> GetBranchesByCompanyId(int companyid)
{
    var branches = await _context.Branch
        .Include(b => b.Manager)
        .Where(b => b.Manager != null && b.Manager.companyID == companyid)
        .ToListAsync();

    if (branches == null || !branches.Any())
        return NotFound();

    return Ok(branches);
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
        
[HttpPost("bulk")]
public async Task<IActionResult> CreateMultiple([FromBody] List<BranchDto> branchDtos)
{
    if (branchDtos == null || !branchDtos.Any())
    {
        return BadRequest("No branches provided.");
    }

    var branchesToAdd = new List<Branch>();

    foreach (var branchDto in branchDtos)
    {
        if (branchDto.ManagerID.HasValue)
        {
            var manager = await _context.Users.FindAsync(branchDto.ManagerID.Value);
            if (manager == null)
            {
                return BadRequest($"Manager with ID {branchDto.ManagerID} does not exist.");
            }

            var existingBranch = await _context.Branch
                                               .FirstOrDefaultAsync(b => b.ManagerID == branchDto.ManagerID.Value);
            if (existingBranch != null)
            {
                return BadRequest($"Manager with ID {branchDto.ManagerID} is already assigned to another branch.");
            }
        }

        var branchModel = new Branch
        {
            branchName = branchDto.branchName,
            ManagerID = branchDto.ManagerID
        };

        branchesToAdd.Add(branchModel);
    }

    await _context.Branch.AddRangeAsync(branchesToAdd);
    await _context.SaveChangesAsync();

    return Ok(branchesToAdd.Select(b => b.ToBranchDto()));
}

 [HttpDelete("{id:int}")]
public async Task<IActionResult> DeleteBranch(int id)
{
    var branch = await _context.Branch.FindAsync(id);
    if (branch == null) return NotFound();

    var manager = await _context.Users.FindAsync(branch.ManagerID);
    _context.Branch.Remove(branch);

    if (manager != null)
    {
        _context.Users.Remove(manager);
    }

    await _context.SaveChangesAsync();
    return NoContent();
}


        [HttpPut("{id:int}")]
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
