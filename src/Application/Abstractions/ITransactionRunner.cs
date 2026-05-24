namespace TiendaUCN.src.Application.Abstractions
{
    /// <summary>
    /// Ejecuta trabajo dentro de una transacción de base de datos.
    /// </summary>
    public interface ITransactionRunner
    {
        /// <summary>
        /// Ejecuta la operación dentro de una transacción y confirma los cambios si finaliza sin errores.
        /// </summary>
        /// <param name="work">Operación a ejecutar.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        Task ExecuteAsync(Func<Task> work, CancellationToken cancellationToken = default);
    }
}
