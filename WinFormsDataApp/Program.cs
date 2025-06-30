using Microsoft.EntityFrameworkCore;
using System.Configuration;
using WinFormsDataApp.Data;
using WinFormsDataApp.Forms;
using WinFormsDataApp.Services;

namespace WinFormsDataApp;

static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static async Task Main()
    {
        try
        {
            // Initialize logging first
            LoggingService.InitializeLogging();

            // Initialize global exception handling
            GlobalExceptionHandler.Initialize();

            LoggingService.LogInformation("Application starting up");

            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            // Initialize database
            await InitializeDatabase();

            LoggingService.LogInformation("Running main application form");
            Application.Run(new MainForm());
        }
        catch (Exception ex)
        {
            // Log any startup errors
            LoggingService.LogFatal(ex, "Fatal error during application startup");

            MessageBox.Show(
                $"A fatal error occurred during application startup:\n\n{ex.Message}\n\nPlease check the log files for more details.",
                "Startup Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);

            Environment.Exit(1);
        }
        finally
        {
            // Ensure logging is properly closed
            LoggingService.CloseLogging();
        }
    }

    private static async Task InitializeDatabase()
    {
        try
        {
            LoggingService.LogInformation("Initializing database");

            var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"]?.ConnectionString
                ?? "Data Source=data.db";

            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlite(connectionString);

            using var context = new AppDbContext(optionsBuilder.Options);
            await DbInitializer.Initialize(context);

            LoggingService.LogInformation("Database initialization completed successfully");
        }
        catch (Exception ex)
        {
            LoggingService.LogError(ex, "Database initialization failed");
            throw; // Re-throw to be handled by the main try-catch
        }
    }
}