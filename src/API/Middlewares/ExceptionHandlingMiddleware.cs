using Serilog;
using System.Security;
using System.Text.Json;
using Tienda_UCN_api.src.Application.DTO.BaseResponse;

namespace TiendaUCN.src.API.Middlewares
{
    /// <summary>
    /// Middleware del manejo de excepciones.
    /// </summary>
    /// <param name="next"> El siguiente middleware en la cadena de ejecución. </param>
    public class ExceptionHandlingMiddleware(RequestDelegate next)
    {
        // Punto de entrada del middleware, recibe el siguiente delegado en la cadena de middlewares
        private readonly RequestDelegate _next = next;

        /// <summary>
        /// Método que se ejecuta para cada solicitud HTTP. Intenta ejecutar el siguiente middleware y captura cualquier excepción no controlada.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Intenta ejecutar el siguiente middleware en la cadena
                // Si no ocurre ninguna excepción, la solicitud se procesa normalmente
                await _next(context);
            }
            catch (Exception ex)
            {
                // Genera un ID de traza único para esta excepción
                var traceId = Guid.NewGuid().ToString();
                // Lo agrega a los encabezados de la respuesta
                context.Response.Headers["trace-id"] = traceId;

                // Mapea la excepción a un código de estado HTTP y un título descriptivo
                var (statusCode, title) = MapExceptionToStatus(ex);

                // Crea un objeto de error con el título y el mensaje de la excepción
                ErrorDetail error = new ErrorDetail(title, ex.Message);

                Log.Error(ex, "Excepción no controlada. Trace ID: {TraceId}", traceId);

                // Define que la respuesta será JSON y establece el código de estado HTTP
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = statusCode;

                // Serializa el objeto de error a JSON con formato camelCase
                var json = JsonSerializer.Serialize(
                    error,
                    new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }
                );

                // Escribe el JSON de error en la respuesta HTTP
                await context.Response.WriteAsync(json);
            }
        }

        /// <summary>
        /// Metodo para mapear de excepciones a códigos de estado HTTP y títulos descriptivos.
        /// </summary>
        /// <param name="ex">La excepción a mapear.</param>
        /// <returns>Una tupla con el código de estado y el título descriptivo.</returns>
        private static (int, string) MapExceptionToStatus(Exception ex)
        {
            return ex switch
            {
                // Mapea diferentes tipos de excepciones a códigos de estado HTTP y títulos descriptivos
                UnauthorizedAccessException _ => (StatusCodes.Status401Unauthorized, "No autorizado"),
                ArgumentNullException _ => (StatusCodes.Status400BadRequest, "Solicitud inválida"),
                KeyNotFoundException _ => (StatusCodes.Status404NotFound, "Recurso no encontrado"),
                InvalidOperationException _ => (StatusCodes.Status409Conflict, "Conflicto de operación"),
                FormatException _ => (StatusCodes.Status400BadRequest, "Formato inválido"),
                SecurityException _ => (StatusCodes.Status403Forbidden, "Acceso prohibido"),
                ArgumentOutOfRangeException _ => (StatusCodes.Status400BadRequest, "Argumento fuera de rango"),
                ArgumentException _ => (StatusCodes.Status400BadRequest, "Argumento inválido"),
                TimeoutException _ => (StatusCodes.Status429TooManyRequests, "Demasiadas solicitudes"),
                JsonException _ => (StatusCodes.Status400BadRequest, "JSON inválido"),
                _ => (StatusCodes.Status500InternalServerError, "Error interno del servidor"),
            };
        }
    }
}