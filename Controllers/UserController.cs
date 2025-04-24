using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using api.Models;
using api.Dtos;
using api.Mappers;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public UserController(UserManager<ApplicationUser> userManager,
                              SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }


        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {

            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return NotFound();
            }


            var userDto = user.ToUserDto();
            return Ok(userDto);
        }


        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserRequestDto userRequestDto)
        {

            var existingCEO = await _userManager.Users.FirstOrDefaultAsync(u => u.isCEO);
            if (userRequestDto.isCEO && existingCEO != null)
            {
                return BadRequest("There is already a CEO in the system.");
            }


            var userModel = new ApplicationUser
            {
                UserName = userRequestDto.username,
                isCEO = userRequestDto.isCEO,
                companyID = userRequestDto.companyID
            };

            var result = await _userManager.CreateAsync(userModel, userRequestDto.password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            var userDto = userModel.ToUserDto();
            return CreatedAtAction(nameof(GetById), new { id = userModel.Id }, userDto);
        }


        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserRequestDto updateUserDto)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return NotFound();
            }

            user.UserName = updateUserDto.username;
            user.isCEO = updateUserDto.isCEO;
            user.companyID = updateUserDto.companyID;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return NoContent();
        }


        [HttpPut("{id:int}/password")]
        public async Task<IActionResult> ChangePassword(int id, [FromBody] ChangePasswordDto changePasswordDto)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return NotFound();
            }


            var result = await _userManager.ChangePasswordAsync(user, changePasswordDto.CurrentPassword, changePasswordDto.NewPassword);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return NoContent();
        }
    }
}
