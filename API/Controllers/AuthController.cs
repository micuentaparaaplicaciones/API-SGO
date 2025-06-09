using API.DataServices;
using API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IUserDataService _userDataService;
        private readonly PasswordHasher<User> _passwordHasher;
        private readonly IConfiguration _configuration;

        public AuthController(IUserDataService userDataService, IConfiguration configuration)
        {
            _userDataService = userDataService;
            _passwordHasher = new PasswordHasher<User>();
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            var exists = await _userDataService.GetByEmail(dto.Email);
            if (exists != null)
                return BadRequest(new { message = "El correo ya está registrado." });

            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                Phone = dto.Phone,
                Address = dto.Address,
                Role = dto.Role,
                CreatedBy = dto.CreatedBy,
                ModifiedBy = dto.ModifiedBy
            };

            user.Password = _passwordHasher.HashPassword(user, dto.Password);

            await _userDataService.Add(user);
            return Ok(new { message = "Usuario registrado correctamente.", user.Id });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto login)
        {
            var user = await _userDataService.GetByEmail(login.Email);
            if (user == null)
                return Unauthorized(new { message = "Credenciales inválidas." });

            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, login.Password);
            if (result == PasswordVerificationResult.Failed)
                return Unauthorized(new { message = "Credenciales inválidas." });

            var token = GenerateJwtToken(user);
            return Ok(new { token });
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim("id", user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}