using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos;
using api.Dtos.Shift;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("api/shift")]
    [ApiController]
    public class ShiftController : ControllerBase
    {
        private readonly ApplicationDBContext _context;

        public ShiftController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var shifts = await _context.Shift.ToListAsync();
            var shiftDtos = shifts.Select(s => s.ToShiftDto(_context));
            return Ok(shiftDtos);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetShiftById(int id)
        {
            var shift = await _context.Shift
                .Where(s => s.shiftID == id)
                .Include(s => s.ShiftType)
                .FirstOrDefaultAsync();

            if (shift == null)
            {
                return NotFound($"Shift with ID {id} not found.");
            }

            var shiftDto = shift.ToShiftDto(_context);
            return Ok(shiftDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateShift([FromBody] ShiftRequestDto shiftRequestDto)
        {
            var shiftType = await _context.ShiftType.FirstOrDefaultAsync(st => st.shiftTypeID == shiftRequestDto.ShiftTypeID);
            if (shiftType == null)
            {
                return BadRequest("Invalid ShiftTypeID.");
            }

            var shiftModel = shiftRequestDto.ToShiftFromCreateDTO(_context);
            _context.Shift.Add(shiftModel);
            await _context.SaveChangesAsync();

            var shiftDto = shiftModel.ToShiftDto(_context);
            return CreatedAtAction(nameof(GetShiftById), new { id = shiftModel.shiftID }, shiftDto);
        }
        [HttpPost("bulk")]
public async Task<IActionResult> CreateMultipleShifts([FromBody] List<ShiftRequestDto> shiftRequestDtos)
{
    if (shiftRequestDtos == null || !shiftRequestDtos.Any())
    {
        return BadRequest("No shifts provided.");
    }

    var createdShifts = new List<ShiftDto>();

    foreach (var shiftRequestDto in shiftRequestDtos)
    {
        var shiftType = await _context.ShiftType
            .FirstOrDefaultAsync(st => st.shiftTypeID == shiftRequestDto.ShiftTypeID);

        if (shiftType == null)
        {
            return BadRequest($"Invalid ShiftTypeID: {shiftRequestDto.ShiftTypeID}");
        }

        var shiftModel = shiftRequestDto.ToShiftFromCreateDTO(_context);
        _context.Shift.Add(shiftModel);

        var shiftDto = shiftModel.ToShiftDto(_context);
        createdShifts.Add(shiftDto);
    }

    await _context.SaveChangesAsync();

    return Ok(createdShifts);
}


        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteShift(int id)
        {
            var shift = await _context.Shift.FirstOrDefaultAsync(s => s.shiftID == id);
            if (shift == null)
            {
                return NotFound($"Shift with ID {id} not found.");
            }

            _context.Shift.Remove(shift);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateShift(int id, [FromBody] ShiftRequestDto updateShiftDto)
        {
            var shift = await _context.Shift.FirstOrDefaultAsync(s => s.shiftID == id);
            if (shift == null)
            {
                return NotFound($"Shift with ID {id} not found.");
            }

            if (!await _context.ShiftType.AnyAsync(st => st.shiftTypeID == updateShiftDto.ShiftTypeID))
            {
                return BadRequest("Invalid ShiftTypeID.");
            }


            shift.ShiftTypeID = updateShiftDto.ShiftTypeID;
            shift.extra = updateShiftDto.extra;
            shift.comment = updateShiftDto.comment;

            await _context.SaveChangesAsync();

            var shiftDto = shift.ToShiftDto(_context);
            return Ok(shiftDto);
        }
    }
}
