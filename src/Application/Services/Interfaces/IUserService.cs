using TiendaUCN.src.Application.DTOs.AuthDTO;

namespace TiendaUCN.src.Application.Services.Interfaces
{
    public interface IUserService
    {
        Task<string> RegisterAsync(RegisterDTO registerDTO);

    }
}