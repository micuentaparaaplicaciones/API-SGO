// Revisado
using Microsoft.EntityFrameworkCore;

namespace API.Exceptions
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            // El error logeado se puede ver en el output.
            _logger.LogError(ex, "Error: {Message} - Ruta: {Path}", ex.Message, context.Request.Path);

            var response = context.Response;
            response.ContentType = "application/json";

            var (statusCode, message) = ex switch
            {
                KeyNotFoundException => (StatusCodes.Status404NotFound, "Recurso no encontrado."),
                ArgumentException => (StatusCodes.Status400BadRequest, ex.Message),
                UnauthorizedAccessException => (StatusCodes.Status401Unauthorized, "No autorizado."),
                DbUpdateConcurrencyException => (StatusCodes.Status409Conflict, "Conflicto de concurrencia detectado. El registro ha sido modificado por otro proceso."),
                DbUpdateException dbEx when dbEx.InnerException?.Message.Contains("ORA-00001") == true
                    => (StatusCodes.Status409Conflict, "La clave ya existe en la base de datos."),
                DbUpdateException => (StatusCodes.Status400BadRequest, "Error al actualizar la base de datos."),
                _ => (StatusCodes.Status500InternalServerError, "Ocurrió un error interno en el servidor.")
            };

            response.StatusCode = statusCode;
            await response.WriteAsJsonAsync(new
            {
                StatusCode = statusCode,
                Message = message,
                DetailedError = ex.Message
            });
        }
    }

}
