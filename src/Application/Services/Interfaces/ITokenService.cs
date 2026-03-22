using TiendaUCN.src.Domain.Models;

namespace TiendaUCN.src.Application.Services.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(User user, string roleName);
    }
}