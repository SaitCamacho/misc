using System.Diagnostics;
using System.Text;
using Microsoft.AspNetCore.Http;

public class ApiTraceMiddleware
{
    private readonly RequestDelegate _next;

    public ApiTraceMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();

        // Habilitar buffering para poder leer el body sin "consumirlo"
        context.Request.EnableBuffering();

        string bodyRequest = await LeerBodyRequest(context.Request);

        // Capturar el body de respuesta
        var originalBodyStream = context.Response.Body;
        using var responseBody = new MemoryStream();
        context.Response.Body = responseBody;

        string? errorMensaje = null;

        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            errorMensaje = ex.Message;
            throw; // lo re-lanzamos para que tu manejador global de excepciones lo capture
        }
        finally
        {
            stopwatch.Stop();

            string bodyResponse = await LeerBodyResponse(context.Response);

            ApiTraceLogger.LogTrace(
                metodo: context.Request.Method,
                ruta: context.Request.Path,
                queryString: context.Request.QueryString.ToString(),
                statusCode: context.Response.StatusCode,
                duracionMs: stopwatch.ElapsedMilliseconds,
                ip: context.Connection.RemoteIpAddress?.ToString(),
                usuario: context.User?.Identity?.IsAuthenticated == true ? context.User.Identity.Name : null,
                bodyRequest: bodyRequest,
                bodyResponse: bodyResponse,
                errorMensaje: errorMensaje
            );

            await responseBody.CopyToAsync(originalBodyStream);
        }
    }

    private static async Task<string> LeerBodyRequest(HttpRequest request)
    {
        if (!request.Body.CanSeek) return string.Empty;

        request.Body.Position = 0;
        using var reader = new StreamReader(request.Body, Encoding.UTF8, leaveOpen: true);
        string body = await reader.ReadToEndAsync();
        request.Body.Position = 0;
        return body;
    }

    private static async Task<string> LeerBodyResponse(HttpResponse response)
    {
        response.Body.Seek(0, SeekOrigin.Begin);
        using var reader = new StreamReader(response.Body, Encoding.UTF8, leaveOpen: true);
        string body = await reader.ReadToEndAsync();
        response.Body.Seek(0, SeekOrigin.Begin);
        return body;
    }
}