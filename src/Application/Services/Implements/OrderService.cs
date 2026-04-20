using Mapster;
using Serilog;
using TiendaUCN.src.Application.Services.Interfaces;
using TiendaUCN.src.Domain.Models;
using TiendaUCN.src.Infrastructure.Repositories.Interfaces;

namespace TiendaUCN.src.Application.Services.Implements
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;
        public OrderService(IOrderRepository orderRepository, ICartRepository cartRepository, IProductRepository productRepository)
        {
            _orderRepository = orderRepository;
            _cartRepository = cartRepository;
            _productRepository = productRepository;
        }

        public async Task<string> CreateOrderAsync(int userId)
        {
            // Obtener el carrito del usuario
            var cart = await _cartRepository.GetByUserIdAsync(userId)
                ?? throw new InvalidOperationException("No se encontró un carrito para el usuario.");

            // Validar que el carrito no esté vacío
            if (cart.CartItems.Count == 0)
            {
                Log.Information("El usuario {UserId} intentó crear una orden con un carrito vacío.", userId);
                throw new InvalidOperationException("No se puede crear una orden con un carrito vacío.");
            }

            // Generar un código único
            string code = await GenerateOrderCodeAsync();

            // Mapear el carrito a una orden
            Order order = cart.Adapt<Order>();
            order.Code = code;
            order.UserId = userId;

            // Guardar la orden en la base de datos
            var isCreated = await _orderRepository.CreateAsync(order);
            if (!isCreated)
            {
                Log.Error("Error al crear la orden para el usuario {UserId}.", userId);
                throw new Exception("No se pudo crear la orden. Por favor, intente nuevamente.");
            }

            // Actualizar el stock de los productos
            foreach (var item in cart.CartItems)
            {
                var newStock = item.Product.Stock - item.Quantity;
                await _productRepository.UpdateStockAsync(item.ProductId, newStock);
            }

            // Eliminar todos los items del carrito
            var isCleared = await _cartRepository.ClearCartItemsAsync(cart.Id);
            if (!isCleared)
            {
                Log.Error("Error al limpiar el carrito para el userId: {UserId}", userId);
                throw new Exception($"Error al limpiar el carrito para el userId: {userId}");
            }
            Log.Information("Carrito limpiado. CartId: {CartId}", cart.Id);

            // Actualizar el precio total del carrito a 0
            var newTotalPrice = 0;
            await _cartRepository.UpdateTotalPriceAsync(cart.Id, newTotalPrice);

            // Retornar el código de la orden creada
            return code;
        }

        private async Task<string> GenerateOrderCodeAsync()
        {
            string code;
            do
            {
                // Generar un código único para la orden
                var timestamp = DateTime.UtcNow.ToString("yyMMddHHmmss");
                var random = Random.Shared.Next(100, 999);
                code = $"ORD-{timestamp}-{random}";
            }
            // Verificar que el código generado no exista en la base de datos
            while (await _orderRepository.ExistsByCodeAsync(code));

            return code;
        }
    }
}