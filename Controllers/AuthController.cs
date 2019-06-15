using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RestApiDating.Data;
using RestApiDating.Dtos;
using RestApiDating.Models;

namespace RestApiDating.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController: ControllerBase
    {
        private readonly IAuthRepository _repository;
        private readonly IConfiguration _configuration;

        public AuthController(IAuthRepository repository, IConfiguration configuration)
        {
            _repository = repository;
            _configuration = configuration;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Register(UserDto dto)
        {
            dto.Username = dto.Username.ToLower();

            if(await _repository.UserExits(dto.Username))
            return BadRequest("Ya existe el usuario");
            
            var user = new User();
            user.Username = dto.Username;

            var newUser = await _repository.Register(user, dto.Password);

            return StatusCode(201);
        }

        [HttpPost("[action]")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var user = await _repository.Login(dto.Username, dto.Password);

            if(user == null) return Unauthorized();

            return Ok(new {token = GenerateToken(user)});
        }

        // genera un token para el usuario especificado
        private string GenerateToken(User user)
        {
            var claims = new []
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                // aqui podemos agregar mas claims como rol, permisos
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8
            .GetBytes(_configuration.GetSection("Jwt:Key").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1), // el token expira en 24hs
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}