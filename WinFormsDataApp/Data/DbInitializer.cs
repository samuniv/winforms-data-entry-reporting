using Microsoft.EntityFrameworkCore;
using WinFormsDataApp.Models;

namespace WinFormsDataApp.Data
{
    public static class DbInitializer
    {
        public static async Task Initialize(AppDbContext context)
        {
            // Apply any pending migrations
            await context.Database.MigrateAsync();

            // Check if database has been seeded
            if (context.Customers.Any())
            {
                return; // Database has been seeded
            }

            // Seed with sample customers
            var customers = new[]
            {
                new Customer
                {
                    Name = "John Smith",
                    Email = "john.smith@email.com",
                    Phone = "(555) 123-4567",
                    Address = "123 Main St, Anytown, USA"
                },
                new Customer
                {
                    Name = "Jane Doe",
                    Email = "jane.doe@email.com",
                    Phone = "(555) 987-6543",
                    Address = "456 Oak Ave, Somewhere, USA"
                },
                new Customer
                {
                    Name = "Bob Johnson",
                    Email = "bob.johnson@email.com",
                    Phone = "(555) 555-5555",
                    Address = "789 Pine Rd, Anywhere, USA"
                },
                new Customer
                {
                    Name = "Alice Williams",
                    Email = "alice.williams@email.com",
                    Phone = "(555) 111-2222",
                    Address = "321 Elm St, Nowhere, USA"
                }
            };

            context.Customers.AddRange(customers);
            await context.SaveChangesAsync();

            // Seed with sample orders
            var orders = new[]
            {
                new Order
                {
                    CustomerId = 1,
                    Quantity = 5,
                    OrderDate = DateTime.Now.AddDays(-10)
                },
                new Order
                {
                    CustomerId = 1,
                    Quantity = 3,
                    OrderDate = DateTime.Now.AddDays(-5)
                },
                new Order
                {
                    CustomerId = 2,
                    Quantity = 10,
                    OrderDate = DateTime.Now.AddDays(-8)
                },
                new Order
                {
                    CustomerId = 3,
                    Quantity = 2,
                    OrderDate = DateTime.Now.AddDays(-3)
                },
                new Order
                {
                    CustomerId = 4,
                    Quantity = 7,
                    OrderDate = DateTime.Now.AddDays(-1)
                }
            };

            context.Orders.AddRange(orders);
            await context.SaveChangesAsync();
        }
    }
}
