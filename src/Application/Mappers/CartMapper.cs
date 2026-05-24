using Mapster;
using TiendaUCN.src.Application.DTOs.CartDTO;
using TiendaUCN.src.Domain.Models;

namespace TiendaUCN.src.Application.Mappers
{
    /// <summary>
    /// Mapper para el carrito de compras
    /// </summary>
    public class CartMapper
    {
        /// <summary>
        /// Interfaz de configuracion.
        /// </summary>
        private readonly IConfiguration _configuration;

        /// <summary>
        /// URL de la imagen por defecto.
        /// </summary>
        private readonly string? _defaultImageURL;

        /// <summary>
        /// Constructor del CartMapper
        /// </summary>
        /// <param name="configuration">Interfaz de configuracion.</param>
        /// <exception cref="InvalidOperationException"></exception>
        public CartMapper(IConfiguration configuration)
        {
            _configuration = configuration;
            _defaultImageURL = _configuration.GetValue<string>("Products:DefaultImageUrl") ?? throw new InvalidOperationException("La URL de la imagen por defecto no puede ser nula.");
        }

        /// <summary>
        /// Configura todas las mapeos para el carrito de compras.
        /// </summary>
        public void ConfigureAllMappings()
        {
            ConfigureCartMappings();
            ConfigureCartItemMapppings();
        }

        /// <summary>
        /// Configura el mapeo del carrito de compras.
        /// </summary>
        private void ConfigureCartMappings()
        {
            TypeAdapterConfig<Cart, CartDTO>.NewConfig()
                .Map(dest => dest.Items, src => src.CartItems.Select(i => i.Adapt<CartItemDTO>()).ToList());
        }

        /// <summary>
        /// Configura el mapeo de los items del carrito de compras.
        /// </summary>
        private void ConfigureCartItemMapppings()
        {
            TypeAdapterConfig<CartItem, CartItemDTO>.NewConfig()
                .Map(dest => dest.ProductName, src => src.Product.Name)
                .Map(dest => dest.ProductImageUrl, src => src.Product.Images.FirstOrDefault() != null ? src.Product.Images.First().ImageUrl : _defaultImageURL)
                .Map(dest => dest.ProductPrice, src => src.Product.Price)
                .Map(dest => dest.TotalPrice, src => src.Quantity * src.Product.Price);
        }
    }
}