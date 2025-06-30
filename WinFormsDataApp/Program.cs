using Microsoft.EntityFrameworkCore;
using System.Configuration;
using WinFormsDataApp.Data;
using WinFormsDataApp.Forms;

namespace WinFormsDataApp;

static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static async Task Main()
    {
        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();

        // Initialize database
        await InitializeDatabase();

        Application.Run(new MainForm());
    }

    private static async Task InitializeDatabase()
    {
        try
        {
            var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"]?.ConnectionString
                ?? "Data Source=data.db";

            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlite(connectionString);

            using var context = new AppDbContext(optionsBuilder.Options);
            await DbInitializer.Initialize(context);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Database initialization failed: {ex.Message}", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            Environment.Exit(1);
        }
    }
}