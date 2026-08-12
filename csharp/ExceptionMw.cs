using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

public class ExceptionLoggingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            string contexto = $"{context.Request.Method} {context.Request.Path}{context.Request.QueryString}";

            ExceptionLogger.LogException(ex, contexto, escribirEnVisorEventos: true);

            // Si quieres que la API responda algo controlado en lugar de tumbarse:
            context.Response.Clear();
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsync(
                "{\"error\": \"Ocurrió un error interno. Contacte al administrador.\"}"
            );

            // Si prefieres que la excepción siga su curso normal (por ejemplo,
            // porque ya tienes otro middleware de manejo de errores), comenta
            // las 4 líneas de arriba y descomenta esto:
            // throw;
        }
    }
}