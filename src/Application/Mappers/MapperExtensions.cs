using Mapster;
using TiendaUCN.src.Application.Mappers;

namespace Tienda_UCN_api.Src.Application.Mappers
{
    /// <summary>
    /// Clase de configuración de mapeos para Mapster
    /// </summary>
    public class MapperExtensions
    {
        /// <summary>
        /// Configura los mapeos de Mapster para toda la aplicación
        /// </summary>
        /// <param name="serviceProvider">Proveedor de servicios</param>
        public static void ConfigureMapster(IServiceProvider serviceProvider)
        {
            // Configuración global de Mapster para ignorar valores nulos
            TypeAdapterConfig.GlobalSettings.Default.IgnoreNullValues(true);

            // Configuración de mapeos para el usuario
            var userMapper = serviceProvider.GetRequiredService<UserMapper>();
            userMapper.ConfigureAllMappings();

            // Configuración de mapeos para el producto
            var productMapper = serviceProvider.GetRequiredService<ProductMapper>();
            productMapper.ConfigureAllMappings();

            // Configuración de mapeos para el carrito
            var cartMapper = serviceProvider.GetRequiredService<CartMapper>();
            cartMapper.ConfigureAllMappings();

            // Configuración de mapeos para la orden
            var orderMapper = serviceProvider.GetRequiredService<OrderMapper>();
            orderMapper.ConfigureAllMappings();
        }
    }
}