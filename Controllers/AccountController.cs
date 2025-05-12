using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using api.Dtos;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace api.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;

        public AccountController(UserManager<ApplicationUser> userManager,
                                 SignInManager<ApplicationUser> signInManager,
                                 IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRequestDto userRequestDto)
{
    var existingUser = await _userManager.FindByNameAsync(userRequestDto.username);
    if (existingUser != null)
    {
        return BadRequest("User already exists.");
    }

    var user = new ApplicationUser
    {
        UserName = userRequestDto.username,
        isCEO = userRequestDto.isCEO,
        companyID = userRequestDto.companyID
    };

    var result = await _userManager.CreateAsync(user, userRequestDto.password);
    if (!result.Succeeded)
    {
        return BadRequest(result.Errors);
    }

    // Fetch the created user to get their ID
    var createdUser = await _userManager.FindByNameAsync(userRequestDto.username);

    return Ok(new
    {
        message = "User registered successfully.",
        user = new
        {
            id = createdUser.Id,
            username = createdUser.UserName,
            isCEO = createdUser.isCEO,
            companyID = createdUser.companyID
        }
    });
}




        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var user = await _userManager.FindByNameAsync(loginDto.username);
            if (user == null)
            {
                return Unauthorized("Invalid credentials.");
            }


            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.password, false);
            if (!result.Succeeded)
            {
                return Unauthorized("Invalid credentials.");
            }


            var token = GenerateJwtToken(user);
            return Ok(new { token });
        }

        private string GenerateJwtToken(ApplicationUser user)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var key = Encoding.UTF8.GetBytes(jwtSettings["SigningKey"]!);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName ?? string.Empty),
                new Claim("companyId", user.companyID.ToString()),
                new Claim("userId", user.Id.ToString()),
                new Claim("isCEO", user.isCEO.ToString()),
                new Claim("username", user.UserName!.ToString())

            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
              
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(60),
                Issuer = jwtSettings["Issuer"],
                Audience = jwtSettings["Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(securityToken);
        }
    }
}
