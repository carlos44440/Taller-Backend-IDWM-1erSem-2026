namespace TiendaUCN.src.Application.Jobs.Interfaces
{
    /// <summary>
    /// Interfaz para el trabajo de usuario
    /// </summary>
    public interface IUserJob
    {
        /// <summary>
        /// Elimina los usuarios no confirmados.
        /// </summary>
        /// <returns></returns>
        Task DeleteUnconfirmedUsersAsync();

        /// <summary>
        /// Elimina los tokens expirados en la blacklist.
        /// </summary>
        /// <returns></returns>
        Task DeleteExpiredTokensInBlacklistAsync();
    }
}