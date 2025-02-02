using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var shiftType = await _context.ShiftType.FindAsync(id);
            if (shiftType == null) return NotFound();
            return Ok(shiftType);
        }

        [HttpPost]
        public async Task<IActionResult> CreateShiftType([FromBody] ShiftTypeRequestDto createShiftTypeDto)
        {
            if (string.IsNullOrWhiteSpace(createShiftTypeDto.shiftTypeName))
            {
                return BadRequest("ShiftType name is required.");
            }

            var shiftType = createShiftTypeDto.ToShiftTypeFromDto();

            _context.ShiftType.Add(shiftType);
            await _context.SaveChangesAsync();

            var shiftTypeDto = shiftType.ToShiftTypeDto();
            return CreatedAtAction(nameof(GetById), new { id = shiftType.shiftTypeID }, shiftTypeDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShiftType(int id)
        {
            var shiftType = await _context.ShiftType.FindAsync(id);
            if (shiftType == null) return NotFound();

            _context.ShiftType.Remove(shiftType);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateShiftType(int id, [FromBody] ShiftTypeRequestDto updateShiftTypeDto)
        {
            var shiftType = await _context.ShiftType.FindAsync(id);
            if (shiftType == null) return NotFound();

            shiftType.shiftTypeName = updateShiftTypeDto.shiftTypeName;
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
