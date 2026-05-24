namespace TiendaUCN.src.Application.DTOs.ProductDTO
{
    /// <summary>
    /// DTO que representa el detalle de un producto para clientes.
    /// </summary>
    public class ProductDetailCustomerDTO
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
        /// <remarks>
        /// Puede ser nula si no se ha definido una descripción.
        /// </remarks>
        public string? Description { get; set; }

        /// <summary>
        /// Precio del producto.
        /// </summary>
        public required int Price { get; set; }

        /// <summary>
        /// Indica si el producto tiene stock disponible.
        /// </summary>
        public required bool InStock { get; set; }

        /// <summary>
        /// Nombre de la marca del producto.
        /// </summary>
        public required string BrandName { get; set; }

        /// <summary>
        /// Descripción de la marca del producto.
        /// </summary>
        public required string BrandDescription { get; set; }

        /// <summary>
        /// Nombre de la categoría del producto.
        /// </summary>
        public required string CategoryName { get; set; }

        /// <summary>
        /// Descripción de la categoría del producto.
        /// </summary>
        public required string CategoryDescription { get; set; }

        /// <summary>
        /// Lista de URLs de las imágenes del producto.
        /// </summary>
        public List<string> ImagesURL { get; set; } = new List<string>();
    }
}