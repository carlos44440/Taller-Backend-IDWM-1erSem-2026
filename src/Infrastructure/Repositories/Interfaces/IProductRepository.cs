using CloudinaryDotNet;
using TiendaUCN.src.Application.DTOs.ProductDTO;
using TiendaUCN.src.Application.DTOs.ProductDTO.Admin;
using TiendaUCN.src.Domain.Models;

namespace TiendaUCN.src.Infrastructure.Repositories.Interfaces
{
    /// <summary>
    /// Interfaz para el repositorio de productos.
    /// </summary>
    public interface IProductRepository
    {
        /// <summary>
        /// Verifica si un producto existe por su nombre y el nombre de su marca.
        /// </summary>
        /// <param name="name">El nombre del producto</param>
        /// <param name="brandName">El nombre de la marca</param>
        /// <returns>true si existe, false si no</returns>
        Task<bool> ExistsByNameAndBrandAsync(string name, string brandName);

        /// <summary>
        /// Crea un nuevo producto en la base de datos.
        /// </summary>
        /// <param name="product">El producto a crear</param>
        /// <returns>true si lo crea, false si no</returns>
        Task<bool> CreateAsync(Product product);

        /// <summary>
        /// Verifica si un producto existe por su ID.
        /// </summary>
        /// <param name="id">El ID del producto</param>
        /// <returns>true si existe, false si no</returns>
        Task<bool> ExistsByIdAsync(int id);

        /// <summary>
        /// Cambia el estado de un producto (activo/inactivo).
        /// </summary>
        /// <param name="id">El ID del producto</param>
        /// <returns>true si lo cambia, false si no</returns>
        Task<bool> SwitchStatusAsync(int id);

        /// <summary>
        /// Obtiene el estado de un producto por su ID.
        /// </summary>
        /// <param name="id">El ID del producto</param>
        /// <returns>El estado del producto o null si no se encuentra</returns>
        Task<string?> GetStatusAsync(int id);

        /// <summary>
        /// Verifica si un producto existe por su ID y si está activo (para clientes).
        /// </summary>
        /// <param name="id">El ID del producto</param>
        /// <returns>true si existe y está activo, false si no</returns>
        Task<bool> ExistsByIdCustomerAsync(int id);

        /// <summary>
        /// Obtiene un producto por su ID para clientes (solo si está activo).
        /// </summary>
        /// <param name="id">El ID del producto</param>
        /// <returns>El producto o null si no se encuentra o no está activo</returns>
        Task<Product?> GetProductByIdForCustomerAsync(int id);

        /// <summary>
        /// Obtiene un producto por su ID para administradores (sin importar su estado).
        /// </summary>
        /// <param name="id">El ID del producto</param>
        /// <returns>El producto o null si no se encuentra</returns>
        Task<Product?> GetProductByIdForAdminAsync(int id);

        /// <summary>
        /// Elimina un producto por su ID.
        /// </summary>
        /// <param name="id">El ID del producto</param>
        /// <returns>true si lo elimina, false si no</returns>
        Task<bool> DeleteAsync(int id);

        /// <summary>
        /// Obtiene una lista de productos filtrados y paginados para administradores.
        /// </summary>
        /// <param name="searchParams">Los parámetros de búsqueda</param>
        /// <returns>Una tupla con la lista de productos y el total de registros</returns>
        Task<(IEnumerable<Product> products, int totalCount)> GetFilteredForAdminAsync(SearchParamsDTO searchParams);

        /// <summary>
        /// Obtiene una lista de productos filtrados y paginados para clientes (solo activos).
        /// </summary>
        /// <param name="searchParams">Los parámetros de búsqueda</param>
        /// <returns>Una tupla con la lista de productos y el total de registros</returns>
        Task<(IEnumerable<Product> products, int totalCount)> GetFilteredForCustomerAsync(SearchParamsDTO searchParams);

        /// <summary>
        /// Actualiza un producto existente.
        /// </summary>
        /// <param name="product">El producto a actualizar</param>
        /// <returns>true si lo actualiza, false si no</returns>
        Task<bool> UpdateAsync(Product product);

        /// <summary>
        /// Actualiza el stock de un producto por su ID.
        /// </summary>
        /// <param name="productId">El ID del producto</param>
        /// <param name="newStock">El nuevo stock</param>
        /// <returns>true si lo actualiza, false si no</returns>
        Task<bool> UpdateStockAsync(int productId, int newStock);
    }
}