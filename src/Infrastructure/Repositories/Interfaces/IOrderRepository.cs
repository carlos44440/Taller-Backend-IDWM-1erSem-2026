using TiendaUCN.src.Domain.Models;

namespace TiendaUCN.src.Infrastructure.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        Task<bool> ExistsByCodeAsync(string code);
        Task<bool> CreateAsync(Order order);
    }
}