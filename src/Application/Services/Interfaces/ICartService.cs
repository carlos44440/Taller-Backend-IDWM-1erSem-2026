using TiendaUCN.src.Application.DTOs.CartDTO;

namespace TiendaUCN.src.Application.Services.Interfaces
{
    /// <summary>
    /// Interfaz del servicio de gestión del carrito de compras.
    /// Define las operaciones disponibles para crear, modificar y procesar un carrito.
    /// </summary>
    public interface ICartService
    {
        /// <summary>
        /// Crea un nuevo carrito de compras o retorna uno existente asociado al comprador.
        /// </summary>
        /// <param name="buyerId">Identificador único del comprador (puede ser anónimo o autenticado).</param>
        /// <param name="userId">Identificador del usuario autenticado (opcional).</param>
        /// <returns>Un objeto <see cref="CartDTO"/> con la información del carrito.</returns>
        Task<CartDTO> CreateOrGetCartAsync(string buyerId, int? userId = null);

        /// <summary>
        /// Agrega un producto al carrito de compras o incrementa su cantidad si ya existe.
        /// </summary>
        /// <param name="buyerId">Identificador del comprador.</param>
        /// <param name="addCartItemDTO">DTO que contiene el ID del producto y la cantidad a agregar.</param>
        /// <param name="userId">Identificador del usuario autenticado (opcional).</param>
        /// <returns>El carrito actualizado representado como <see cref="CartDTO"/>.</returns>
        Task<CartDTO> AddCartItemAsync(string buyerId, AddChangeCartItemDTO addCartItemDTO, int? userId = null);

        /// <summary>
        /// Actualiza la cantidad de un producto específico dentro del carrito.
        /// </summary>
        /// <param name="buyerId">Identificador del comprador.</param>
        /// <param name="changeCartItemDTO">DTO que contiene el ID del producto y la nueva cantidad.</param>
        /// <param name="userId">Identificador del usuario autenticado (opcional).</param>
        /// <returns>El carrito actualizado representado como <see cref="CartDTO"/>.</returns>
        Task<CartDTO> UpdateCartItemQuantityAsync(string buyerId, AddChangeCartItemDTO changeCartItemDTO, int? userId = null);

        /// <summary>
        /// Elimina un producto específico del carrito de compras.
        /// </summary>
        /// <param name="buyerId">Identificador del comprador.</param>
        /// <param name="productId">Identificador del producto a eliminar.</param>
        /// <param name="userId">Identificador del usuario autenticado (opcional).</param>
        /// <returns>El carrito actualizado representado como <see cref="CartDTO"/>.</returns>
        Task<CartDTO> RemoveCartItemAsync(string buyerId, int productId, int? userId = null);

        /// <summary>
        /// Elimina todos los productos del carrito de compras.
        /// </summary>
        /// <param name="buyerId">Identificador del comprador.</param>
        /// <param name="userId">Identificador del usuario autenticado (opcional).</param>
        /// <returns>Un carrito vacío representado como <see cref="CartDTO"/>.</returns>
        Task<CartDTO> ClearCartAsync(string buyerId, int? userId = null);

        /// <summary>
        /// Procesa el carrito de compras y realiza el checkout.
        /// </summary>
        /// <param name="userId">Identificador del usuario autenticado que realiza la compra.</param>
        /// <returns>
        /// Un objeto <see cref="CheckoutResultDTO"/> que contiene el resultado del proceso de compra,
        /// incluyendo información sobre éxito, errores o detalles de la transacción.
        /// </returns>
        Task<CheckoutResultDTO> CheckoutCartAsync(int userId);
    }
}