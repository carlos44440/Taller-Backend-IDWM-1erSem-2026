namespace TiendaUCN.src.Application.DTOs.ProductDTO.Admin
{
    /// <summary>
    /// DTO que representa un listado paginado de productos para administración.
    /// </summary>
    public class ListedProductsForAdminDTO
    {
        /// <summary>
        /// Lista de productos disponibles para administración.
        /// </summary>
        public List<ProductForAdminDTO> Products { get; set; } = new List<ProductForAdminDTO>();

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