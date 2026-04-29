namespace TiendaUCN.src.Application.Services.Interfaces
{
    /// <summary>
    /// Interfaz del servicio de gestión de imágenes.
    /// </summary>
    public interface IImageService
    {
        /// <summary>
        /// Sube una imagen a Cloudinary y la asocia a un producto.
        /// </summary>
        /// <param name="file">El archivo de imagen a subir.</param>
        /// <param name="productId">El ID del producto al que se asociará la imagen.</param>
        /// <returns>Un valor que indica si la operación fue exitosa.</returns>
        Task<bool> UploadAsync(IFormFile file, int productId);

        /// <summary>
        /// Elimina una imagen de Cloudinary.
        /// </summary>
        /// <param name="publicId">El ID público de la imagen a eliminar.</param>
        /// <returns>Un valor que indica si la operación fue exitosa.</returns>
        Task<bool> DeleteAsync(string publicId);
    }
}