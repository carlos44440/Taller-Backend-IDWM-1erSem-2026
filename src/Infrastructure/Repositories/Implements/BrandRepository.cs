using Microsoft.EntityFrameworkCore;
using TiendaUCN.src.Domain.Models;
using TiendaUCN.src.Infrastructure.Data;
using TiendaUCN.src.Infrastructure.Repositories.Interfaces;

namespace TiendaUCN.src.Infrastructure.Repositories.Implements
{
    /// <summary>
    /// Repositorio de la marca.
    /// </summary>
    public class BrandRepository : IBrandRepository
    {
        /// <summary>
        /// Contexto de la base de datos.
        /// </summary>
        private readonly DataContext _context;

        /// <summary>
        /// Constructor del repositorio de la marca.
        /// </summary>
        /// <param name="context">El contexto de la base de datos</param>
        public BrandRepository(DataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Verifica si existe una marca por su nombre.
        /// </summary>
        /// <param name="name">El nombre de la marca</param>
        /// <returns>true si la marca existe, false si no</returns>
        public async Task<bool> ExistsByNameAsync(string name)
        {
            return await _context.Brands
                .AnyAsync(b =>
                    b.Name.ToLower() == name.ToLower() &&
                    b.IsDeleted == false);
        }

        /// <summary>
        /// Crea una nueva marca en la base de datos.
        /// </summary>
        /// <param name="brand">La marca a crear</param>
        /// <returns>true si la marca se crea correctamente, false si no</returns>
        public async Task<bool> CreateAsync(Brand brand)
        {
            _context.Brands.Add(brand);
            return await _context.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// Verifica si una marca existe por su ID.
        /// </summary>
        /// <param name="id">ID de la marca</param>
        /// <returns>true si existe, false si no</returns>
        public async Task<bool> ExistsByIdAsync(int id)
        {
            return await _context.Brands
                .AnyAsync(b =>
                    b.Id == id &&
                    b.IsDeleted == false);
        }

        /// <summary>
        /// Actualiza el nombre de una marca existente.
        /// </summary>
        /// <param name="id">ID de la marca</param>
        /// <param name="name">Nuevo nombre de la marca</param>
        /// <returns>true si la actualiza, false si no</returns>
        public async Task<bool> UpdateNameAsync(int id, string name)
        {
            var result = await _context.Brands
                .Where(b => b.Id == id)
                .ExecuteUpdateAsync(b => b.SetProperty(x => x.Name, name));

            return result > 0;
        }

        /// <summary>
        /// Actualiza la descripción de una marca existente.
        /// </summary>
        /// <param name="id">ID de la marca</param>
        /// <param name="description">Nueva descripción de la marca</param>
        /// <returns>true si la actualiza, false si no</returns>
        public async Task<bool> UpdateDescriptionAsync(int id, string description)
        {
            var result = await _context.Brands
                .Where(b => b.Id == id)
                .ExecuteUpdateAsync(b => b.SetProperty(x => x.Description, description));

            return result > 0;
        }

        /// <summary>
        /// Elimina una marca de la base de datos.
        /// </summary>
        /// <param name="id">ID de la marca</param>
        /// <returns>true si la elimina, false si no</returns>
        public async Task<bool> DeleteAsync(int id)
        {
            var result = await _context.Brands
                .Where(b => b.Id == id)
                .ExecuteUpdateAsync(b => b.SetProperty(x => x.IsDeleted, true));

            return result > 0;
        }

        /// <summary>
        /// Obtiene el ID de una marca por su nombre.
        /// </summary>
        /// <param name="name">Nombre de la marca</param>
        /// <returns>El ID de la marca si existe, 0 si no</returns>
        public async Task<int> GetIdByNameAsync(string name)
        {
            return await _context.Brands
                .Where(b => b.Name.ToLower() == name.ToLower() && !b.IsDeleted)
                .Select(b => b.Id)
                .FirstOrDefaultAsync();
        }
    }
}