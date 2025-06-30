using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.Configuration;

namespace WinFormsDataApp.Data
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

            // Read connection string from app.config
            var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"]?.ConnectionString
                ?? "Data Source=data.db";

            optionsBuilder.UseSqlite(connectionString);

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
