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
        /// Constructor del ProductMapper
        /// </summary>
        /// <param name="configuration">Interfaz de configuración</param>
        /// <exception cref="InvalidOperationException"></exception>
        public ProductMapper(IConfiguration configuration)
        {
            _configuration = configuration;
            _defaultImageURL = _configuration.GetValue<string>("Products:DefaultImageUrl") ?? throw new InvalidOperationException("La URL de la imagen por defecto no puede ser nula.");
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
                .Map(dest => dest.InStock, src => src.Stock > 0)
                .Map(dest => dest.BrandName, src => src.Brand.Name)
                .Map(dest => dest.BrandDescription, src => src.Brand.Description)
                .Map(dest => dest.CategoryName, src => src.Category.Name)
                .Map(dest => dest.CategoryDescription, src => src.Category.Description)
                .Map(dest => dest.ImagesURL, src => src.Images.Count() != 0 ?
                    src.Images.Select(i => i.ImageUrl).ToList() : new List<string> { _defaultImageURL! });

            TypeAdapterConfig<Product, ProductDetailAdminDTO>.NewConfig()
                .Map(dest => dest.BrandName, src => src.Brand.Name)
                .Map(dest => dest.BrandDescription, src => src.Brand.Description)
                .Map(dest => dest.CategoryName, src => src.Category.Name)
                .Map(dest => dest.CategoryDescription, src => src.Category.Description)
                .Map(dest => dest.ImagesURL, src => src.Images.Count() != 0 ?
                    src.Images.Select(i => i.ImageUrl).ToList() : new List<string> { _defaultImageURL! });

            TypeAdapterConfig<Product, ProductForCustomerDTO>.NewConfig()
                .Map(dest => dest.MainImageURL, src => src.Images.FirstOrDefault() != null ? src.Images.First().ImageUrl : _defaultImageURL)
                .Map(dest => dest.InStock, src => src.Stock > 0);

            TypeAdapterConfig<Product, ProductForAdminDTO>.NewConfig()
                .Map(dest => dest.MainImageURL, src => src.Images.FirstOrDefault() != null ? src.Images.First().ImageUrl : _defaultImageURL)
                .Map(dest => dest.Available, src => src.IsActive ? "Activo" : "Inactivo");
        }
    }
}