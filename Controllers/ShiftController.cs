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
            var shiftDtos = shifts.Select(s => s.ToShiftDto());
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

           

           
            return Ok(shifts);
        }

[HttpDelete("delete/{branchId:int}")]
        public async Task<IActionResult> DeleteShiftsByDate(int branchId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var shifts = await _context.Shift
                .Where(s => s.branchID == branchId)
                .Where(s => s.date >= DateOnly.FromDateTime(startDate) && s.date <= DateOnly.FromDateTime(endDate)).ToListAsync();
            foreach (var shift in shifts)
            {
                _context.Shift.Remove(shift);
                await _context.SaveChangesAsync();
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

            var shiftDto = shiftModel.ToShiftDto();
            return CreatedAtAction(nameof(GetAll), new { id = shiftDto.shiftID }, shiftDto);
        }
[HttpPost("bulk")]
public async Task<IActionResult> CreateOrUpdateMultipleShifts([FromBody] List<ShiftWorkerRequestDto> shiftRequestDtos)
{
    if (shiftRequestDtos == null || !shiftRequestDtos.Any())
        return BadRequest("No shifts provided.");

    var processedShifts = new List<Shift>();
    var newAssignments = new List<WorkerShift>();

    var shiftIdsToRemoveAssignments = new List<int>();

    foreach (var dto in shiftRequestDtos)
    {
        var shiftTypeExists = await _context.ShiftType.AnyAsync(st => st.shiftTypeID == dto.ShiftTypeID);
        if (!shiftTypeExists) return BadRequest($"Invalid ShiftTypeID: {dto.ShiftTypeID}");

        var branchExists = await _context.Branch.AnyAsync(b => b.branchID == dto.branchID);
        if (!branchExists) return BadRequest($"Invalid branchID: {dto.branchID}");

        Shift? shift = await _context.Shift.FirstOrDefaultAsync(s => s.shiftID == dto.shiftID);

        if (dto.shiftID > 0)
        {
            if (shift == null) return NotFound($"Shift with ID {dto.shiftID} not found.");

            shift.date = DateOnly.FromDateTime(dto.date);
            shift.ShiftTypeID = dto.ShiftTypeID;
                    shift.extra = dto.extra;
            shift.branchID = dto.branchID;
            shift.comment = dto.comment;
        }
        else
        {
            shift = new Shift
            {
                date = DateOnly.FromDateTime(dto.date),
                ShiftTypeID = dto.ShiftTypeID,
                extra = dto.extra,
                branchID = dto.branchID,
                comment = dto.comment
            };

            _context.Shift.Add(shift);
        }

        processedShifts.Add(shift);
       
        shiftIdsToRemoveAssignments.Add(shift.shiftID);
    }


     await _context.SaveChangesAsync(); 

  

var oldAssignments = (await _context.WorkerShift
    .ToListAsync())
    .Where(ws => shiftIdsToRemoveAssignments.Contains(ws.ShiftID))
    .ToList();



    _context.WorkerShift.RemoveRange(oldAssignments);
    await _context.SaveChangesAsync();

    var allWorkers = await _context.Worker.ToListAsync();
    var validWorkers = allWorkers.ToDictionary(w => w.workerID);

    foreach (var dto in shiftRequestDtos)
    {
        var matchedShift = processedShifts.FirstOrDefault(s =>
            s.shiftID == dto.shiftID ||
            (dto.shiftID == 0 && s.date == DateOnly.FromDateTime(dto.date) &&
             s.ShiftTypeID == dto.ShiftTypeID && s.branchID == dto.branchID)
        );

        if (matchedShift == null) continue;

        foreach (var workerID in dto.WorkerIDs.Distinct())
        {
            if (!validWorkers.ContainsKey(workerID)) continue;

            newAssignments.Add(new WorkerShift
            {
                ShiftID = matchedShift.shiftID,
                WorkerID = workerID,
                Worker = validWorkers[workerID],
                Shift = matchedShift
            });
        }
    }

    await _context.WorkerShift.AddRangeAsync(newAssignments);
    await _context.SaveChangesAsync();

    var result = processedShifts.Select(s => s.ToShiftDto()).ToList();
    return Ok(result);
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

            var shiftDto = shift.ToShiftDto();
            return Ok(shiftDto);
        }
    }
}
