using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

public static class ApiTraceLogger
{
    private static readonly string LogFolder = Path.Combine(AppContext.BaseDirectory, "Logs", "Trazabilidad");
    private static readonly object _lock = new object();

    public static void LogTrace(
        string metodo,
        string ruta,
        string? queryString,
        int statusCode,
        long duracionMs,
        string? ip = null,
        string? usuario = null,
        string? bodyRequest = null,
        string? bodyResponse = null,
        string? errorMensaje = null)
    {
        try
        {
            if (!Directory.Exists(LogFolder))
                Directory.CreateDirectory(LogFolder);

            string fileName = $"trace_{DateTime.Now:yyyy-MM-dd}.txt";
            string filePath = Path.Combine(LogFolder, fileName);

            var sb = new StringBuilder();
            sb.AppendLine("========================================");
            sb.AppendLine($"Fecha:       {DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}");
            sb.AppendLine($"Método:      {metodo}");
            sb.AppendLine($"Ruta:        {ruta}{(string.IsNullOrEmpty(queryString) ? "" : queryString)}");
            sb.AppendLine($"StatusCode:  {statusCode}");
            sb.AppendLine($"Duración:    {duracionMs} ms");
            if (!string.IsNullOrWhiteSpace(ip))
                sb.AppendLine($"IP:          {ip}");
            if (!string.IsNullOrWhiteSpace(usuario))
                sb.AppendLine($"Usuario:     {usuario}");
            if (!string.IsNullOrWhiteSpace(bodyRequest))
                sb.AppendLine($"Body Req:    {Truncar(bodyRequest)}");
            if (!string.IsNullOrWhiteSpace(bodyResponse))
                sb.AppendLine($"Body Resp:   {Truncar(bodyResponse)}");
            if (!string.IsNullOrWhiteSpace(errorMensaje))
                sb.AppendLine($"Error:       {errorMensaje}");
            sb.AppendLine("========================================");
            sb.AppendLine();

            lock (_lock)
            {
                File.AppendAllText(filePath, sb.ToString(), Encoding.UTF8);
            }
        }
        catch
        {
            // El logger nunca debe tumbar la API.
        }
    }

    private static string Truncar(string texto, int maxLength = 2000)
    {
        return texto.Length > maxLength
            ? texto.Substring(0, maxLength) + "... (truncado)"
            : texto;
    }
}