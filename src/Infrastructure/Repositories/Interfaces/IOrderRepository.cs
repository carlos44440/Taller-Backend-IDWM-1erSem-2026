using TiendaUCN.src.Application.DTOs.ProductDTO;
using TiendaUCN.src.Domain.Models;

namespace TiendaUCN.src.Infrastructure.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        Task<bool> ExistsByCodeAsync(string code);
        Task<bool> CreateAsync(Order order);
        Task<Order?> GetByCodeAsync(string code, int userId);
        Task<(IEnumerable<Order> orders, int totalCount)> GetFilteredForUserIdAsync(SearchParamsDTO searchParams, int userId);
    }
}