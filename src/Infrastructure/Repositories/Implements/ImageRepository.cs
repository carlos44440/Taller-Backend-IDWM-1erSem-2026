using Microsoft.EntityFrameworkCore;
using TiendaUCN.src.Domain.Models;
using TiendaUCN.src.Infrastructure.Data;
using TiendaUCN.src.Infrastructure.Repositories.Interfaces;

namespace TiendaUCN.src.Infrastructure.Repositories.Implements
{
    /// <summary>
    /// Implementación del repositorio de imágenes.
    /// </summary>
    public class ImageRepository : IImageRepository
    {
        /// <summary>
        /// Contexto de datos para acceder a la base de datos.
        /// </summary>
        private readonly DataContext _context;

        /// <summary>
        /// Constructor del repositorio de imágenes.
        /// </summary>
        /// <param name="context">Contexto de datos para acceder a la base de datos</param>
        public ImageRepository(DataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Crea una nueva imagen en la base de datos.
        /// </summary>
        /// <param name="image">La imagen a crear</param>
        /// <returns>true si la crea, false si no</returns>
        public async Task<bool?> CreateAsync(Image image)
        {
            var existsImage = await _context.Images.AnyAsync(i => i.PublicId == image.PublicId);
            if (!existsImage)
            {
                _context.Images.Add(image);

                // Si la imagen se agrega correctamente, retorna true, de lo contrario false
                return await _context.SaveChangesAsync() > 0;
            }

            // Si la image ya existe, retrorna null
            return null;
        }

        /// <summary>
        /// Elimina una imagen de la base de datos.
        /// </summary>
        /// <param name="publicId">El ID público de la imagen</param>
        /// <returns>true si la elimina, false si no, null si no existe</returns>
        public async Task<bool?> DeleteAsync(string publicId)
        {
            var image = await _context.Images.FirstOrDefaultAsync(i => i.PublicId == publicId);
            if (image != null)
            {
                _context.Images.Remove(image);

                // Si la imagen se elimina correctamente, retorna true, de lo contrario false
                return await _context.SaveChangesAsync() > 0;
            }

            // Si la imagen no existe, retrorna null
            return null;
        }
    }
}