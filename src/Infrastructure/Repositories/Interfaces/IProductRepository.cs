using TiendaUCN.src.Domain.Models;

namespace TiendaUCN.src.Infrastructure.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<bool> ExistsByNameAndBrandAsync(string name, string brandName);
        Task<bool> CreateAsync(Product product);
    }
}