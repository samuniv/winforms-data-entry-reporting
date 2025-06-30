using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.Text;
using WinFormsDataApp.DTOs;
using WinFormsDataApp.Models;
using WinFormsDataApp.Repositories;
using WinFormsDataApp.Services;

namespace WinFormsDataApp.Services.ImportExport
{
    public class CsvImportService
    {
        private readonly CustomerRepository _customerRepository;
        private readonly OrderRepository _orderRepository;

        public CsvImportService(CustomerRepository customerRepository,
                               OrderRepository orderRepository)
        {
            _customerRepository = customerRepository;
            _orderRepository = orderRepository;
        }

        public async Task<CsvImportResult<CustomerImportDto>> ImportCustomersFromCsvAsync(string filePath, bool hasHeaders = true)
        {
            var result = new CsvImportResult<CustomerImportDto>();

            try
            {
                LoggingService.LogInformation($"Starting customer CSV import from: {filePath}");

                using var reader = new StringReader(File.ReadAllText(filePath, Encoding.UTF8));
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = hasHeaders,
                    MissingFieldFound = null,
                    BadDataFound = null
                };

                using var csv = new CsvReader(reader, config);
                var records = csv.GetRecords<CustomerImportDto>().ToList();

                result.TotalRecords = records.Count;

                // Validate records
                foreach (var record in records)
                {
                    ValidateCustomerRecord(record);
                    if (record.IsValid)
                    {
                        result.ValidRecords.Add(record);
                    }
                    else
                    {
                        result.InvalidRecords.Add(record);
                    }
                }

                LoggingService.LogInformation($"CSV parsed: {result.TotalRecords} total, {result.ValidRecords.Count} valid, {result.InvalidRecords.Count} invalid");
            }
            catch (Exception ex)
            {
                LoggingService.LogError(ex, "Error importing customers from CSV");
                result.Errors.Add($"Failed to read CSV file: {ex.Message}");
            }

            return result;
        }

        public async Task<CsvImportResult<OrderImportDto>> ImportOrdersFromCsvAsync(string filePath, bool hasHeaders = true)
        {
            var result = new CsvImportResult<OrderImportDto>();

            try
            {
                LoggingService.LogInformation($"Starting order CSV import from: {filePath}");

                using var reader = new StringReader(File.ReadAllText(filePath, Encoding.UTF8));
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = hasHeaders,
                    MissingFieldFound = null,
                    BadDataFound = null
                };

                using var csv = new CsvReader(reader, config);
                var records = csv.GetRecords<OrderImportDto>().ToList();

                result.TotalRecords = records.Count;

                // Get existing customers for lookup
                var existingCustomers = await _customerRepository.GetAllAsync();
                var customerLookup = existingCustomers.ToDictionary(c => c.Name.ToLowerInvariant(), c => c.Id);

                // Validate and resolve customer references
                foreach (var record in records)
                {
                    await ValidateOrderRecord(record, customerLookup);
                    if (record.IsValid)
                    {
                        result.ValidRecords.Add(record);
                    }
                    else
                    {
                        result.InvalidRecords.Add(record);
                    }
                }

                LoggingService.LogInformation($"CSV parsed: {result.TotalRecords} total, {result.ValidRecords.Count} valid, {result.InvalidRecords.Count} invalid");
            }
            catch (Exception ex)
            {
                LoggingService.LogError(ex, "Error importing orders from CSV");
                result.Errors.Add($"Failed to read CSV file: {ex.Message}");
            }

