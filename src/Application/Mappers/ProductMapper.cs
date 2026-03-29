using Mapster;
using TiendaUCN.src.Application.DTOs.ProductDTO;
using TiendaUCN.src.Domain.Models;

namespace TiendaUCN.src.Application.Mappers
{
    public class ProductMapper
    {
        public void ConfigureAllMappings()
        {
            ConfigureProductMappings();
        }

        public void ConfigureProductMappings()
        {
            TypeAdapterConfig<CreateBrandCategoryDTO, Brand>.NewConfig();
            TypeAdapterConfig<CreateBrandCategoryDTO, Category>.NewConfig();
        }
    }
}