namespace TiendaUCN.src.Application.Services.Interfaces
{
    public interface IOrderService
    {
        Task<string> CreateOrderAsync(int userId);
    }
}