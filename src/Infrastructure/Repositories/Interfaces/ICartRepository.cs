using TiendaUCN.src.Domain.Models;

namespace TiendaUCN.src.Infrastructure.Repositories.Interfaces
{
    public interface ICartRepository
    {
        Task<Cart?> GetByUserIdAsync(int userId);
        Task<Cart?> GetByBuyerIdAsync(string buyerId);
        Task<Cart?> CreateToUserByBuyerCartAsync(Cart buyerCart, int userId);
        Task<bool> CreateAsync(Cart cart);
        Task<bool> AddItemAsync(Cart cart, CartItem cartItem);
        Task UpdateItemQuantityAsync(int cartId, int cartItemId, int newQuantity);
        Task UpdateTotalPriceAsync(int cartId, int newTotalPrice);
    }
}