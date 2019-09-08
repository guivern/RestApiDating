using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RestApiDating.Data;
using RestApiDating.Dtos;
using RestApiDating.Models;

namespace RestApiDating.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IDatingRepository _repository;
        private readonly IMapper _mapper;

        public UsersController(IDatingRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;

        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _repository.GetUsers();
            var usersDto = _mapper.Map<List<UserListDto>>(users);

            return Ok(usersDto);
        }

        [HttpGet("{id}", Name = "GetUser")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _repository.GetUser(id);
            var dto = _mapper.Map<UserDetailDto>(user);

            return Ok(dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UserUpdateDto dto)
        {
            // solo el usuario propietario puede editar su perfil
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var user = await _repository.GetUser(id);
            user = _mapper.Map(dto, user);

            if (await _repository.SaveAll()) return NoContent();

            throw new Exception($"Error al intentar actualizar el usuario {id}");
        }

        [HttpGet("current")]
        public IActionResult GetCurrentUserId()
        {
            // obtiene el id del usuario a traves del token
            var id = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            return Ok(new { Id = id });
        }
    }
}