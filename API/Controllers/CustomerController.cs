// Revisado
using API.DataServices;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/customer")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerDataService _dataService;

        public CustomerController(ICustomerDataService dataService)
        {
            _dataService = dataService ?? throw new ArgumentNullException(nameof(dataService));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var customers = await _dataService.GetAll();
            return customers.Any()
                ? Ok(customers)
                : NotFound(new { message = "No se encontraron clientes." });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByKey(int id)
        {
            var customer = await _dataService.GetByKey(id);
            return customer != null
                ? Ok(customer)
                : NotFound(new { message = $"No se encontró cliente con el identificador {id}." });
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Customer customer)
        {
            await _dataService.Add(customer);
            return CreatedAtAction(nameof(GetByKey), new { id = customer.Id }, customer);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Customer customer)
        {
            if (id != customer.Id)
                return BadRequest(new { message = "El ID de la URL no coincide con el del cuerpo." });

            await _dataService.Update(customer);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var customer = await _dataService.GetByKey(id);
            if (customer == null)
                return NotFound(new { message = $"No se encontró cliente con el identificador {id}." });

            await _dataService.Remove(customer);
            return NoContent();
        }

        [HttpPost("add-multiple")]
        public async Task<IActionResult> AddMultiple([FromBody] List<Customer> customers)
        {
            if (customers == null || customers.Count == 0)
                return BadRequest(new { message = "La lista de clientes está vacía." });

            await _dataService.AddMultiple(customers);
            return Ok(new { message = "Clientes agregados correctamente." });
        }

        [HttpPut("update-multiple")]
        public async Task<IActionResult> UpdateMultiple([FromBody] List<Customer> customers)
        {
            if (customers == null || customers.Count == 0)
                return BadRequest(new { message = "La lista de clientes está vacía." });

            await _dataService.UpdateMultiple(customers);
            return NoContent();
        }

        [HttpDelete("remove-multiple")]
        public async Task<IActionResult> RemoveMultiple([FromBody] List<Customer> customers)
        {
            if (customers == null || customers.Count == 0)
                return BadRequest(new { message = "La lista de clientes a eliminar está vacía." });

            await _dataService.RemoveMultiple(customers);
            return NoContent();
        }
    }
}