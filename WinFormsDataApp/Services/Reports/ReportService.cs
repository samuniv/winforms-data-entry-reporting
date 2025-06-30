using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using WinFormsDataApp.Models;
using WinFormsDataApp.Reports;
using WinFormsDataApp.Repositories;
using WinFormsDataApp.Services;

namespace WinFormsDataApp.Services.Reports
{
    public class ReportService
    {
        private readonly CustomerRepository _customerRepository;
        private readonly OrderRepository _orderRepository;

        public ReportService(CustomerRepository customerRepository, OrderRepository orderRepository)
        {
            _customerRepository = customerRepository;
            _orderRepository = orderRepository;

            // Initialize QuestPDF license (MIT)
            QuestPDF.Settings.License = LicenseType.Community;
        }

        /// <summary>
        /// Generates an invoice PDF for a specific order and saves it to file
        /// </summary>
        public async Task<string> GenerateInvoiceAsync(Order order, Customer customer,
                                                      string? outputPath = null, bool openFile = true)
        {
            try
            {
                LoggingService.LogInformation($"Generating invoice for Order {order.Id}, Customer {customer.Name}");

                var document = new InvoiceDocument(order, customer);

                // Generate output path if not provided
                if (string.IsNullOrEmpty(outputPath))
                {
                    var fileName = $"Invoice_{order.Id}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
                    outputPath = Path.Combine(GetReportsDirectory(), fileName);
                }

                // Generate PDF
                await Task.Run(() => document.GeneratePdf(outputPath));

                LoggingService.LogInformation($"Invoice generated successfully: {outputPath}");

                // Open file if requested
                if (openFile)
                {
                    OpenPdfFile(outputPath);
                }

                return outputPath;
            }
            catch (Exception ex)
            {
                LoggingService.LogError(ex, $"Failed to generate invoice for Order {order.Id}");
                throw;
            }
        }

        /// <summary>
        /// Generates a summary report PDF for a date range
        /// </summary>
        public async Task<string> GenerateSummaryReportAsync(IEnumerable<Order> orders, IEnumerable<Customer> customers,
                                                            DateTime startDate, DateTime endDate,
                                                            string? outputPath = null, bool openFile = true)
        {
            try
            {
                LoggingService.LogInformation($"Generating summary report for period {startDate:MM/dd/yyyy} - {endDate:MM/dd/yyyy}");

                var document = new SummaryDocument(orders.ToList(), customers.ToList(), startDate, endDate);

                // Generate output path if not provided
                if (string.IsNullOrEmpty(outputPath))
                {
                    var fileName = $"Summary_Report_{startDate:yyyyMMdd}_to_{endDate:yyyyMMdd}_{DateTime.Now:HHmmss}.pdf";
                    outputPath = Path.Combine(GetReportsDirectory(), fileName);
                }

                // Generate PDF
                await Task.Run(() => document.GeneratePdf(outputPath));

                LoggingService.LogInformation($"Summary report generated successfully: {outputPath}");

                // Open file if requested
                if (openFile)
                {
                    OpenPdfFile(outputPath);
                }

                return outputPath;
            }
            catch (Exception ex)
            {
                LoggingService.LogError(ex, $"Failed to generate summary report for period {startDate:MM/dd/yyyy} - {endDate:MM/dd/yyyy}");
                throw;
            }
        }

        /// <summary>
        /// Gets or creates the reports directory
        /// </summary>
        private string GetReportsDirectory()
        {
            var reportsDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "WinFormsDataApp", "Reports");
            if (!Directory.Exists(reportsDir))
            {
                Directory.CreateDirectory(reportsDir);
                LoggingService.LogInformation($"Created reports directory: {reportsDir}");
            }
            return reportsDir;
        }

        /// <summary>
        /// Generates an invoice PDF for a specific order by ID
        /// </summary>
        public async Task<byte[]> GenerateInvoicePdfAsync(int orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null)
                throw new ArgumentException($"Order with ID {orderId} not found.");

            var customer = await _customerRepository.GetByIdAsync(order.CustomerId);
            if (customer == null)
                throw new ArgumentException($"Customer with ID {order.CustomerId} not found.");

            LoggingService.LogInformation($"Generating invoice PDF for Order {orderId}");

            var document = new InvoiceDocument(order, customer);
            return document.GeneratePdf();
        }

        /// <summary>
        /// Generates a summary report PDF for a date range
        /// </summary>
        public async Task<byte[]> GenerateSummaryPdfAsync(DateTime startDate, DateTime endDate)
        {
            var orders = await _orderRepository.GetByDateRangeAsync(startDate, endDate);
            var customers = await _customerRepository.GetAllAsync();

            LoggingService.LogInformation($"Generating summary PDF for period {startDate:MM/dd/yyyy} - {endDate:MM/dd/yyyy}");

            var document = new SummaryDocument(orders, customers, startDate, endDate);
            return document.GeneratePdf();
        }

        /// <summary>
        /// Saves an invoice PDF to a specified file path
        /// </summary>
        public async Task SaveInvoicePdfAsync(int orderId, string filePath)
        {
            try
            {
                var pdfBytes = await GenerateInvoicePdfAsync(orderId);
                await File.WriteAllBytesAsync(filePath, pdfBytes);

                LoggingService.LogInformation($"Invoice PDF saved to: {filePath}");
            }
            catch (Exception ex)
            {
                LoggingService.LogError(ex, $"Failed to save invoice PDF for Order {orderId}");
                throw;
            }
        }

        /// <summary>
        /// Saves a summary report PDF to a specified file path
        /// </summary>
        public async Task SaveSummaryPdfAsync(DateTime startDate, DateTime endDate, string filePath)
        {
            try
            {
                var pdfBytes = await GenerateSummaryPdfAsync(startDate, endDate);
                await File.WriteAllBytesAsync(filePath, pdfBytes);

                LoggingService.LogInformation($"Summary PDF saved to: {filePath}");
            }
            catch (Exception ex)
            {
                LoggingService.LogError(ex, $"Failed to save summary PDF for period {startDate:MM/dd/yyyy} - {endDate:MM/dd/yyyy}");
                throw;
            }
        }

        /// <summary>
        /// Opens a PDF file with the system's default PDF viewer
        /// </summary>
        public void OpenPdfFile(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                    throw new FileNotFoundException($"PDF file not found: {filePath}");

                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = filePath,
                    UseShellExecute = true
                });

                LoggingService.LogInformation($"Opened PDF file: {filePath}");
            }
            catch (Exception ex)
            {
                LoggingService.LogError(ex, $"Failed to open PDF file: {filePath}");
                throw;
            }
        }
    }
}