            return result;
        }

        public async Task<ImportSaveResult> SaveCustomersAsync(List<CustomerImportDto> validCustomers)
        {
            var result = new ImportSaveResult();

            try
            {
                LoggingService.LogInformation($"Saving {validCustomers.Count} customers to database");

                foreach (var customerDto in validCustomers)
                {
                    try
                    {
                        var customer = new Customer
                        {
                            Name = customerDto.Name.Trim(),
                            Email = customerDto.Email.Trim(),
                            Phone = customerDto.Phone.Trim(),
                            Address = customerDto.Address.Trim()
                        };

                        await _customerRepository.AddAsync(customer);
                        result.SuccessCount++;
                    }
                    catch (Exception ex)
                    {
                        result.FailureCount++;
                        result.Errors.Add($"Customer '{customerDto.Name}': {ex.Message}");
                        LoggingService.LogError(ex, $"Failed to save customer '{customerDto.Name}'");
                    }
                }

                LoggingService.LogInformation($"Customer import completed: {result.SuccessCount} saved, {result.FailureCount} failed");
            }
            catch (Exception ex)
            {
                LoggingService.LogError(ex, "Error during customer save operation");
                result.Errors.Add($"Save operation failed: {ex.Message}");
            }

            return result;
        }

        public async Task<ImportSaveResult> SaveOrdersAsync(List<OrderImportDto> validOrders)
        {
            var result = new ImportSaveResult();

            try
            {
                LoggingService.LogInformation($"Saving {validOrders.Count} orders to database");

                foreach (var orderDto in validOrders)
                {
                    try
                    {
                        var order = new Order
                        {
                            CustomerId = orderDto.ResolvedCustomerId!.Value,
                            Quantity = orderDto.Quantity!.Value,
                            OrderDate = orderDto.OrderDate!.Value
                        };

                        await _orderRepository.AddAsync(order);
                        result.SuccessCount++;
                    }
                    catch (Exception ex)
                    {
                        result.FailureCount++;
                        result.Errors.Add($"Order for customer ID {orderDto.ResolvedCustomerId}: {ex.Message}");
                        LoggingService.LogError(ex, $"Failed to save order for customer ID {orderDto.ResolvedCustomerId}");
                    }
                }

                LoggingService.LogInformation($"Order import completed: {result.SuccessCount} saved, {result.FailureCount} failed");
            }
            catch (Exception ex)
            {
                LoggingService.LogError(ex, "Error during order save operation");
                result.Errors.Add($"Save operation failed: {ex.Message}");
            }

            return result;
        }

        private void ValidateCustomerRecord(CustomerImportDto customer)
        {
            customer.ValidationErrors.Clear();

            if (string.IsNullOrWhiteSpace(customer.Name))
                customer.ValidationErrors.Add("Name is required");

            if (string.IsNullOrWhiteSpace(customer.Email))
                customer.ValidationErrors.Add("Email is required");
            else if (!IsValidEmail(customer.Email))
                customer.ValidationErrors.Add("Email format is invalid");

            if (string.IsNullOrWhiteSpace(customer.Phone))
                customer.ValidationErrors.Add("Phone is required");

            if (string.IsNullOrWhiteSpace(customer.Address))
                customer.ValidationErrors.Add("Address is required");
        }

        private async Task ValidateOrderRecord(OrderImportDto order, Dictionary<string, int> customerLookup)
        {
            order.ValidationErrors.Clear();

            // Resolve customer ID
            if (order.CustomerId.HasValue)
            {
                // Direct customer ID provided
                var customerExists = await _customerRepository.GetByIdAsync(order.CustomerId.Value);
                if (customerExists != null)
                {
                    order.ResolvedCustomerId = order.CustomerId.Value;
                }
                else
                {
                    order.ValidationErrors.Add($"Customer ID {order.CustomerId} not found");
                }
            }
            else if (!string.IsNullOrWhiteSpace(order.CustomerName))
            {
                // Look up customer by name
                if (customerLookup.TryGetValue(order.CustomerName.ToLowerInvariant(), out var customerId))
                {
                    order.ResolvedCustomerId = customerId;
                }
                else
                {
                    order.ValidationErrors.Add($"Customer '{order.CustomerName}' not found");
                }
            }
            else
            {
                order.ValidationErrors.Add("Either Customer ID or Customer Name is required");
            }

            if (!order.Quantity.HasValue)
                order.ValidationErrors.Add("Quantity is required");
            else if (order.Quantity < 1 || order.Quantity > 1000)
                order.ValidationErrors.Add("Quantity must be between 1 and 1000");

            if (!order.OrderDate.HasValue)
                order.ValidationErrors.Add("Order Date is required");
            else if (order.OrderDate > DateTime.Today)
                order.ValidationErrors.Add("Order Date cannot be in the future");
            else if (order.OrderDate < DateTime.Today.AddYears(-1))
                order.ValidationErrors.Add("Order Date cannot be more than one year ago");
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }

    public class CsvImportResult<T>
    {
        public int TotalRecords { get; set; }
        public List<T> ValidRecords { get; set; } = new List<T>();
        public List<T> InvalidRecords { get; set; } = new List<T>();
        public List<string> Errors { get; set; } = new List<string>();
        public bool HasErrors => Errors.Any() || InvalidRecords.Any();
    }

    public class ImportSaveResult
    {
        public int SuccessCount { get; set; }
        public int FailureCount { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        public bool HasErrors => Errors.Any() || FailureCount > 0;
    }
}
