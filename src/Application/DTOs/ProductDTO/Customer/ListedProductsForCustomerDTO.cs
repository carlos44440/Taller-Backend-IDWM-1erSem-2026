namespace TiendaUCN.src.Application.DTOs.ProductDTO.Customer
{
    /// <summary>
    /// DTO que representa un listado paginado de productos para clientes.
    /// </summary>
    public class ListedProductsForCustomerDTO
    {
        /// <summary>
        /// Lista de productos disponibles para el cliente.
        /// </summary>
        public List<ProductForCustomerDTO> Products { get; set; } = new List<ProductForCustomerDTO>();

        /// <summary>
        /// Cantidad total de productos disponibles.
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Cantidad total de páginas disponibles.
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// Página actual del listado.
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// Cantidad de elementos por página.
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Cantidad de productos presentes en la página actual.
        /// </summary>
        public int ProductsInPage { get; set; }
    }
}