// Revisado
using API.DataServices;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/product")]
    public class ProductController : ControllerBase
    {
        private readonly IProductDataService _dataService;

        public ProductController(IProductDataService dataService)
        {
            _dataService = dataService ?? throw new ArgumentNullException(nameof(dataService));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _dataService.GetAll();
            return products.Any()
                ? Ok(products)
                : NotFound(new { message = "No se encontraron productos." });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByKey(int id)
        {
            var product = await _dataService.GetByKey(id);
            return product != null
                ? Ok(product)
                : NotFound(new { message = $"No se encontró producto con el identificador {id}." });
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Product product)
        {
            await _dataService.Add(product);
            return CreatedAtAction(nameof(GetByKey), new { id = product.Id }, product);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Product product)
        {
            if (id != product.Id)
                return BadRequest(new { message = "El ID de la URL no coincide con el del cuerpo." });

            await _dataService.Update(product);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var product = await _dataService.GetByKey(id);
            if (product == null)
                return NotFound(new { message = $"No se encontró producto con el identificador {id}." });

            await _dataService.Remove(product);
            return NoContent();
        }

        [HttpPost("add-multiple")]
        public async Task<IActionResult> AddMultiple([FromBody] List<Product> products)
        {
            if (products == null || products.Count == 0)
                return BadRequest(new { message = "La lista de productos está vacía." });

            await _dataService.AddMultiple(products);
            return Ok(new { message = "Productos agregados correctamente." });
        }

        [HttpPut("update-multiple")]
        public async Task<IActionResult> UpdateMultiple([FromBody] List<Product> products)
        {
            if (products == null || products.Count == 0)
                return BadRequest(new { message = "La lista de productos está vacía." });

            await _dataService.UpdateMultiple(products);
            return NoContent();
        }

        [HttpDelete("remove-multiple")]
        public async Task<IActionResult> RemoveMultiple([FromBody] List<Product> products)
        {
            if (products == null || products.Count == 0)
                return BadRequest(new { message = "La lista de productos a eliminar está vacía." });

            await _dataService.RemoveMultiple(products);
            return NoContent();
        }
    }
}