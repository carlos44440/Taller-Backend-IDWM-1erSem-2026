using TiendaUCN.src.Domain.Models;

namespace TiendaUCN.src.Infrastructure.Repositories.Interfaces
{
    /// <summary>
    /// Interfaz del repositorio de carritos de compra.
    /// </summary>
    public interface ICartRepository
    {
        /// <summary>
        /// Obtiene el carrito de compra por el ID del usuario.
        /// </summary>
        /// <param name="userId">ID del usuario</param>
        /// <returns>El carrito de compra si existe, null si no</returns>
        Task<Cart?> GetByUserIdAsync(int userId);

        /// <summary>
        /// Obtiene el carrito de compra por el ID del comprador.
        /// </summary>
        /// <param name="buyerId">ID del comprador</param>
        /// <returns>El carrito de compra si existe, null si no</returns>
        Task<Cart?> GetByBuyerIdAsync(string buyerId);

        /// <summary>
        /// Crea un nuevo carrito de compra en la base de datos.
        /// </summary>
        /// <param name="cart">El carrito de compra a crear</param>
        /// <returns>true si lo crea, false si no</returns>
        Task<bool> CreateAsync(Cart cart);

        /// <summary>
        /// Agrega un nuevo item al carrito de compra.
        /// </summary>
        /// <param name="cartItem">El item a agregar</param>
        /// <returns>true si lo agrega, false si no</returns>
        Task<bool> AddItemAsync(CartItem cartItem);

        /// <summary>
        /// Actualiza la cantidad de un item existente en el carrito de compra.
        /// </summary>
        /// <param name="cartId">El ID del carrito de compra</param>
        /// <param name="cartItemId">El ID del item en el carrito</param>
        /// <param name="newQuantity">La nueva cantidad</param>
        /// <returns></returns>
        Task UpdateItemQuantityAsync(int cartId, int cartItemId, int newQuantity);

        /// <summary>
        /// Actualiza el precio total del carrito de compra.
        /// </summary>
        /// <param name="cartId">El ID del carrito de compra</param>
        /// <param name="newTotalPrice">El nuevo precio total</param>
        /// <returns></returns>
        Task UpdateTotalPriceAsync(int cartId, int newTotalPrice);

        /// <summary>
        /// Elimina un item del carrito de compra.
        /// </summary>
        /// <param name="cartId">El ID del carrito de compra</param>
        /// <param name="cartItemId">El ID del item en el carrito</param>
        /// <returns>true si lo elimina, false si no</returns>
        Task<bool> RemoveItemAsync(int cartId, int cartItemId);

        /// <summary>
        /// Elimina todos los items del carrito de compra.
        /// </summary>
        /// <param name="cartId">El ID del carrito de compra</param>
        /// <returns>true si los elimina, false si no</returns>
        Task<bool> ClearCartItemsAsync(int cartId);
    }
}