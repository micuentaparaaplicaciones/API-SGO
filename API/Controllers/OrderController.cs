// Revisado
using API.DataServices;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/order")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderDataService _dataService;

        public OrderController(IOrderDataService dataService)
        {
            _dataService = dataService ?? throw new ArgumentNullException(nameof(dataService));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var orders = await _dataService.GetAll();
            return orders.Any()
                ? Ok(orders)
                : NotFound(new { message = "No se encontraron órdenes." });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByKey(int id)
        {
            var order = await _dataService.GetByKey(id);
            return order != null
                ? Ok(order)
                : NotFound(new { message = $"No se encontró orden con el identificador {id}." });
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Order order)
        {
            await _dataService.Add(order);
            return CreatedAtAction(nameof(GetByKey), new { id = order.Id }, order);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Order order)
        {
            if (id != order.Id)
                return BadRequest(new { message = "El ID de la URL no coincide con el del cuerpo." });

            await _dataService.Update(order); 
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var order = await _dataService.GetByKey(id);
            if (order == null)
                return NotFound(new { message = $"No se encontró orden con el identificador {id}." });

            await _dataService.Remove(order); 
            return NoContent();
        }

        [HttpPost("add-multiple")]
        public async Task<IActionResult> AddMultiple([FromBody] List<Order> orders)
        {
            if (orders == null || orders.Count == 0)
                return BadRequest(new { message = "La lista de órdenes está vacía." });

            await _dataService.AddMultiple(orders);
            return Ok(new { message = "Órdenes agregadas correctamente." });
        }

        [HttpPut("update-multiple")]
        public async Task<IActionResult> UpdateMultiple([FromBody] List<Order> orders)
        {
            if (orders == null || orders.Count == 0)
                return BadRequest(new { message = "La lista de órdenes está vacía." });

            await _dataService.UpdateMultiple(orders);
            return NoContent();
        }

        [HttpDelete("remove-multiple")]
        public async Task<IActionResult> RemoveMultiple([FromBody] List<Order> orders)
        {
            if (orders == null || orders.Count == 0)
                return BadRequest(new { message = "La lista de órdenes está vacía." });

            await _dataService.RemoveMultiple(orders);
            return NoContent();
        }

    }

}