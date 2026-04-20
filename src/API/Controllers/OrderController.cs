using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Tienda_UCN_api.src.Application.DTO;
using TiendaUCN.src.Application.DTOs.OrderDTO;
using TiendaUCN.src.Application.DTOs.ProductDTO;
using TiendaUCN.src.Application.Services.Interfaces;

namespace TiendaUCN.src.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> CreateOrder()
        {
            var userId = GetUserId();
            var result = await _orderService.CreateOrderAsync(userId!.Value);
            return Created($"api/order/{result}", new GenericResponse<string>("Orden creada exitosamente", result));
        }

        [HttpGet("{orderCode}")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetOrderDetail([FromRoute] string orderCode)
        {
            var userId = GetUserId();
            var result = await _orderService.GetOrderDetailAsync(orderCode, userId!.Value);
            return Ok(new GenericResponse<OrderDetailDTO>("Detalles de la orden", result));
        }

        [HttpGet("user-orders")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetUserOrders([FromQuery] SearchParamsDTO searchParams)
        {
            var userId = GetUserId();
            var result = _orderService.GetOrdersByUserIdAsync(searchParams, userId!.Value);
            return Ok(new GenericResponse<ListedOrderDetailDTO>("Órdenes del usuario obtenidas exitosamente", await result));
        }
        private int? GetUserId()
        {
            // Intentar obtener el userId del token JWT
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Si no se encuentra el claim o no se puede convertir a int, retornar null
            if (int.TryParse(userIdClaim, out int userId))
                return userId;

            return null;
        }
    }
}