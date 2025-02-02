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
    [Route("api/profession")]
    [ApiController]
    public class ProfessionController : ControllerBase
    {
        private readonly ApplicationDBContext _context;

        public ProfessionController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProfessions()
        {
            var professions = await _context.Profession
                                            .Select(p => p.ToProfessionDto())
                                            .ToListAsync();

            return Ok(professions);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProfessionById(int id)
        {
            var profession = await _context.Profession
                                           .FirstOrDefaultAsync(p => p.professionID == id);

            if (profession == null)
            {
                return NotFound("Profession not found.");
            }

            return Ok(profession.ToProfessionDto());
        }

        [HttpPost]
        public async Task<IActionResult> CreateProfession([FromBody] CreateProfessionDto createProfessionDto)
        {
            if (string.IsNullOrWhiteSpace(createProfessionDto.professionName))
            {
                return BadRequest("Profession name is required.");
            }

            var existingProfession = await _context.Profession
                                                   .FirstOrDefaultAsync(p => p.professionName == createProfessionDto.professionName);

            if (existingProfession != null)
            {
                return BadRequest("A profession with this name already exists.");
            }

            var profession = createProfessionDto.ToProfession();
            _context.Profession.Add(profession);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProfessionById), new { id = profession.professionID }, profession.ToProfessionDto());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProfession(int id)
        {
            var profession = await _context.Profession.FindAsync(id);
            if (profession == null)
            {
                return NotFound("Profession not found.");
            }

            _context.Profession.Remove(profession);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProfession(int id, [FromBody] CreateProfessionDto updateProfessionDto)
        {
            var profession = await _context.Profession.FindAsync(id);
            if (profession == null)
            {
                return NotFound("Profession not found.");
            }

            if (string.IsNullOrWhiteSpace(updateProfessionDto.professionName))
            {
                return BadRequest("Profession name is required.");
            }

            profession.professionName = updateProfessionDto.professionName;
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
