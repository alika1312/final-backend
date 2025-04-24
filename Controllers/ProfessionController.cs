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

        [HttpGet("{id:int}")]
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


            var profession = createProfessionDto.ToProfession();
            _context.Profession.Add(profession);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProfessionById), new { id = profession.professionID }, profession.ToProfessionDto());
        }

[HttpPost("bulk")]
public async Task<IActionResult> CreateMultipleProfessions([FromBody] List<CreateProfessionDto> createProfessionDtos)
{
    if (createProfessionDtos == null || !createProfessionDtos.Any())
    {
        return BadRequest("At least one profession must be provided.");
    }

    var invalidItems = createProfessionDtos
        .Where(dto => string.IsNullOrWhiteSpace(dto.professionName))
        .ToList();

    if (invalidItems.Any())
    {
        return BadRequest("All professions must have a valid name.");
    }

    var professions = createProfessionDtos
        .Select(dto => dto.ToProfession())
        .ToList();

    _context.Profession.AddRange(professions);
    await _context.SaveChangesAsync();

    var professionDtos = professions
        .Select(p => p.ToProfessionDto())
        .ToList();

    return Ok(professionDtos);
}


        [HttpDelete("{id:int}")]
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

        [HttpPut("{id:int}")]
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
