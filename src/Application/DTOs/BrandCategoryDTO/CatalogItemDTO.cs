namespace TiendaUCN.src.Application.DTOs.BrandCategoryDTO
{
    /// <summary>
    /// DTO de lectura para ítems de catálogo (categorías y marcas).
    /// </summary>
    public class CatalogItemDTO
    {
        /// <summary>
        /// Identificador del ítem.
        /// </summary>
        public required int Id { get; set; }

        /// <summary>
        /// Nombre del ítem.
        /// </summary>
        public required string Name { get; set; }
    }
}
