// Revisado
using API.DataServices;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/supplier")]
    public class SupplierController : ControllerBase
    {
        private readonly ISupplierDataService _dataService;

        public SupplierController(ISupplierDataService dataService)
        {
            _dataService = dataService ?? throw new ArgumentNullException(nameof(dataService));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var suppliers = await _dataService.GetAll();
            return suppliers.Any()
                ? Ok(suppliers)
                : NotFound(new { message = "No se encontraron proveedores." });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByKey(int id)
        {
            var supplier = await _dataService.GetByKey(id);
            return supplier != null
                ? Ok(supplier)
                : NotFound(new { message = $"No se encontró proveedor con el identificador {id}." });
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Supplier supplier)
        {
            await _dataService.Add(supplier);
            return CreatedAtAction(nameof(GetByKey), new { id = supplier.Id }, supplier);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Supplier supplier)
        {
            if (id != supplier.Id)
                return BadRequest(new { message = "El ID de la URL no coincide con el del cuerpo." });

            await _dataService.Update(supplier);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var supplier = await _dataService.GetByKey(id);
            if (supplier == null)
                return NotFound(new { message = $"No se encontró proveedor con el identificador {id}." });

            await _dataService.Remove(supplier);
            return NoContent();
        }

        [HttpPost("add-multiple")]
        public async Task<IActionResult> AddMultiple([FromBody] List<Supplier> suppliers)
        {
            if (suppliers == null || suppliers.Count == 0)
                return BadRequest(new { message = "La lista de proveedores está vacía." });

            await _dataService.AddMultiple(suppliers);
            return Ok(new { message = "Proveedores agregados correctamente." });
        }

        [HttpPut("update-multiple")]
        public async Task<IActionResult> UpdateMultiple([FromBody] List<Supplier> suppliers)
        {
            if (suppliers == null || suppliers.Count == 0)
                return BadRequest(new { message = "La lista de proveedores está vacía." });

            await _dataService.UpdateMultiple(suppliers);
            return NoContent();
        }

        [HttpDelete("remove-multiple")]
        public async Task<IActionResult> RemoveMultiple([FromBody] List<Supplier> suppliers)
        {
            if (suppliers == null || suppliers.Count == 0)
                return BadRequest(new { message = "La lista de proveedores a eliminar está vacía." });

            await _dataService.RemoveMultiple(suppliers);
            return NoContent();
        }
    }
}