using Microsoft.EntityFrameworkCore;
using WinFormsDataApp.Data;
using WinFormsDataApp.Models;

namespace WinFormsDataApp.Repositories
{
    public class CustomerRepository
    {
        private readonly AppDbContext _context;

        public CustomerRepository(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Gets all customers from the database
        /// </summary>
        /// <returns>List of all customers</returns>
        public async Task<List<Customer>> GetAllAsync()
        {
            try
            {
                return await _context.Customers
                    .Include(c => c.Orders.Where(o => !o.IsDeleted))
                    .OrderBy(c => c.Name)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving customers: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets a customer by their ID
        /// </summary>
        /// <param name="id">Customer ID</param>
        /// <returns>Customer if found, null otherwise</returns>
        public async Task<Customer?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Customers
                    .Include(c => c.Orders.Where(o => !o.IsDeleted))
                    .FirstOrDefaultAsync(c => c.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving customer with ID {id}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets a customer by their email address
        /// </summary>
        /// <param name="email">Customer email</param>
        /// <returns>Customer if found, null otherwise</returns>
        public async Task<Customer?> GetByEmailAsync(string email)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email))
                    return null;

                return await _context.Customers
                    .Include(c => c.Orders.Where(o => !o.IsDeleted))
                    .FirstOrDefaultAsync(c => c.Email.ToLower() == email.ToLower());
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving customer with email {email}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Adds a new customer to the database
        /// </summary>
        /// <param name="customer">Customer to add</param>
        /// <returns>The added customer with updated ID</returns>
        public async Task<Customer> AddAsync(Customer customer)
        {
            if (customer == null)
                throw new ArgumentNullException(nameof(customer));

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Check for duplicate email
                var existingCustomer = await GetByEmailAsync(customer.Email);
                if (existingCustomer != null)
                {
                    throw new InvalidOperationException($"A customer with email '{customer.Email}' already exists.");
                }

                // Reset ID for new customers
                customer.Id = 0;

                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return customer;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        /// <summary>
        /// Updates an existing customer in the database
        /// </summary>
        /// <param name="customer">Customer to update</param>
        /// <returns>The updated customer</returns>
        public async Task<Customer> UpdateAsync(Customer customer)
        {
            if (customer == null)
                throw new ArgumentNullException(nameof(customer));

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Check if customer exists
                var existingCustomer = await GetByIdAsync(customer.Id);
                if (existingCustomer == null)
                {
                    throw new InvalidOperationException($"Customer with ID {customer.Id} not found.");
                }

                // Check for duplicate email (excluding current customer)
                var duplicateCustomer = await _context.Customers
                    .FirstOrDefaultAsync(c => c.Email.ToLower() == customer.Email.ToLower() && c.Id != customer.Id);

                if (duplicateCustomer != null)
                {
                    throw new InvalidOperationException($"Another customer with email '{customer.Email}' already exists.");
                }

                // Update properties
                existingCustomer.Name = customer.Name;
                existingCustomer.Email = customer.Email;
                existingCustomer.Phone = customer.Phone;
                existingCustomer.Address = customer.Address;

                _context.Customers.Update(existingCustomer);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return existingCustomer;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        /// <summary>
        /// Deletes a customer from the database
        /// </summary>
        /// <param name="id">ID of the customer to delete</param>
        /// <returns>True if deleted successfully, false if customer not found</returns>
        public async Task<bool> DeleteAsync(int id)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var customer = await GetByIdAsync(id);
                if (customer == null)
                {
                    return false;
                }

                // Check if customer has orders
                var hasOrders = await _context.Orders
                    .AnyAsync(o => o.CustomerId == id && !o.IsDeleted);

                if (hasOrders)
                {
                    throw new InvalidOperationException("Cannot delete customer with existing orders. Please delete or reassign orders first.");
                }

                _context.Customers.Remove(customer);
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
        /// Gets the count of customers in the database
        /// </summary>
        /// <returns>Number of customers</returns>
        public async Task<int> GetCountAsync()
        {
            try
            {
                return await _context.Customers.CountAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting customer count: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Searches customers by name or email
        /// </summary>
        /// <param name="searchTerm">Search term</param>
        /// <returns>List of matching customers</returns>
        public async Task<List<Customer>> SearchAsync(string searchTerm)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchTerm))
                {
                    return await GetAllAsync();
                }

                var term = searchTerm.ToLower().Trim();

                return await _context.Customers
                    .Include(c => c.Orders.Where(o => !o.IsDeleted))
                    .Where(c => c.Name.ToLower().Contains(term) ||
                               c.Email.ToLower().Contains(term))
                    .OrderBy(c => c.Name)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error searching customers: {ex.Message}", ex);
            }
        }
    }
}
