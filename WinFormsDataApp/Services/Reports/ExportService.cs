using ClosedXML.Excel;
using WinFormsDataApp.Models;
using WinFormsDataApp.Services;

namespace WinFormsDataApp.Services.Reports
{
    public class ExportService
    {
        public async Task ExportCustomersToExcelAsync(IEnumerable<Customer> customers, string filePath)
        {
            try
            {
                LoggingService.LogInformation($"Exporting {customers.Count()} customers to Excel: {filePath}");

                using var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add("Customers");

                // Add header row
                worksheet.Cell(1, 1).Value = "ID";
                worksheet.Cell(1, 2).Value = "Name";
                worksheet.Cell(1, 3).Value = "Email";
                worksheet.Cell(1, 4).Value = "Phone";
                worksheet.Cell(1, 5).Value = "Address";

                // Style header row
                var headerRange = worksheet.Range(1, 1, 1, 5);
                headerRange.Style.Font.Bold = true;
                headerRange.Style.Fill.BackgroundColor = XLColor.LightBlue;
                headerRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thick;

                // Add data rows
                int row = 2;
                foreach (var customer in customers)
                {
                    worksheet.Cell(row, 1).Value = customer.Id;
                    worksheet.Cell(row, 2).Value = customer.Name;
                    worksheet.Cell(row, 3).Value = customer.Email;
                    worksheet.Cell(row, 4).Value = customer.Phone;
                    worksheet.Cell(row, 5).Value = customer.Address;
                    row++;
                }

                // Auto-fit columns
                worksheet.Columns().AdjustToContents();

                // Add borders to data
                if (row > 2)
                {
                    var dataRange = worksheet.Range(1, 1, row - 1, 5);
                    dataRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    dataRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                }

                workbook.SaveAs(filePath);
                LoggingService.LogInformation($"Customers exported successfully to: {filePath}");
            }
            catch (Exception ex)
            {
                LoggingService.LogError(ex, $"Failed to export customers to Excel: {filePath}");
                throw;
            }
        }

        public async Task ExportOrdersToExcelAsync(IEnumerable<Order> orders, IEnumerable<Customer> customers, string filePath)
        {
            try
            {
                LoggingService.LogInformation($"Exporting {orders.Count()} orders to Excel: {filePath}");

                var customerLookup = customers.ToDictionary(c => c.Id, c => c.Name);

                using var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add("Orders");

                // Add header row
                worksheet.Cell(1, 1).Value = "Order ID";
                worksheet.Cell(1, 2).Value = "Customer";
                worksheet.Cell(1, 3).Value = "Customer ID";
                worksheet.Cell(1, 4).Value = "Quantity";
                worksheet.Cell(1, 5).Value = "Order Date";
                worksheet.Cell(1, 6).Value = "Unit Price";
                worksheet.Cell(1, 7).Value = "Total Value";
                worksheet.Cell(1, 8).Value = "Status";

                // Style header row
                var headerRange = worksheet.Range(1, 1, 1, 8);
                headerRange.Style.Font.Bold = true;
                headerRange.Style.Fill.BackgroundColor = XLColor.LightGreen;
                headerRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thick;

                // Add data rows
                int row = 2;
                foreach (var order in orders)
                {
                    var customerName = customerLookup.GetValueOrDefault(order.CustomerId, "Unknown");
                    var unitPrice = 10.00m; // Sample price
                    var totalValue = order.Quantity * unitPrice;

                    worksheet.Cell(row, 1).Value = order.Id;
                    worksheet.Cell(row, 2).Value = customerName;
                    worksheet.Cell(row, 3).Value = order.CustomerId;
                    worksheet.Cell(row, 4).Value = order.Quantity;
                    worksheet.Cell(row, 5).Value = order.OrderDate;
                    worksheet.Cell(row, 6).Value = unitPrice;
                    worksheet.Cell(row, 7).Value = totalValue;
                    worksheet.Cell(row, 8).Value = order.IsDeleted ? "Deleted" : "Active";

                    // Format currency columns
                    worksheet.Cell(row, 6).Style.NumberFormat.Format = "$#,##0.00";
                    worksheet.Cell(row, 7).Style.NumberFormat.Format = "$#,##0.00";

                    // Format date column
                    worksheet.Cell(row, 5).Style.NumberFormat.Format = "mm/dd/yyyy";

                    // Color deleted orders
                    if (order.IsDeleted)
                    {
                        var rowRange = worksheet.Range(row, 1, row, 8);
                        rowRange.Style.Font.FontColor = XLColor.Red;
                        rowRange.Style.Font.Strikethrough = true;
                    }

                    row++;
                }

                // Auto-fit columns
                worksheet.Columns().AdjustToContents();

                // Add borders to data
                if (row > 2)
                {
                    var dataRange = worksheet.Range(1, 1, row - 1, 8);
                    dataRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    dataRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                }

                // Add totals row
                if (row > 2)
                {
                    worksheet.Cell(row, 3).Value = "TOTALS:";
                    worksheet.Cell(row, 3).Style.Font.Bold = true;

                    worksheet.Cell(row, 4).FormulaA1 = $"SUM(D2:D{row - 1})";
                    worksheet.Cell(row, 7).FormulaA1 = $"SUM(G2:G{row - 1})";

                    var totalsRange = worksheet.Range(row, 3, row, 7);
                    totalsRange.Style.Font.Bold = true;
                    totalsRange.Style.Fill.BackgroundColor = XLColor.LightYellow;
                    totalsRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thick;
                }

                workbook.SaveAs(filePath);
                LoggingService.LogInformation($"Orders exported successfully to: {filePath}");
            }
            catch (Exception ex)
            {
                LoggingService.LogError(ex, $"Failed to export orders to Excel: {filePath}");
                throw;
            }
        }

