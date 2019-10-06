using System;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RestApiDating.Data;
using RestApiDating.Dtos;
using RestApiDating.Helpers;
using RestApiDating.Models;

namespace RestApiDating.Controllers
{
    [ApiController]
    [Route("api/users/{userId}/[controller]")]
    [ServiceFilter(typeof(LogUserActivity))]
    public class MensajesController : ControllerBase
    {
        private IDatingRepository _repository;
        private IMapper _mapper;

        public MensajesController(IDatingRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet("{id}", Name = "GetMensaje")]
        public async Task<IActionResult> GetMensaje(int userId, int id)
        {
            if (!IsValidUser(userId)) return Unauthorized();

            var mensaje = await _repository.GetMensaje(id);

            if (mensaje == null) return NotFound();

            var dto = _mapper.Map<MensajeDetailDto>(mensaje);

            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create(int userId, MensajeCreateDto dto)
        {
            if (!IsValidUser(userId)) return Unauthorized();

            var receptor = await _repository.GetUser(dto.ReceptorId);
            if (receptor == null) return BadRequest("No existe el usuario receptor");

            var mensaje = _mapper.Map<Mensaje>(dto);
            mensaje.EmisorId = userId;

            _repository.Add<Mensaje>(mensaje);

            if (await _repository.SaveAll())
            {
                var dtoDetail = _mapper.Map<MensajeDetailDto>(mensaje);
                return CreatedAtRoute("GetMensaje", new { Id = mensaje.Id }, dtoDetail);
            }

            throw new Exception("Ocurrio un error al intentar enviar el mensaje");
        }

        private bool IsValidUser(int userId)
        {
            var userLoggedId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            return userLoggedId == userId;
        }
    }
}