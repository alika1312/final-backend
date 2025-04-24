using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api.Models; 

namespace api.Controllers
{
    [Route("api/shifttype")]
    [ApiController]
    public class ShiftTypeController : ControllerBase
    {
        private readonly ApplicationDBContext _context;

        public ShiftTypeController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var shiftTypes = await _context.ShiftType.ToListAsync();
            return Ok(shiftTypes);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var shiftType = await _context.ShiftType.FindAsync(id);
            if (shiftType == null) return NotFound();
            return Ok(shiftType);
        }

     
   
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteShiftType(int id)
        {
            var shiftType = await _context.ShiftType.FindAsync(id);
            if (shiftType == null) return NotFound();

            _context.ShiftType.Remove(shiftType);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateShiftType(int id, [FromBody] ShiftTypeRequestDto updateShiftTypeDto)
        {
            var shiftType = await _context.ShiftType.FindAsync(id);
            if (shiftType == null) return NotFound();

            shiftType.shiftTypeName = updateShiftTypeDto.shiftTypeName;
            await _context.SaveChangesAsync();

            return NoContent();
        }
         [HttpPost("bulk")]
        public async Task<IActionResult> CreateShiftTypes([FromBody] List<ShiftTypeRequestDto> shiftTypeDtos)
      {
    if (shiftTypeDtos == null || !shiftTypeDtos.Any())
    {
        return BadRequest("No shift types provided.");
    }

    // Optional: Validate each entry
    var invalidNames = shiftTypeDtos
        .Where(dto => string.IsNullOrWhiteSpace(dto.shiftTypeName))
        .Select((dto, index) => index + 1)
        .ToList();

    if (invalidNames.Any())
    {
        return BadRequest($"ShiftType name is required for entries at positions: {string.Join(", ", invalidNames)}");
    }

    // Convert all DTOs to entities
    var shiftTypes = shiftTypeDtos
        .Select(dto => dto.ToShiftTypeFromDto())
        .ToList();

    _context.ShiftType.AddRange(shiftTypes);
    await _context.SaveChangesAsync();

    // Convert saved entities to DTOs to return
    var createdDtos = shiftTypes
        .Select(st => st.ToShiftTypeDto())
        .ToList();

    return Ok(createdDtos);
}

[HttpPost("company/{companyId:int}")]
public async Task<IActionResult> CreateShiftTypesForCompany(int companyId, [FromBody] List<string> shiftTypeNames)
{
    if (shiftTypeNames == null || !shiftTypeNames.Any())
    {
        return BadRequest("No shift type names provided.");
    }

    var company = await _context.Company
        .Include(c => c.shiftTypes)
        .FirstOrDefaultAsync(c => c.companyID == companyId);

    if (company == null)
    {
        return NotFound("Company not found.");
    }

    var newShiftTypes = shiftTypeNames.Select(name => new ShiftType
    {
        shiftTypeName = name,
        companyID = companyId
    }).ToList();

    _context.ShiftType.AddRange(newShiftTypes);
    await _context.SaveChangesAsync();

    return Ok(newShiftTypes);
}


[HttpGet("company/{companyId:int}")]
public async Task<IActionResult> GetCompanyShiftTypes(int companyId)
{
    var shiftTypes = await _context.ShiftType
        .Where(st => st.companyID == companyId)
        .ToListAsync();

    return Ok(shiftTypes);
}

    }
}

