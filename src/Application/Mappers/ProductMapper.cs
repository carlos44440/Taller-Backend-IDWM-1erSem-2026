using Mapster;
using TiendaUCN.src.Application.DTOs.ProductDTO;
using TiendaUCN.src.Application.DTOs.ProductDTO.Admin;
using TiendaUCN.src.Application.DTOs.ProductDTO.Customer;
using TiendaUCN.src.Domain.Models;

namespace TiendaUCN.src.Application.Mappers
{
    /// <summary>
    /// Mapper para los productos
    /// </summary>
    public class ProductMapper
    {
        /// <summary>
        /// Interfaz de configuración.
        /// </summary>
        private readonly IConfiguration _configuration;

        /// <summary>
        /// URL de la imagen por defecto.
        /// </summary>
        private readonly string? _defaultImageURL;

        /// <summary>
        /// Número de unidades para considerar que el producto tiene pocas unidades disponibles.
        /// </summary>
        private readonly int _fewUnitsAvailable;

        /// <summary>
        /// Constructor del ProductMapper
        /// </summary>
        /// <param name="configuration">Interfaz de configuración</param>
        /// <exception cref="InvalidOperationException"></exception>
        public ProductMapper(IConfiguration configuration)
        {
            _configuration = configuration;
            _defaultImageURL = _configuration.GetValue<string>("Products:DefaultImageUrl") ?? throw new InvalidOperationException("La URL de la imagen por defecto no puede ser nula.");
            _fewUnitsAvailable = _configuration.GetValue<int?>("Products:FewUnitsAvailable") ?? throw new InvalidOperationException("La configuración 'FewUnitsAvailable' no puede ser nula.");
        }

        /// <summary>
        /// Configura todas las mapeos para los productos.
        /// </summary>
        public void ConfigureAllMappings()
        {
            ConfigureProductMappings();
        }

        /// <summary>
        /// Configura el mapeo de los productos.
        /// </summary>
        private void ConfigureProductMappings()
        {
            TypeAdapterConfig<Product, ProductDetailCustomerDTO>.NewConfig()
                .Map(dest => dest.Price, src => src.Price.ToString("C"))
                .Map(dest => dest.StockIndicator, src => GetStockIndicator(src.Stock))
                .Map(dest => dest.BrandName, src => src.Brand.Name)
                .Map(dest => dest.BrandDescription, src => src.Brand.Description)
                .Map(dest => dest.CategoryName, src => src.Category.Name)
                .Map(dest => dest.CategoryDescription, src => src.Category.Description)
                .Map(dest => dest.ImagesURL, src => src.Images.Count() != 0 ?
                    src.Images.Select(i => i.ImageUrl).ToList() : new List<string> { _defaultImageURL! });

            TypeAdapterConfig<Product, ProductDetailAdminDTO>.NewConfig()
                .Map(dest => dest.Price, src => src.Price.ToString("C"))
                .Map(dest => dest.BrandName, src => src.Brand.Name)
                .Map(dest => dest.BrandDescription, src => src.Brand.Description)
                .Map(dest => dest.CategoryName, src => src.Category.Name)
                .Map(dest => dest.CategoryDescription, src => src.Category.Description)
                .Map(dest => dest.ImagesURL, src => src.Images.Count() != 0 ?
                    src.Images.Select(i => i.ImageUrl).ToList() : new List<string> { _defaultImageURL! });

            TypeAdapterConfig<Product, ProductForCustomerDTO>.NewConfig()
                .Map(dest => dest.MainImageURL, src => src.Images.FirstOrDefault() != null ? src.Images.First().ImageUrl : _defaultImageURL)
                .Map(dest => dest.Price, src => src.Price.ToString("C"))
                .Map(dest => dest.StockIndicator, src => GetStockIndicator(src.Stock));

            TypeAdapterConfig<Product, ProductForAdminDTO>.NewConfig()
                .Map(dest => dest.MainImageURL, src => src.Images.FirstOrDefault() != null ? src.Images.First().ImageUrl : _defaultImageURL)
                .Map(dest => dest.Price, src => src.Price.ToString("C"))
                .Map(dest => dest.Available, src => src.IsActive ? "Activo" : "Inactivo");
        }

        /// <summary>
        /// Obtiene un indicador de stock.
        /// </summary>
        /// <param name="stock">La cantidad de unidades en stock</param>
        /// <returns>El indicador de stock</returns>
        private string GetStockIndicator(int stock)
        {
            if (stock == 0) { return "Producto sin stock"; }
            if (stock <= _fewUnitsAvailable) { return "Pocas unidades disponibles"; }
            return "Con Stock"!;
        }
    }
}