// Revisado
using API.DataServices;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly IUserDataService _dataService;

        public UserController(IUserDataService dataService)
        {
            _dataService = dataService ?? throw new ArgumentNullException(nameof(dataService));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _dataService.GetAll();
            return users.Any()
                ? Ok(users)
                : NotFound(new { message = "No se encontraron usuarios." });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByKey(int id)
        {
            var user = await _dataService.GetByKey(id);
            return user != null
                ? Ok(user)
                : NotFound(new { message = $"No se encontró usuario con el identificador {id}." });
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] User user)
        {
            await _dataService.Add(user);
            return CreatedAtAction(nameof(GetByKey), new { id = user.Id }, user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] User user)
        {
            if (id != user.Id)
                return BadRequest(new { message = "El ID de la URL no coincide con el del cuerpo." });

            await _dataService.Update(user);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var user = await _dataService.GetByKey(id);
            if (user == null)
                return NotFound(new { message = $"No se encontró usuario con el identificador {id}." });

            await _dataService.Remove(user);
            return NoContent();
        }

        [HttpPost("add-multiple")]
        public async Task<IActionResult> AddMultiple([FromBody] List<User> users)
        {
            if (users == null || users.Count == 0)
                return BadRequest(new { message = "La lista de usuarios está vacía." });

            await _dataService.AddMultiple(users);
            return Ok(new { message = "Usuarios agregados correctamente." });
        }

        [HttpPut("update-multiple")]
        public async Task<IActionResult> UpdateMultiple([FromBody] List<User> users)
        {
            if (users == null || users.Count == 0)
                return BadRequest(new { message = "La lista de usuarios está vacía." });

            await _dataService.UpdateMultiple(users);
            return NoContent();
        }

        [HttpDelete("remove-multiple")]
        public async Task<IActionResult> RemoveMultiple([FromBody] List<User> users)
        {
            if (users == null || users.Count == 0)
                return BadRequest(new { message = "La lista de usuarios a eliminar está vacía." });

            await _dataService.RemoveMultiple(users);
            return NoContent();
        }
    }
}

