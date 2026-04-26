namespace Tienda_UCN_api.src.Application.DTO.BaseResponse
{
    /// <summary>
    /// DTO para representar detalles de un error en una respuesta de la API.
    /// </summary>
    /// <param name="message">El mensaje de error.</param>
    /// <param name="details">Los detalles del error.</param>
    public class ErrorDetail(string message, string? details = null)
    {
        /// <summary>
        /// El mensaje de error.
        /// </summary>
        public string Message { get; set; } = message;

        /// <summary>
        /// Información adicional sobre el error.
        /// </summary>
        public string? Details { get; set; } = details;
    }
}