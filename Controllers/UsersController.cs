using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RestApiDating.Data;

namespace RestApiDating.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IDatingRepository _repository;

        public UsersController(IDatingRepository repository)
        {
            _repository = repository;

        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _repository.GetUsers();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _repository.GetUser(id);
            return Ok(user);
        }
    }
}