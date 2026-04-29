using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Tienda_UCN_api.src.Application.DTO;
using TiendaUCN.src.Application.DTOs.CartDTO;
using TiendaUCN.src.Application.Services.Interfaces;
using TiendaUCN.src.Domain.Models;

namespace TiendaUCN.src.API.Controllers
{
    /// <summary>
    /// Controlador de carrito de compras.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        /// <summary>
        /// Interfaz del servicio de carrito de compras.
        /// </summary>
        private readonly ICartService _cartService;

        /// <summary>
        /// Constructor del controlador de carrito de compras.
        /// </summary>
        /// <param name="cartService">Interfaz del servicio de carrito de compras</param>
        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        /// <summary>
        /// Endpoint para obtener el carrito de compras del usuario actual o del comprador anónimo.
        /// </summary>
        /// <returns>DTO del carrito de compras</returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetCart()
        {
            var buyerId = GetBuyerId();
            var userId = GetUserId();
            var cart = await _cartService.CreateOrGetCartAsync(buyerId, userId);
            return Ok(new GenericResponse<CartDTO>("Carrito obtenido exitosamente", cart));
        }

        /// <summary>
        /// Endpoint para agregar un item al carrito de compras.
        /// </summary>
        /// <param name="addCartItemDTO">DTO para la adición de item al carrito</param>
        /// <returns>DTO del carrito de compras actualizado</returns>
        [HttpPost("items")]
        [AllowAnonymous]
        public async Task<IActionResult> AddCartItem([FromBody] AddChangeCartItemDTO addCartItemDTO)
        {
            var buyerId = GetBuyerId();
            var userId = GetUserId();
            var cart = await _cartService.AddCartItemAsync(buyerId, addCartItemDTO, userId);
            return Ok(new GenericResponse<CartDTO>("Item agregado al carrito exitosamente", cart));
        }

        /// <summary>
        /// Endpoint para actualizar la cantidad de un item en el carrito de compras.
        /// </summary>
        /// <param name="changeCartItemDTO">DTO para la actualización de cantidad de item</param>
        /// <returns>DTO del carrito de compras actualizado</returns>
        [HttpPatch("items")]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateCartItemQuantity([FromBody] AddChangeCartItemDTO changeCartItemDTO)
        {
            var buyerId = GetBuyerId();
            var userId = GetUserId();
            var cart = await _cartService.UpdateCartItemQuantityAsync(buyerId, changeCartItemDTO, userId);
            return Ok(new GenericResponse<CartDTO>("Cantidad del item actualizada exitosamente", cart));
        }

        /// <summary>
        /// Endpoint para remover un item del carrito de compras.
        /// </summary>
        /// <param name="productId">ID del producto a remover</param>
        /// <returns>DTO del carrito de compras actualizado</returns>
        [HttpDelete("items/{productId}")]
        [AllowAnonymous]
        public async Task<IActionResult> RemoveCartItem([FromRoute] int productId)
        {
            var buyerId = GetBuyerId();
            var userId = GetUserId();
            var cart = await _cartService.RemoveCartItemAsync(buyerId, productId, userId);
            return Ok(new GenericResponse<CartDTO>("Item removido del carrito exitosamente", cart));
        }

        /// <summary>
        /// Endpoint para limpiar el carrito de compras.
        /// </summary>
        /// <returns>DTO del carrito de compras vacío</returns>
        [HttpPut("clear")]
        [AllowAnonymous]
        public async Task<IActionResult> ClearCart()
        {
            var buyerId = GetBuyerId();
            var userId = GetUserId();
            var cart = await _cartService.ClearCartAsync(buyerId, userId);
            return Ok(new GenericResponse<CartDTO>("Carrito limpiado exitosamente", cart));
        }

        /// <summary>
        /// Endpoint para realizar el checkout del carrito de compras.
        /// </summary>
        /// <returns>DTO del carrito de compras</returns>
        [HttpPost("checkout")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> CheckoutCart()
        {
            var userId = GetUserId();
            var result = await _cartService.CheckoutCartAsync(userId!.Value);
            return Ok(new GenericResponse<CheckoutResultDTO>("Checkout realizado exitosamente", result));
        }

        /// <summary>
        /// Método auxiliar para obtener el buyerId
        /// </summary>
        /// <returns>El ID del comprador</returns>
        /// <exception cref="Exception"></exception>
        private string GetBuyerId()
        {
            // Intentar obtener el buyerId del contexto HTTP
            var buyerId = HttpContext.Items["BuyerId"]?.ToString();

            if (string.IsNullOrEmpty(buyerId))
            {
                throw new Exception("No se encontró el id del comprador.");
            }
            return buyerId;
        }

        /// <summary>
        /// Método auxiliar para obtener el userId
        /// </summary>
        /// <returns>El ID del usuario</returns>
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