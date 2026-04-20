using Mapster;
using TiendaUCN.src.Domain.Models;

namespace TiendaUCN.src.Application.Mappers
{
    public class OrderMapper
    {
        private readonly IConfiguration _configuration;
        private readonly string _defaultImageUrl;
        private readonly TimeZoneInfo _timeZoneInfo;
        public OrderMapper(IConfiguration configuration)
        {
            _configuration = configuration;
            _defaultImageUrl = _configuration["Products:DefaultImageUrl"] ?? throw new InvalidOperationException("La URL de la imagen por defecto no puede ser nula.");
            // Configurar la zona horaria local
            _timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(TimeZoneInfo.Local.Id);
        }

        public void ConfigureAllMappings()
        {
            ConfigureOrderItemsMappings();
            ConfigureOrderMappings();
        }

        private void ConfigureOrderMappings()
        {
            TypeAdapterConfig<Cart, Order>.NewConfig()
                .Map(dest => dest.OrderItems, src => src.CartItems.Adapt<List<OrderItem>>())
                .Ignore(dest => dest.Id);
        }

        private void ConfigureOrderItemsMappings()
        {
            TypeAdapterConfig<CartItem, OrderItem>.NewConfig()
                .Map(dest => dest.NameAtMoment, src => src.Product.Name)
                .Map(dest => dest.DescriptionAtMoment, src => src.Product.Description)
                .Map(dest => dest.UnitPriceAtMoment, src => src.Product.Price)
                .Map(dest => dest.ImageUrlAtMoment, src => src.Product.Images != null && src.Product.Images.Any() ? src.Product.Images.First().ImageUrl : _defaultImageUrl)
                .Map(dest => dest.SubtotalPrice, src => src.Quantity * src.Product.Price)
                .Ignore(dest => dest.Id);
        }
    }
}