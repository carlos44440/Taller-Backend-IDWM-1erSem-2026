using Microsoft.EntityFrameworkCore;
using TiendaUCN.src.Domain.Models;
using TiendaUCN.src.Infrastructure.Data;
using TiendaUCN.src.Infrastructure.Repositories.Interfaces;

namespace TiendaUCN.src.Infrastructure.Repositories.Implements
{
    /// <summary>
    /// Repositorio del carrito de compra.
    /// </summary>
    public class CartRepository : ICartRepository
    {
        /// <summary>
        /// Contexto de la base de datos.
        /// </summary>
        private readonly DataContext _context;

        /// <summary>
        /// Constructor del repositorio del carrito de compra.
        /// </summary>
        /// <param name="context">El contexto de la base de datos</param>
        public CartRepository(DataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtiene el carrito de compra por el ID del usuario.
        /// </summary>
        /// <param name="userId">ID del usuario</param>
        /// <returns>El carrito de compra si existe, null si no</returns>
        public async Task<Cart?> GetByUserIdAsync(int userId)
        {
            return await _context.Carts
                .AsNoTracking() // Solo lectura, no guarda el cart en cache
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Product)
                        .ThenInclude(p => p.Images)
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        /// <summary>
        /// Obtiene el carrito de compra por el ID del comprador.
        /// </summary>
        /// <param name="buyerId">ID del comprador</param>
        /// <returns>El carrito de compra si existe, null si no</returns>
        public async Task<Cart?> GetByBuyerIdAsync(string buyerId)
        {
            // Retornar el carrito exclusivo para el navegador
            return await _context.Carts
                .AsNoTracking() // Solo lectura, no guarda el cart en cache
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Product)
                        .ThenInclude(p => p.Images)
                .FirstOrDefaultAsync(c => c.BuyerId == buyerId && c.UserId == null);
        }

        /// <summary>
        /// Crea un nuevo carrito de compra en la base de datos.
        /// </summary>
        /// <param name="cart">El carrito de compra a crear</param>
        /// <returns>true si lo crea, false si no</returns>
        public async Task<bool> CreateAsync(Cart cart)
        {
            _context.Carts.Add(cart);
            return await _context.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// Agrega un nuevo item al carrito de compra.
        /// </summary>
        /// <param name="cartItem">El item a agregar</param>
        /// <returns>true si lo agrega, false si no</returns>
        public async Task<bool> AddItemAsync(CartItem cartItem)
        {
            _context.CartItems.Add(cartItem);
            return await _context.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// Actualiza la cantidad de un item existente en el carrito de compra.
        /// </summary>
        /// <param name="cartId">El ID del carrito de compra</param>
        /// <param name="cartItemId">El ID del item en el carrito</param>
        /// <param name="newQuantity">La nueva cantidad</param>
        /// <returns></returns>
        public async Task UpdateItemQuantityAsync(int cartId, int cartItemId, int newQuantity)
        {
            await _context.CartItems
                .Where(ci => ci.Id == cartItemId && ci.CartId == cartId)
                .ExecuteUpdateAsync(ci =>
                    ci.SetProperty(c => c.Quantity, newQuantity));
        }

        /// <summary>
        /// Actualiza el precio total del carrito de compra.
        /// </summary>
        /// <param name="cartId">El ID del carrito de compra</param>
        /// <param name="newTotalPrice">El nuevo precio total</param>
        /// <returns></returns>
        public async Task UpdateTotalPriceAsync(int cartId, int newTotalPrice)
        {
            await _context.Carts
                .Where(c => c.Id == cartId)
                .ExecuteUpdateAsync(c =>
                    c.SetProperty(c => c.TotalPrice, newTotalPrice));
        }

        /// <summary>
        /// Elimina un item del carrito de compra.
        /// </summary>
        /// <param name="cartId">El ID del carrito de compra</param>
        /// <param name="cartItemId">El ID del item en el carrito</param>
        /// <returns>true si lo elimina, false si no</returns>
        public async Task<bool> RemoveItemAsync(int cartId, int cartItemId)
        {
            return await _context.CartItems
                .Where(ci => ci.Id == cartItemId && ci.CartId == cartId)
                .ExecuteDeleteAsync() > 0;
        }

        /// <summary>
        /// Elimina todos los items del carrito de compra.
        /// </summary>
        /// <param name="cartId">El ID del carrito de compra</param>
        /// <returns>true si los elimina, false si no</returns>
        public async Task<bool> ClearCartItemsAsync(int cartId)
        {
            return await _context.CartItems
                .Where(ci => ci.CartId == cartId)
                .ExecuteDeleteAsync() > 0;
        }
    }
}