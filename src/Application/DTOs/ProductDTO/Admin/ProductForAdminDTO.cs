namespace TiendaUCN.src.Application.DTOs.ProductDTO.Admin
{
    /// <summary>
    /// DTO que representa la información básica de un producto para administración.
    /// </summary>
    public class ProductForAdminDTO
    {
        /// <summary>
        /// Identificador único del producto.
        /// </summary>
        public required int Id { get; set; }

        /// <summary>
        /// Nombre del producto.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// URL de la imagen principal del producto.
        /// </summary>
        /// <remarks>
        /// Puede ser nula si el producto no tiene imagen asignada.
        /// </remarks>
        public string? MainImageURL { get; set; }

        /// <summary>
        /// Precio del producto.
        /// </summary>
        public required int Price { get; set; }

        /// <summary>
        /// Stock disponible del producto.
        /// </summary>
        public required int Stock { get; set; }

        /// <summary>
        /// Estado de disponibilidad del producto.
        /// </summary>
        /// <remarks>
        /// Indica si el producto está disponible o no para su venta.
        /// </remarks>
        public required string Available { get; set; }
    }
}