        public async Task ExportSummaryToExcelAsync(IEnumerable<Order> orders, IEnumerable<Customer> customers,
                                                   DateTime startDate, DateTime endDate, string filePath)
        {
            try
            {
                LoggingService.LogInformation($"Exporting summary report to Excel: {filePath}");

                using var workbook = new XLWorkbook();

                // Summary Statistics Sheet
                var summarySheet = workbook.Worksheets.Add("Summary");
                await CreateSummarySheet(summarySheet, orders, customers, startDate, endDate);

                // Orders Detail Sheet
                var ordersSheet = workbook.Worksheets.Add("Orders Detail");
                await CreateOrdersDetailSheet(ordersSheet, orders, customers);

                // Customer Summary Sheet
                var customerSheet = workbook.Worksheets.Add("Customer Summary");
                await CreateCustomerSummarySheet(customerSheet, orders, customers);

                workbook.SaveAs(filePath);
                LoggingService.LogInformation($"Summary report exported successfully to: {filePath}");
            }
            catch (Exception ex)
            {
                LoggingService.LogError(ex, $"Failed to export summary to Excel: {filePath}");
                throw;
            }
        }

        private async Task CreateSummarySheet(IXLWorksheet worksheet, IEnumerable<Order> orders,
                                            IEnumerable<Customer> customers, DateTime startDate, DateTime endDate)
        {
            // Title
            worksheet.Cell(1, 1).Value = "SALES SUMMARY REPORT";
            worksheet.Cell(1, 1).Style.Font.Bold = true;
            worksheet.Cell(1, 1).Style.Font.FontSize = 16;
            worksheet.Range(1, 1, 1, 4).Merge();

            // Period
            worksheet.Cell(2, 1).Value = $"Period: {startDate:MM/dd/yyyy} - {endDate:MM/dd/yyyy}";
            worksheet.Cell(2, 1).Style.Font.Bold = true;

            // Key Metrics
            worksheet.Cell(4, 1).Value = "KEY METRICS";
            worksheet.Cell(4, 1).Style.Font.Bold = true;
            worksheet.Cell(4, 1).Style.Fill.BackgroundColor = XLColor.LightBlue;

            var totalOrders = orders.Count();
            var totalQuantity = orders.Sum(o => o.Quantity);
            var totalValue = totalQuantity * 10m;
            var avgOrderSize = totalOrders > 0 ? (double)totalQuantity / totalOrders : 0;
            var activeCustomers = orders.Select(o => o.CustomerId).Distinct().Count();

            worksheet.Cell(5, 1).Value = "Total Orders:";
            worksheet.Cell(5, 2).Value = totalOrders;
            worksheet.Cell(6, 1).Value = "Total Quantity:";
            worksheet.Cell(6, 2).Value = totalQuantity;
            worksheet.Cell(7, 1).Value = "Total Value:";
            worksheet.Cell(7, 2).Value = totalValue;
            worksheet.Cell(7, 2).Style.NumberFormat.Format = "$#,##0.00";
            worksheet.Cell(8, 1).Value = "Average Order Size:";
            worksheet.Cell(8, 2).Value = avgOrderSize;
            worksheet.Cell(8, 2).Style.NumberFormat.Format = "#,##0.0";
            worksheet.Cell(9, 1).Value = "Active Customers:";
            worksheet.Cell(9, 2).Value = activeCustomers;

            // Make metrics bold
            var metricsRange = worksheet.Range(5, 1, 9, 1);
            metricsRange.Style.Font.Bold = true;

            worksheet.Columns().AdjustToContents();
        }

