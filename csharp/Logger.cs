using System;
using System.Diagnostics;
using System.IO;
using System.Text;

public static class ExceptionLogger
{
    private static readonly string LogFolder = Path.Combine(AppContext.BaseDirectory, "Logs", "Errores");
    private static readonly object _lock = new object();

    private const string EventSource = "MiAplicacion"; // cámbialo por el nombre de tu app
    private const string EventLogName = "Application";

    public static void LogException(Exception ex, string? contexto = null, bool escribirEnVisorEventos = false)
    {
        try
        {
            if (!Directory.Exists(LogFolder))
                Directory.CreateDirectory(LogFolder);

            string fileName = $"error_{DateTime.Now:yyyy-MM-dd}.txt";
            string filePath = Path.Combine(LogFolder, fileName);

            var sb = new StringBuilder();
            sb.AppendLine("========================================");
            sb.AppendLine($"Fecha:      {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            if (!string.IsNullOrWhiteSpace(contexto))
                sb.AppendLine($"Contexto:   {contexto}");
            sb.AppendLine($"Tipo:       {ex.GetType().FullName}");
            sb.AppendLine($"Mensaje:    {ex.Message}");
            sb.AppendLine($"StackTrace: {ex.StackTrace}");

            var inner = ex.InnerException;
            int nivel = 1;
            while (inner != null)
            {
                sb.AppendLine($"--- InnerException nivel {nivel} ---");
                sb.AppendLine($"Tipo:       {inner.GetType().FullName}");
                sb.AppendLine($"Mensaje:    {inner.Message}");
                sb.AppendLine($"StackTrace: {inner.StackTrace}");
                inner = inner.InnerException;
                nivel++;
            }

            sb.AppendLine("========================================");
            sb.AppendLine();

            string textoCompleto = sb.ToString();

            lock (_lock)
            {
                File.AppendAllText(filePath, textoCompleto, Encoding.UTF8);
            }

            if (escribirEnVisorEventos)
            {
                EscribirEnVisorEventos(textoCompleto);
            }
        }
        catch
        {
            // Nunca dejes que el logger tumbe la app.
        }
    }

    private static void EscribirEnVisorEventos(string mensaje)
    {
        try
        {
            if (!OperatingSystem.IsWindows())
                return; // EventLog solo existe en Windows

            if (!EventLog.SourceExists(EventSource))
            {
                // Crear la fuente requiere permisos de administrador,
                // normalmente se hace una sola vez en instalación/setup.
                EventLog.CreateEventSource(EventSource, EventLogName);
            }

            // El visor de eventos tiene un límite práctico de tamaño de mensaje
            string mensajeRecortado = mensaje.Length > 30000
                ? mensaje.Substring(0, 30000) + "... (truncado)"
                : mensaje;

            EventLog.WriteEntry(EventSource, mensajeRecortado, EventLogEntryType.Error);
        }
        catch
        {
            // Si falla el visor de eventos, no debe afectar el logueo a archivo.
        }
    }
}