using Serilog;
using System;
using System.IO;

namespace WinFormsDataApp.Services
{
    public static class LoggingService
    {
        public static void InitializeLogging()
        {
            // Ensure logs directory exists
            var logsDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
            Directory.CreateDirectory(logsDirectory);

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.FromLogContext()
                .Enrich.WithProperty("Application", "WinFormsDataApp")
                .Enrich.WithProperty("Version", "1.0.0")
                .WriteTo.File(
                    path: Path.Combine(logsDirectory, "log-.txt"),
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 7, // Keep logs for 7 days
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] [{Application} v{Version}] {Message:lj}{NewLine}{Exception}",
                    shared: true // Allow multiple processes to write to the same file
                )
                .CreateLogger();

            Log.Information("Application started with logging initialized");
        }

        public static void CloseLogging()
        {
            Log.Information("Application shutting down");
            Log.CloseAndFlush();
        }

        public static void LogError(Exception exception, string? message = null)
        {
            if (string.IsNullOrEmpty(message))
            {
                Log.Error(exception, "An error occurred");
            }
            else
            {
                Log.Error(exception, message);
            }
        }

        public static void LogWarning(string message, params object[] args)
        {
            Log.Warning(message, args);
        }

        public static void LogInformation(string message, params object[] args)
        {
            Log.Information(message, args);
        }

        public static void LogDebug(string message, params object[] args)
        {
            Log.Debug(message, args);
        }

        public static void LogFatal(Exception exception, string message)
        {
            Log.Fatal(exception, message);
        }
    }
}
