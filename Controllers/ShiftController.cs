using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos;
using api.Dtos.Shift;
using api.Mappers;
using api.Models;
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

      
         [HttpGet("{branchId:int}")]
        public async Task<IActionResult> GetShiftsByDate(int branchId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var shifts = await _context.Shift
                .Where(s => s.branchID == branchId)
                .Where(s => s.date >= DateOnly.FromDateTime(startDate) && s.date <= DateOnly.FromDateTime(endDate))
                .Select(s => new 
                {
                    shiftID = s.shiftID,
                    extra = s.extra,
                    comment = s.comment,
                      date = s.date,
                    worker = s.WorkerShifts
                  
                        .Select(ws => new 
                        {
                            workerID = ws.Worker!.workerID,
                            workerName = ws.Worker.workerName,
                            workerProfession = ws.Worker.WorkerProfessions
                                .Select(wp => new 
                                {
                                    professionID = wp.Profession!.professionID,
                                    professionName = wp.Profession.professionName
                                })
                                .ToList()
                        })
                        .ToList(),
          
                    
                    shiftType = new 
                    {
                        shiftTypeID = s.ShiftType!.shiftTypeID,
                        shiftTypeName = s.ShiftType.shiftTypeName
                    }
                })
                .ToListAsync();

            if (shifts == null || !shifts.Any())
            {
                return NotFound($"No shifts found for Branch ID {branchId} in the specified date range.");
            }

           
            return Ok(shifts);
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
            return CreatedAtAction(nameof(GetAll), new { id = shiftDto.shiftID }, shiftDto);
        }
    [HttpPost("bulk/create")]
public async Task<IActionResult> CreateOrUpdateMultipleShifts([FromBody] List<ShiftRequestDto> shiftRequestDtos)
{
    if (shiftRequestDtos == null || !shiftRequestDtos.Any())
    {
        return BadRequest("No shifts provided.");
    }

    var processedShifts = new List<ShiftDto>();

    foreach (var shiftRequestDto in shiftRequestDtos)
    {
        
        var shiftType = await _context.ShiftType
            .FirstOrDefaultAsync(st => st.shiftTypeID == shiftRequestDto.ShiftTypeID);

        if (shiftType == null)
        {
            return BadRequest($"Invalid ShiftTypeID: {shiftRequestDto.ShiftTypeID}");
        }

        Shift shiftModel;

        
        if (shiftRequestDto.shiftID > 0)
        {
            var foundShift = await _context.Shift
                .FirstOrDefaultAsync(s => s.shiftID == shiftRequestDto.shiftID);

         if (foundShift == null)
{
    return NotFound($"Shift with ID {shiftRequestDto.shiftID} not found.");
}

            shiftModel = foundShift!;


            shiftModel.date = shiftRequestDto.date;
            shiftModel.ShiftTypeID = shiftRequestDto.ShiftTypeID;
            shiftModel.branchID = shiftRequestDto.branchID;
            shiftModel.comment = shiftRequestDto.comment;
           
        }
        else
        {
            
            shiftModel = shiftRequestDto.ToShiftFromCreateDTO(_context);
            _context.Shift.Add(shiftModel);
        }

        var shiftDto = shiftModel.ToShiftDto(_context);
        processedShifts.Add(shiftDto);
    }

    await _context.SaveChangesAsync();
    return Ok(processedShifts);
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
