namespace TiendaUCN.src.Application.DTOs.ProductDTO.Customer
{
    /// <summary>
    /// DTO que representa la información básica de un producto para clientes.
    /// </summary>
    public class ProductForCustomerDTO
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
        /// Descripción del producto.
        /// </summary>
        public required string Description { get; set; }

        /// <summary>
        /// URL de la imagen principal del producto.
        /// </summary>
        public required string MainImageURL { get; set; }

        /// <summary>
        /// Precio del producto.
        /// </summary>
        public required string Price { get; set; }

        /// <summary>
        /// Indicador de disponibilidad de stock del producto.
        /// </summary>
        /// <remarks>
        /// Representa el estado del stock en un formato entendible para el cliente.
        /// </remarks>
        public required string StockIndicator { get; set; }
    }
}