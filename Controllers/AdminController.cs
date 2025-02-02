using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api.Data;
using api.Dtos;
using api.Mappers;

namespace api.Controllers
{
    [Route("api/admin")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly ApplicationDBContext _context;

        public AdminController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var admins = await _context.Admin
                                       .Include(a => a.User)
                                       .Select(a => a.ToAdminDto())
                                       .ToListAsync();
            return Ok(admins);
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetAdminByUserId(int userId)
        {
            var admin = await _context.Admin
                                      .Include(a => a.User)
                                      .FirstOrDefaultAsync(a => a.userID == userId);

            if (admin == null)
            {
                return NotFound("Admin not found.");
            }

            var adminDto = admin.ToAdminDto();
            return Ok(adminDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAdmin([FromBody] CreateAdminRequestDto createAdminDto)
        {

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == createAdminDto.userID);

            if (user == null)
            {
                return NotFound("User not found.");
            }


            var existingAdmin = await _context.Admin.FirstOrDefaultAsync(a => a.userID == createAdminDto.userID);
            if (existingAdmin != null)
            {
                return BadRequest("User is already an admin.");
            }


            var admin = createAdminDto.ToAdminsFromDto();
            _context.Admin.Add(admin);
            await _context.SaveChangesAsync();

            var adminDto = admin.ToAdminDto();
            return CreatedAtAction(nameof(GetAdminByUserId), new { userId = admin.userID }, adminDto);
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteAdmin(int userId)
        {
            var admin = await _context.Admin.FirstOrDefaultAsync(a => a.userID == userId);
            if (admin == null)
            {
                return NotFound("Admin not found.");
            }

            _context.Admin.Remove(admin);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
