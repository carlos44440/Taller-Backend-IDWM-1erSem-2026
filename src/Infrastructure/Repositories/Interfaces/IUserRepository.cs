using TiendaUCN.src.Domain.Models;

namespace TiendaUCN.src.Infrastructure.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<bool> ExistsByNameAsync(string name);
        Task<bool> ExistsByEmailAsync(string email);
        Task<bool> ExistsByRutAsync(string rut);
        Task<bool> ExistsByPhoneNumberAsync(string phoneNumber);
        Task<int> CreateAsync(User user);
        Task<bool> SaveVerificationCodeAsync(int userId, string verificationCode, DateTime verificationCodeExpiry);
    }
}