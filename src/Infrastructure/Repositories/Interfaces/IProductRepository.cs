using TiendaUCN.src.Application.DTOs.ProductDTO;
using TiendaUCN.src.Domain.Models;

namespace TiendaUCN.src.Infrastructure.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<bool> ExistsByNameAndBrandAsync(string name, string brandName);
        Task<bool> CreateAsync(Product product);
        Task<bool> ExistsByIdAsync(int id);
        Task<bool> SwitchStatusAsync(int id);
        Task<bool> ExistsByIdCustomerAsync(int id);
        Task<Product?> GetProductByIdForCustomerAsync(int id);
        Task<Product?> GetProductByIdForAdminAsync(int id);
    }
}