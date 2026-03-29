using TiendaUCN.src.Application.DTOs.ProductDTO;

namespace TiendaUCN.src.Application.Services.Interfaces
{
    public interface IProductService
    {
        Task<string> CreateProductAsync(CreateProductDTO createProductDTO);
    }
}