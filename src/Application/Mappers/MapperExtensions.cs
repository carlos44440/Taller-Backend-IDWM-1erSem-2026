using Mapster;
using TiendaUCN.src.Application.Mappers;

namespace Tienda_UCN_api.Src.Application.Mappers
{
    public class MapperExtensions
    {
        public static void ConfigureMapster(IServiceProvider serviceProvider)
        {
            // Configuración global de Mapster para ignorar valores nulos
            TypeAdapterConfig.GlobalSettings.Default.IgnoreNullValues(true);

            // Configuración de mapeos específicos
            var userMapper = serviceProvider.GetRequiredService<UserMapper>();
            userMapper.ConfigureAllMappings();

            var productMapper = serviceProvider.GetRequiredService<ProductMapper>();
            productMapper.ConfigureAllMappings();

            var cartMapper = serviceProvider.GetRequiredService<CartMapper>();
            cartMapper.ConfigureAllMappings();
        }
    }
}