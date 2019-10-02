using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestApiDating.Data;
using RestApiDating.Dtos;
using RestApiDating.Helpers;
using RestApiDating.Models;

namespace RestApiDating.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(LogUserActivity))]
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
        public async Task<IActionResult> GetAll([FromQuery]UserParams userParams)
        {
            userParams.UserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var currentUser = await _repository.GetUser(userParams.UserId);

            if(string.IsNullOrEmpty(userParams.Genero))
            {
                // por default filtramos por el sexo opuesto
                userParams.Genero = currentUser.Genero.ToLower().Equals("hombre") ? "mujer" : "hombre";
            }

            var users = await _repository.GetUsers(userParams);
            var usersDto = _mapper.Map<List<UserListDto>>(users);

            Response.AddPagination(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);

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

        [HttpPost("{id}/like/{likedId}")]
        public async Task<IActionResult> Like(int id, int likedId)
        {
            var tokenUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if(id != tokenUserId) return Unauthorized();

            if(id == likedId) return BadRequest();
            
            var like = await _repository.GetLike(id, likedId);
            if(like != null) return BadRequest("Ya has dado like a este usuario");

            var userLiked = await _repository.GetUser(likedId);
            if(userLiked == null) return BadRequest("No existe el usuario");

            var newLike = new Like()
            {
                LikerId = id,
                LikedId = likedId
            };

            _repository.Add<Like>(newLike);

            if(await _repository.SaveAll()) return Ok();

            return BadRequest("Ocurrió un error");
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