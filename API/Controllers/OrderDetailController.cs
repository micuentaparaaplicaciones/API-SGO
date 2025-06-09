// Revisado
using API.Models;
using API.DataServices;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/order-detail")]
    [ApiController]
    public class OrderDetailController : ControllerBase
    {
        private readonly IOrderDetailDataService _dataService;

        public OrderDetailController(IOrderDetailDataService dataService)
        {
            _dataService = dataService ?? throw new ArgumentNullException(nameof(dataService));
        }

        [HttpGet]
        public async Task<IActionResult> GetOrdersDetails()
        {
            var details = await _dataService.GetAll();
            return details.Any()
                ? Ok(details)
                : NotFound(new { message = "No se encontraron detalles de órdenes." });
        }

        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrderDetails(int orderId)
        {
            var details = await _dataService.GetOrderDetails(orderId);
            return details.Any()
                ? Ok(details)
                : NotFound(new { message = $"No se encontraron detalles de orden con el identificador {orderId}." });
        }

        [HttpGet("{orderId}/{productId}")]
        public async Task<IActionResult> GetByKey(int orderId, int productId)
        {
            var key = (orderId, productId); // <- clave compuesta
            var orderDetail = await _dataService.GetByKey(key);

            return orderDetail != null
                ? Ok(orderDetail)
                : NotFound(new
                {
                    message = $"No se encontró detalle de orden con el identificador de orden {orderId} y de producto {productId}."
                });
        }


        [HttpPost]
        public async Task<IActionResult> Add([FromBody] OrderDetail orderDetail)
        {
            await _dataService.Add(orderDetail);
            return CreatedAtAction(nameof(GetByKey), new { orderId = orderDetail.OrderId, productId = orderDetail.ProductId }, orderDetail);
        }

        [HttpPut("{orderId}/{productId}")]
        public async Task<IActionResult> Update(int orderId, int productId, [FromBody] OrderDetail orderDetail)
        {
            if (orderId != orderDetail.OrderId || productId != orderDetail.ProductId)
                return BadRequest(new { message = "El ID de la URL no coincide con el del cuerpo." });

            await _dataService.Update(orderDetail);
            return NoContent();
        }

        [HttpDelete("{orderId}/{productId}")]
        public async Task<IActionResult> Remove(int orderId, int productId)
        {
            var key = (orderId, productId); // <- clave compuesta
            var orderDetail = await _dataService.GetByKey(key);

            if (orderDetail == null)
                return NotFound(new
                {
                    message = $"No se encontró detalle de orden con el identificador de orden {orderId} y de producto {productId}."
                });

            await _dataService.Remove(orderDetail);
            return NoContent();
        }

        [HttpPost("add-multiple")]
        public async Task<IActionResult> AddMultiple([FromBody] List<OrderDetail> orderDetails)
        {
            if (orderDetails == null || orderDetails.Count == 0)
                return BadRequest(new { message = "La lista de detalles está vacía." });

            await _dataService.AddMultiple(orderDetails);
            return Ok(new { message = "Detalles agregados correctamente." });
        }

        [HttpPut("update-multiple")]
        public async Task<IActionResult> UpdateMultiple([FromBody] List<OrderDetail> orderDetails)
        {
            if (orderDetails == null || orderDetails.Count == 0)
                return BadRequest("La lista de detalles está vacía.");

            await _dataService.UpdateMultiple(orderDetails);
            return NoContent();
        }

        [HttpDelete("remove-multiple")]
        public async Task<IActionResult> RemoveMultiple([FromBody] List<OrderDetail> orderDetails)
        {
            if (orderDetails == null || orderDetails.Count == 0)
                return BadRequest(new { message = "La lista de detalles a eliminar está vacía." });

            await _dataService.RemoveMultiple(orderDetails);
            return NoContent();
        }
    }
}