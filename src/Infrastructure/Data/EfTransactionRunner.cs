using Microsoft.EntityFrameworkCore;
using TiendaUCN.src.Application.Abstractions;

namespace TiendaUCN.src.Infrastructure.Data
{
    /// <summary>
    /// Implementación de <see cref="ITransactionRunner"/> usando Entity Framework Core.
    /// </summary>
    public sealed class EfTransactionRunner : ITransactionRunner
    {
        /// <summary>
        /// Contexto de datos compartido con los repositorios del request.
        /// </summary>
        private readonly DataContext _context;

        /// <summary>
        /// Inicializa una nueva instancia de <see cref="EfTransactionRunner"/>.
        /// </summary>
        /// <param name="context">Contexto de datos compartido con los repositorios del request.</param>
        public EfTransactionRunner(DataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Ejecuta la operación dentro de una transacción y confirma los cambios si finaliza sin errores.
        /// </summary>
        /// <param name="work">Operación a ejecutar.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        public async Task ExecuteAsync(Func<Task> work, CancellationToken cancellationToken = default)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                await work();
                await transaction.CommitAsync(cancellationToken);
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }
    }
}