        private async Task CreateOrdersDetailSheet(IXLWorksheet worksheet, IEnumerable<Order> orders, IEnumerable<Customer> customers)
        {
            await ExportOrdersToWorksheet(worksheet, orders, customers);
        }

        private async Task CreateCustomerSummarySheet(IXLWorksheet worksheet, IEnumerable<Order> orders, IEnumerable<Customer> customers)
        {
            var customerSummary = orders
                .GroupBy(o => o.CustomerId)
                .Select(g => new
                {
                    CustomerId = g.Key,
                    CustomerName = customers.FirstOrDefault(c => c.Id == g.Key)?.Name ?? "Unknown",
                    OrderCount = g.Count(),
                    TotalQuantity = g.Sum(o => o.Quantity),
                    TotalValue = g.Sum(o => o.Quantity) * 10m,
                    LastOrderDate = g.Max(o => o.OrderDate)
                })
                .OrderByDescending(x => x.TotalValue)
                .ToList();

            // Headers
            worksheet.Cell(1, 1).Value = "Customer ID";
            worksheet.Cell(1, 2).Value = "Customer Name";
            worksheet.Cell(1, 3).Value = "Order Count";
            worksheet.Cell(1, 4).Value = "Total Quantity";
            worksheet.Cell(1, 5).Value = "Total Value";
            worksheet.Cell(1, 6).Value = "Last Order Date";

            var headerRange = worksheet.Range(1, 1, 1, 6);
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.Lavender;

            // Data
            int row = 2;
            foreach (var customer in customerSummary)
            {
                worksheet.Cell(row, 1).Value = customer.CustomerId;
                worksheet.Cell(row, 2).Value = customer.CustomerName;
                worksheet.Cell(row, 3).Value = customer.OrderCount;
                worksheet.Cell(row, 4).Value = customer.TotalQuantity;
                worksheet.Cell(row, 5).Value = customer.TotalValue;
                worksheet.Cell(row, 6).Value = customer.LastOrderDate;

                worksheet.Cell(row, 5).Style.NumberFormat.Format = "$#,##0.00";
                worksheet.Cell(row, 6).Style.NumberFormat.Format = "mm/dd/yyyy";

                row++;
            }

            worksheet.Columns().AdjustToContents();
        }

        private async Task ExportOrdersToWorksheet(IXLWorksheet worksheet, IEnumerable<Order> orders, IEnumerable<Customer> customers)
        {
            var customerLookup = customers.ToDictionary(c => c.Id, c => c.Name);

            // Headers
            worksheet.Cell(1, 1).Value = "Order ID";
            worksheet.Cell(1, 2).Value = "Customer";
            worksheet.Cell(1, 3).Value = "Quantity";
            worksheet.Cell(1, 4).Value = "Order Date";
            worksheet.Cell(1, 5).Value = "Value";
            worksheet.Cell(1, 6).Value = "Status";

            var headerRange = worksheet.Range(1, 1, 1, 6);
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;

            // Data
            int row = 2;
            foreach (var order in orders.OrderByDescending(o => o.OrderDate))
            {
                var customerName = customerLookup.GetValueOrDefault(order.CustomerId, "Unknown");
                var value = order.Quantity * 10m;

                worksheet.Cell(row, 1).Value = order.Id;
                worksheet.Cell(row, 2).Value = customerName;
                worksheet.Cell(row, 3).Value = order.Quantity;
                worksheet.Cell(row, 4).Value = order.OrderDate;
                worksheet.Cell(row, 5).Value = value;
                worksheet.Cell(row, 6).Value = order.IsDeleted ? "Deleted" : "Active";

                worksheet.Cell(row, 4).Style.NumberFormat.Format = "mm/dd/yyyy";
                worksheet.Cell(row, 5).Style.NumberFormat.Format = "$#,##0.00";

                if (order.IsDeleted)
                {
                    var rowRange = worksheet.Range(row, 1, row, 6);
                    rowRange.Style.Font.FontColor = XLColor.Red;
                }

                row++;
            }

            worksheet.Columns().AdjustToContents();
        }

        /// <summary>
        /// Opens an Excel file with the system's default application
        /// </summary>
        public void OpenExcelFile(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                    throw new FileNotFoundException($"Excel file not found: {filePath}");

                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = filePath,
                    UseShellExecute = true
                });

                LoggingService.LogInformation($"Opened Excel file: {filePath}");
            }
            catch (Exception ex)
            {
                LoggingService.LogError(ex, $"Failed to open Excel file: {filePath}");
                throw;
            }
        }
    }
}
