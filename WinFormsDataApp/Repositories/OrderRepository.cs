using Microsoft.EntityFrameworkCore;
using WinFormsDataApp.Data;
using WinFormsDataApp.Models;

namespace WinFormsDataApp.Repositories
{
    public class OrderRepository
    {
        private readonly AppDbContext _context;

        public OrderRepository(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Gets all active orders (not soft-deleted) from the database
        /// </summary>
        /// <returns>List of all active orders</returns>
        public async Task<List<Order>> GetAllAsync()
        {
            try
            {
                return await _context.Orders
                    .Include(o => o.Customer)
                    .Where(o => !o.IsDeleted)
                    .OrderByDescending(o => o.OrderDate)
                    .ThenByDescending(o => o.Id)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving orders: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets all active orders (not soft-deleted) with customer data from the database
        /// </summary>
        /// <returns>List of all active orders with customer data</returns>
        public async Task<List<Order>> GetAllWithCustomersAsync()
        {
            try
            {
                return await _context.Orders
                    .Include(o => o.Customer)
                    .Where(o => !o.IsDeleted)
                    .OrderByDescending(o => o.OrderDate)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error retrieving orders with customers: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets an order by its ID
        /// </summary>
        /// <param name="id">Order ID</param>
        /// <returns>Order if found, null otherwise</returns>
        public async Task<Order?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Orders
                    .Include(o => o.Customer)
                    .FirstOrDefaultAsync(o => o.Id == id && !o.IsDeleted);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving order with ID {id}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets all orders for a specific customer
        /// </summary>
        /// <param name="customerId">Customer ID</param>
        /// <returns>List of orders for the customer</returns>
        public async Task<List<Order>> GetByCustomerIdAsync(int customerId)
        {
            try
            {
                return await _context.Orders
                    .Include(o => o.Customer)
                    .Where(o => o.CustomerId == customerId && !o.IsDeleted)
                    .OrderByDescending(o => o.OrderDate)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving orders for customer {customerId}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets orders within a date range
        /// </summary>
        /// <param name="startDate">Start date</param>
        /// <param name="endDate">End date</param>
        /// <returns>List of orders within the date range</returns>
        public async Task<List<Order>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                return await _context.Orders
                    .Include(o => o.Customer)
                    .Where(o => o.OrderDate.Date >= startDate.Date &&
                               o.OrderDate.Date <= endDate.Date &&
                               !o.IsDeleted)
                    .OrderByDescending(o => o.OrderDate)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving orders for date range: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Adds a new order to the database
        /// </summary>
        /// <param name="order">Order to add</param>
        /// <returns>The added order with updated ID</returns>
        public async Task<Order> AddAsync(Order order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Validate customer exists
                var customer = await _context.Customers.FindAsync(order.CustomerId);
                if (customer == null)
                {
                    throw new InvalidOperationException($"Customer with ID {order.CustomerId} not found.");
                }

                // Validate order date
                if (order.OrderDate > DateTime.Now)
                {
                    throw new InvalidOperationException("Order date cannot be in the future.");
                }

                // Validate quantity
                if (order.Quantity < 1 || order.Quantity > 1000)
                {
                    throw new InvalidOperationException("Quantity must be between 1 and 1000.");
                }

                // Reset ID for new orders
                order.Id = 0;
                order.IsDeleted = false;

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                // Reload with customer data
                return await GetByIdAsync(order.Id) ?? order;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        /// <summary>
        /// Updates an existing order in the database
        /// </summary>
        /// <param name="order">Order to update</param>
        /// <returns>The updated order</returns>
        public async Task<Order> UpdateAsync(Order order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Check if order exists and is not deleted
                var existingOrder = await GetByIdAsync(order.Id);
                if (existingOrder == null)
                {
                    throw new InvalidOperationException($"Order with ID {order.Id} not found or has been deleted.");
                }

                // Validate customer exists
                var customer = await _context.Customers.FindAsync(order.CustomerId);
                if (customer == null)
                {
                    throw new InvalidOperationException($"Customer with ID {order.CustomerId} not found.");
                }

                // Validate order date
                if (order.OrderDate > DateTime.Now)
                {
                    throw new InvalidOperationException("Order date cannot be in the future.");
                }

                // Validate quantity
                if (order.Quantity < 1 || order.Quantity > 1000)
                {
                    throw new InvalidOperationException("Quantity must be between 1 and 1000.");
                }

                // Update properties
                existingOrder.CustomerId = order.CustomerId;
                existingOrder.Quantity = order.Quantity;
                existingOrder.OrderDate = order.OrderDate;

                _context.Orders.Update(existingOrder);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return await GetByIdAsync(existingOrder.Id) ?? existingOrder;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        /// <summary>
        /// Soft deletes an order (sets IsDeleted flag to true)
        /// </summary>
        /// <param name="id">ID of the order to delete</param>
        /// <returns>True if deleted successfully, false if order not found</returns>
        public async Task<bool> SoftDeleteAsync(int id)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var order = await _context.Orders.FindAsync(id);
                if (order == null || order.IsDeleted)
                {
                    return false;
                }

                order.IsDeleted = true;
                _context.Orders.Update(order);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        /// <summary>
        /// Permanently deletes an order from the database
        /// </summary>
        /// <param name="id">ID of the order to delete</param>
        /// <returns>True if deleted successfully, false if order not found</returns>
        public async Task<bool> HardDeleteAsync(int id)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var order = await _context.Orders.FindAsync(id);
                if (order == null)
                {
                    return false;
                }

                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        /// <summary>
        /// Gets the count of active orders in the database
        /// </summary>
        /// <returns>Number of active orders</returns>
        public async Task<int> GetCountAsync()
        {
            try
            {
                return await _context.Orders.CountAsync(o => !o.IsDeleted);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting order count: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets total quantity of all active orders
        /// </summary>
        /// <returns>Total quantity</returns>
        public async Task<int> GetTotalQuantityAsync()
        {
            try
            {
                return await _context.Orders
                    .Where(o => !o.IsDeleted)
                    .SumAsync(o => o.Quantity);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting total quantity: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets orders by quantity range
        /// </summary>
        /// <param name="minQuantity">Minimum quantity</param>
        /// <param name="maxQuantity">Maximum quantity</param>
        /// <returns>List of orders within the quantity range</returns>
        public async Task<List<Order>> GetByQuantityRangeAsync(int minQuantity, int maxQuantity)
        {
            try
            {
                return await _context.Orders
                    .Include(o => o.Customer)
                    .Where(o => o.Quantity >= minQuantity &&
                               o.Quantity <= maxQuantity &&
                               !o.IsDeleted)
                    .OrderByDescending(o => o.OrderDate)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving orders by quantity range: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets all soft-deleted orders
        /// </summary>
        /// <returns>List of soft-deleted orders</returns>
        public async Task<List<Order>> GetDeletedOrdersAsync()
        {
            try
            {
                return await _context.Orders
                    .IgnoreQueryFilters() // This ignores the global IsDeleted filter
                    .Include(o => o.Customer)
                    .Where(o => o.IsDeleted)
                    .OrderByDescending(o => o.OrderDate)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving deleted orders: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Restores a soft-deleted order
        /// </summary>
        /// <param name="id">ID of the order to restore</param>
        /// <returns>True if restored successfully, false if order not found</returns>
        public async Task<bool> RestoreAsync(int id)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var order = await _context.Orders
                    .IgnoreQueryFilters()
                    .FirstOrDefaultAsync(o => o.Id == id && o.IsDeleted);

                if (order == null)
                {
                    return false;
                }

                order.IsDeleted = false;
                _context.Orders.Update(order);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
