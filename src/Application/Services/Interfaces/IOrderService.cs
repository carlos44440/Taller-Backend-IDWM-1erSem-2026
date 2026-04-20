using TiendaUCN.src.Application.DTOs.OrderDTO;
using TiendaUCN.src.Application.DTOs.ProductDTO;

namespace TiendaUCN.src.Application.Services.Interfaces
{
    public interface IOrderService
    {
        Task<string> CreateOrderAsync(int userId);
        Task<OrderDetailDTO> GetOrderDetailAsync(string orderCode, int userId);
        Task<ListedOrderDetailDTO> GetOrdersByUserIdAsync(SearchParamsDTO searchParams, int userId);
    }
}