using Microsoft.EntityFrameworkCore;
using TiendaUCN.src.Domain.Models;
using TiendaUCN.src.Infrastructure.Data;
using TiendaUCN.src.Infrastructure.Repositories.Interfaces;

namespace TiendaUCN.src.Infrastructure.Repositories.Implements
{
    /// <summary>
    /// Implementación del repositorio de categorías.
    /// </summary>
    public class CategoryRepository : ICategoryRepository
    {
        /// <summary>
        /// Contexto de datos para acceder a la base de datos.
        /// </summary>
        private readonly DataContext _context;

        /// <summary>
        /// Constructor del repositorio de categorías.
        /// </summary>
        /// <param name="context">Contexto de datos para acceder a la base de datos</param>
        public CategoryRepository(DataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtiene las categorías activas ordenadas por nombre.
        /// </summary>
        /// <returns>Lista de categorías activas.</returns>
        public async Task<List<Category>> GetActiveAsync()
        {
            return await _context.Categories
                .Where(c => !c.IsDeleted)
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        /// <summary>
        /// Verifica si una categoría existe por su nombre.
        /// </summary>
        /// <param name="name">Nombre de la categoría</param>
        /// <returns>true si existe, false si no</returns>
        public async Task<bool> ExistsByNameAsync(string name)
        {
            return await _context.Categories
                .AnyAsync(b =>
                    b.Name.ToLower() == name.ToLower() &&
                    b.IsDeleted == false);
        }

        /// <summary>
        /// Crea una nueva categoría en la base de datos.
        /// </summary>
        /// <param name="category">La categoría a crear</param>
        /// <returns>true si la crea, false si no</returns>
        public async Task<bool> CreateAsync(Category category)
        {
            _context.Categories.Add(category);
            return await _context.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// Verifica si una categoría existe por su ID.
        /// </summary>
        /// <param name="id">El ID de la categoría</param>
        /// <returns>true si existe, false si no</returns>
        public async Task<bool> ExistsByIdAsync(int id)
        {
            return await _context.Categories.AnyAsync(b =>
                b.Id == id &&
                b.IsDeleted == false);
        }

        /// <summary>
        /// Actualiza el nombre de una categoría existente.
        /// </summary>
        /// <param name="id">El ID de la categoría</param>
        /// <param name="name">El nuevo nombre de la categoría</param>
        /// <returns>true si la actualiza, false si no</returns>
        public async Task<bool> UpdateNameAsync(int id, string name)
        {
            var result = await _context.Categories
                .Where(b => b.Id == id)
                .ExecuteUpdateAsync(b => b.SetProperty(x => x.Name, name));

            return result > 0;
        }

        /// <summary>
        /// Actualiza la descripción de una categoría existente.
        /// </summary>
        /// <param name="id">El ID de la categoría</param>
        /// <param name="description">La nueva descripción de la categoría</param>
        /// <returns>true si la actualiza, false si no</returns>
        public async Task<bool> UpdateDescriptionAsync(int id, string description)
        {
            var result = await _context.Categories
                .Where(b => b.Id == id)
                .ExecuteUpdateAsync(b => b.SetProperty(x => x.Description, description));

            return result > 0;
        }

        /// <summary>
        /// Elimina una categoría existente.
        /// </summary>
        /// <param name="id">El ID de la categoría</param>
        /// <returns>true si la elimina, false si no</returns>
        public async Task<bool> DeleteAsync(int id)
        {
            var result = await _context.Categories
                .Where(b => b.Id == id)
                .ExecuteUpdateAsync(b => b.SetProperty(x => x.IsDeleted, true));

            return result > 0;
        }

        /// <summary>
        /// Obtiene el ID de una categoría por su nombre.
        /// </summary>
        /// <param name="name">El nombre de la categoría</param>
        /// <returns>El ID de la categoría</returns>
        public async Task<int> GetIdByNameAsync(string name)
        {
            return await _context.Categories
                .Where(b => b.Name.ToLower() == name.ToLower() && !b.IsDeleted)
                .Select(b => b.Id)
                .FirstOrDefaultAsync();
        }
    }
}