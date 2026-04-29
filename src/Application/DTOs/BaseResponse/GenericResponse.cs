namespace Tienda_UCN_api.src.Application.DTO
{
    /// <summary>
    /// DTO genérico para representar una respuesta de la API.
    /// </summary>
    /// <typeparam name="T">El tipo de datos de la respuesta.</typeparam>
    /// <param name="message">El mensaje de la respuesta.</param>
    /// <param name="data">Los datos de la respuesta.</param>
    public class GenericResponse<T>(string message, T? data = default)
    {
        /// <summary>
        /// El mensaje de la respuesta.
        /// </summary>
        public string Message { get; set; } = message;

        /// <summary>
        /// Los datos de la respuesta.
        /// </summary>
        public T? Data { get; set; } = data;
    }
}