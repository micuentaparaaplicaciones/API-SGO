using API.DataServices;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/category")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryDataService _dataService;

        public CategoryController(ICategoryDataService dataService)
        {
            _dataService = dataService ?? throw new ArgumentNullException(nameof(dataService));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _dataService.GetAll();
            return categories.Any()
                ? Ok(categories)
                : NotFound(new { message = "No se encontraron categorías." });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByKey(int id)
        {
            var category = await _dataService.GetByKey(id);
            return category != null
                ? Ok(category)
                : NotFound(new { message = $"No se encontró categoría con el identificador {id}." });
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Category category)
        {
            await _dataService.Add(category);
            return CreatedAtAction(nameof(GetByKey), new { id = category.Id }, category);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Category category)
        {
            if (id != category.Id)
                return BadRequest(new { message = "El ID de la URL no coincide con el del cuerpo." });

            await _dataService.Update(category);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var category = await _dataService.GetByKey(id);
            if (category == null)
                return NotFound(new { message = $"No se encontró categoría con el identificador {id}." });

            await _dataService.Remove(category);
            return NoContent();
        }

        [HttpPost("add-multiple")]
        public async Task<IActionResult> AddMultiple([FromBody] List<Category> categories)
        {
            if (categories == null || categories.Count == 0)
                return BadRequest(new { message = "La lista de categorías está vacía." });

            await _dataService.AddMultiple(categories);
            return Ok(new { message = "Categorías agregadas correctamente." });
        }

        [HttpPut("update-multiple")]
        public async Task<IActionResult> UpdateMultiple([FromBody] List<Category> categories)
        {
            if (categories == null || categories.Count == 0)
                return BadRequest(new { message = "La lista de categorías está vacía." });

            await _dataService.UpdateMultiple(categories);
            return NoContent();
        }

        [HttpDelete("remove-multiple")]
        public async Task<IActionResult> RemoveMultiple([FromBody] List<Category> categories)
        {
            if (categories == null || categories.Count == 0)
                return BadRequest(new { message = "La lista de categorías a eliminar está vacía." });

            await _dataService.RemoveMultiple(categories);
            return NoContent();
        }
    }
}
