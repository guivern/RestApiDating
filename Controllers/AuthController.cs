using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
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
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repository;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public AuthController(IAuthRepository repository, IConfiguration configuration, IMapper mapper)
        {
            _repository = repository;
            _configuration = configuration;
            _mapper = mapper;
        }

        [HttpPost("[action]")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            registerDto.Username = registerDto.Username.ToLower();
            var user = _mapper.Map<User>(registerDto);

            if (await _repository.UserExits(registerDto.Username))
            {
                ModelState.AddModelError(nameof(registerDto.Username), "Ya existe el usuario");
                return BadRequest(ModelState);
            }

            var newUser = await _repository.Register(user, registerDto.Password);
            var userDto = _mapper.Map<UserDetailDto>(newUser);

            return CreatedAtRoute("GetUser", new { controller = "users", id = user.Id }, userDto);
        }

        [HttpPost("[action]")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginDto credencial)
        {
            var user = await _repository.Login(credencial.Username, credencial.Password);
            if (user == null) return Unauthorized();

            var userDto = _mapper.Map<UserListDto>(user);
            var token = GenerateToken(user);

            return Ok(new { token, userDto });
        }

        // genera un token para el usuario 
        private string GenerateToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                // aqui podemos agregar mas claims como rol, permisos, foto, etc.
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8
            .GetBytes(_configuration.GetSection("Jwt:Key").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now .AddDays(Double.Parse(
                    _configuration.GetSection("Jwt:ExpireDays").Value)), 
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}