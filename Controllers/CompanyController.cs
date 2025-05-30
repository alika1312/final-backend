using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos;
using api.Mappers;
using api.Mappers.api.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("api/company")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ApplicationDBContext _context;

        public CompanyController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var companies = await _context.Company.ToListAsync();
            return Ok(companies);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var company = await _context.Company
                                        .Include(c => c.users)
                                        .FirstOrDefaultAsync(c => c.companyID == id);

            if (company == null)
            {
                return NotFound("Company not found.");
            }

            var companyDto = company.ToCompanyDto();
            return Ok(companyDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCompany([FromBody] CompanyRequestDto createCompanyDto)
        {
            if (string.IsNullOrWhiteSpace(createCompanyDto.companyName))
            {
                return BadRequest("Company name is required.");
            }

            var company = createCompanyDto.ToCompanyFromDto();

            _context.Company.Add(company);
            await _context.SaveChangesAsync();

            var companyDto = company.ToCompanyDto();
            return CreatedAtAction(nameof(GetById), new { id = company.companyID }, companyDto);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteCompany(int id)
        {
            var company = await _context.Company.FirstOrDefaultAsync(c => c.companyID == id);
            if (company == null)
            {
                return NotFound("Company not found.");
            }

            _context.Company.Remove(company);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateCompany(int id, [FromBody] CompanyRequestDto updateCompanyDto)
        {
            var company = await _context.Company.FirstOrDefaultAsync(c => c.companyID == id);
            if (company == null)
            {
                return NotFound("Company not found.");
            }

            if (string.IsNullOrWhiteSpace(updateCompanyDto.companyName))
            {
                return BadRequest("Company name is required.");
            }

            company.companyName = updateCompanyDto.companyName;
            await _context.SaveChangesAsync();

            return NoContent();
        }
    [HttpGet("{companyId}/full-data")]
public async Task<IActionResult> GetFullCompanyData(int companyId)
{
  var branches = await _context.Branch
        .Include(b => b.Manager)
        .Where(b => b.Manager != null && b.Manager.companyID == companyId)
        .Select(b => new {
            branchID = b.branchID,
            branchName = b.branchName,
            manager = b.Manager!.UserName
        })
        .ToListAsync();

    var shiftTypes = await _context.ShiftType
        .Where(st => st.companyID == companyId)
        .Select(st => new {
            shiftTypeID = st.shiftTypeID,
            shiftTypeName = st.shiftTypeName
        })
        .ToListAsync();

    var professions = await _context.Profession
        .Where(p => p.companyID == companyId)
        .Select(p => new {
            professionID = p.professionID,
            professionName = p.professionName
        })
        .ToListAsync();

  var workers = await _context.Worker
    .Where(w => _context.Branch
        .Where(b => b.Manager != null && b.Manager.companyID == companyId)
        .Select(b => b.branchID)
        .Contains(w.branchID))
    .Select(w => new {
        workerID = w.workerID,
        workerName = w.workerName,
        branchID = w.branchID,
        professions = w.WorkerProfessions
            .Select(wp => new {
                professionID = wp.Profession!.professionID,
                professionName = wp.Profession.professionName
            })
            .ToList()
    })
    .ToListAsync();

    return Ok(new
    {
        branches,
        shiftTypes,
        professions,
        workers
    });
}
    }
}
