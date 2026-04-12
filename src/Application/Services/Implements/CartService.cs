using Mapster;
using Serilog;
using TiendaUCN.src.Application.DTOs.CartDTO;
using TiendaUCN.src.Application.Services.Interfaces;
using TiendaUCN.src.Domain.Models;
using TiendaUCN.src.Infrastructure.Repositories.Interfaces;

namespace TiendaUCN.src.Application.Services.Implements
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUserRepository _userRepository;
        public CartService(ICartRepository cartRepository, IProductRepository productRepository, IUserRepository userRepository)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
            _userRepository = userRepository;
        }

        public async Task<CartDTO> CreateOrGetCartAsync(string buyerId, int? userId = null)
        {
            // Inicializar el carrito como null
            Cart? cart = null;

            // En caso de que el usuario este autenticado
            if (userId.HasValue)
            {
                // Buscar el carrito del usuario autenticado
                cart = await _cartRepository.GetByUserIdAsync(userId.Value);

                // Si no se encuentra el carrito
                if (cart == null)
                {
                    Log.Information("No se encontró un carrito para userId: {UserId}", userId.Value);

                    // Buscar un carrito asociado al buyerId
                    cart = await _cartRepository.GetByBuyerIdAsync(buyerId);

                    if (cart == null)
                    {
                        // Crear un nuevo carrito vacio asociado al userId
                        var cartId = await CreateEmptyCartAsync(buyerId, userId);
                    }
                    else
                    {
                        // Crear un nuevo carrito para el usuario en base al carrito encontrado por buyerId
                        var newCart = new Cart
                        {
                            BuyerId = cart.BuyerId,
                            UserId = userId,
                            TotalPrice = cart.TotalPrice,
                            CartItems = cart.CartItems.Select(ci => new CartItem
                            {
                                ProductId = ci.ProductId,
                                Quantity = ci.Quantity,
                            }).ToList()
                        };

                        var isCreated = await _cartRepository.CreateAsync(newCart);
                        if (!isCreated)
                        {
                            Log.Error("Error al crear un nuevo carrito para userId: {UserId} basado en el carrito encontrado por buyerId: {BuyerId}.", userId.Value, buyerId);
                            throw new Exception($"Error al crear un nuevo carrito para userId: {userId.Value} basado en el carrito encontrado por buyerId: {buyerId}.");
                        }
                    }

                    // Obtener el carrito recién creado
                    cart = await _cartRepository.GetByUserIdAsync(userId.Value);
                }

                // Mapear el carrito a CartDTO
                return cart!.Adapt<CartDTO>();
            }

            // En caso de que el usuario no este autenticado
            cart = await _cartRepository.GetByBuyerIdAsync(buyerId);

            // Si no existe, crear uno nuevo
            if (cart == null)
            {
                Log.Information("No se encontró un carrito para buyerId: {BuyerId} y userId: {UserId}.", buyerId, userId);
                var cartId = await CreateEmptyCartAsync(buyerId, userId);

                // Obtener el carrito recién creado
                cart = await _cartRepository.GetByBuyerIdAsync(buyerId);
            }

            // Mapear el carrito a CartDTO
            return cart!.Adapt<CartDTO>();
        }

        private async Task<int> CreateEmptyCartAsync(string buyerId, int? userId)
        {
            // Crear un nuevo carrito vacío
            var newCart = new Cart
            {
                BuyerId = buyerId,
                UserId = userId,
            };

            // Guardar el nuevo carrito en la base de datos
            var isCreated = await _cartRepository.CreateAsync(newCart);
            if (!isCreated)
            {
                Log.Error("Error al crear un nuevo carrito para buyerId: {BuyerId} y userId: {UserId}.", buyerId, userId);
                throw new Exception($"Error al crear un nuevo carrito para buyerId: {buyerId} y userId: {userId}.");
            }

            Log.Information("Nuevo carrito creado para buyerId: {BuyerId}, userId: {UserId}", buyerId, userId);
            return newCart.Id;
        }

        public async Task<CartDTO> AddCartItemAsync(string buyerId, AddChangeCartItemDTO addCartItemDTO, int? userId = null)
        {
            // Inicializar el carrito como null
            Cart? cart = null;

            // Obtener el carrito
            if (userId.HasValue)
            {
                cart = await _cartRepository.GetByUserIdAsync(userId.Value);
            }
            else
            {
                cart = await _cartRepository.GetByBuyerIdAsync(buyerId);
            }

            // Validar que el carrito exista
            if (cart == null)
            {
                Log.Error("Carrito no encontrado para buyerId: {BuyerId} y userId: {UserId}", buyerId, userId);
                throw new Exception($"Carrito no encontrado para buyerId: {buyerId} y userId: {userId}");
            }

            // Obtener el producto
            var product = await _productRepository.GetProductByIdForCustomerAsync(addCartItemDTO.ProductId);

            // Validar que el producto exista
            if (product == null)
            {
                Log.Error("Producto no encontrado para ID: {ProductId}", addCartItemDTO.ProductId);
                throw new Exception($"Producto no encontrado para ID: {addCartItemDTO.ProductId}");
            }

            // Validar que la cantidad no exceda el stock disponible
            if (product.Stock < addCartItemDTO.Quantity)
            {
                Log.Error("Stock insuficiente para el producto ID: {ProductId}. Stock disponible: {Stock}, cantidad solicitada: {Quantity}", addCartItemDTO.ProductId, product.Stock, addCartItemDTO.Quantity);
                throw new Exception($"Stock insuficiente para el producto ID: {addCartItemDTO.ProductId}. Stock disponible: {product.Stock}, cantidad solicitada: {addCartItemDTO.Quantity}");
            }

            // Verificar si el producto ya está en el carrito
            var existingCartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == addCartItemDTO.ProductId);
            if (existingCartItem != null)
            {
                // Si el producto ya está en el carrito, sumar la cantidad solicitada a la cantidad existente
                var newQuantity = existingCartItem.Quantity + addCartItemDTO.Quantity;
                await _cartRepository.UpdateItemQuantityAsync(cart.Id, existingCartItem.Id, newQuantity);
                Log.Information("Cantidad del producto ID: {ProductId} actualizada en el carrito. Nueva cantidad: {Quantity}", addCartItemDTO.ProductId, newQuantity);
            }
            else
            {
                // Si el producto no está en el carrito, agregarlo como un nuevo item
                var newCartItem = new CartItem
                {
                    ProductId = addCartItemDTO.ProductId,
                    Quantity = addCartItemDTO.Quantity,
                    CartId = cart.Id
                };

                var isAdded = await _cartRepository.AddItemAsync(cart, newCartItem);
                if (!isAdded)
                {
                    Log.Error("Error al agregar el producto ID: {ProductId} al carrito para buyerId: {BuyerId} y userId: {UserId}", addCartItemDTO.ProductId, buyerId, userId);
                    throw new Exception($"Error al agregar el producto ID: {addCartItemDTO.ProductId} al carrito para buyerId: {buyerId} y userId: {userId}");
                }
            }

            // Actualizar el precio total del carrito
            var newTotalPrice = cart.TotalPrice + (product.Price * addCartItemDTO.Quantity);
            await _cartRepository.UpdateTotalPriceAsync(cart.Id, newTotalPrice);
            Log.Information("Precio total del carrito actualizado. CartId: {CartId}", cart.Id);

            // Mapear el carrito a CartDTO
            return cart.Adapt<CartDTO>();
        }
    }
}