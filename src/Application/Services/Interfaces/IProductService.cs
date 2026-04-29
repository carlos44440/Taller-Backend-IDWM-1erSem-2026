using TiendaUCN.src.Application.DTOs.ProductDTO;
using TiendaUCN.src.Application.DTOs.ProductDTO.Admin;
using TiendaUCN.src.Application.DTOs.ProductDTO.Customer;

namespace TiendaUCN.src.Application.Services.Interfaces
{
    /// <summary>
    /// Interfaz del servicio de productos.
    /// </summary>
    public interface IProductService
    {
        /// <summary>
        /// Crea un nuevo producto.
        /// </summary>
        /// <param name="createProductDTO">Datos del producto a crear.</param>
        /// <returns>Mensaje o identificador del producto creado.</returns>
        Task<string> CreateProductAsync(CreateProductDTO createProductDTO);

        /// <summary>
        /// Cambia el estado de un producto.
        /// </summary>
        /// <param name="id">Identificador del producto.</param>
        /// <returns>Mensaje de resultado.</returns>
        Task<string> SwitchStatusProductAsync(int id);

        /// <summary>
        /// Obtiene un producto por su ID para cliente.
        /// </summary>
        /// <param name="id">Identificador del producto.</param>
        /// <returns>Detalle del producto para cliente.</returns>
        Task<ProductDetailCustomerDTO> GetProductByIdForCustomerAsync(int id);

        /// <summary>
        /// Obtiene un producto por su ID para administrador.
        /// </summary>
        /// <param name="id">Identificador del producto.</param>
        /// <returns>Detalle del producto para administrador.</returns>
        Task<ProductDetailAdminDTO> GetProductByIdForAdminAsync(int id);

        /// <summary>
        /// Elimina un producto.
        /// </summary>
        /// <param name="id">Identificador del producto.</param>
        Task DeleteProductAsync(int id);

        /// <summary>
        /// Obtiene el listado de productos para administrador.
        /// </summary>
        /// <param name="searchParams">Parámetros de búsqueda y paginación.</param>
        /// <returns>Listado de productos para administrador.</returns>
        Task<ListedProductsForAdminDTO> GetListedProductsForAdminAsync(SearchParamsDTO searchParams);

        /// <summary>
        /// Obtiene el listado de productos para cliente.
        /// </summary>
        /// <param name="searchParams">Parámetros de búsqueda y paginación.</param>
        /// <returns>Listado de productos para cliente.</returns>
        Task<ListedProductsForCustomerDTO> GetListedProductsForCustomerAsync(SearchParamsDTO searchParams);

        /// <summary>
        /// Actualiza un producto.
        /// </summary>
        /// <param name="id">Identificador del producto.</param>
        /// <param name="updateProductDTO">Datos a actualizar del producto.</param>
        Task UpdateProductAsync(int id, UpdateProductDTO updateProductDTO);
    }
}