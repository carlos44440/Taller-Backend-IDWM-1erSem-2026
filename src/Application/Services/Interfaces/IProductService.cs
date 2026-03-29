using TiendaUCN.src.Application.DTOs.ProductDTO;

namespace TiendaUCN.src.Application.Services.Interfaces
{
    public interface IProductService
    {
        Task<string> CreateProductAsync(CreateProductDTO createProductDTO);
        Task SwitchStatusProductAsync(int id);
        Task<ProductDetailCustomerDTO> GetProductByIdForCustomerAsync(int id);
        Task<ProductDetailAdminDTO> GetProductByIdForAdminAsync(int id);

    }
}