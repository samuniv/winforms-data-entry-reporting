using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using WinFormsDataApp.Models;

namespace WinFormsDataApp.Reports
{
    public class SummaryDocument : IDocument
    {
        private readonly List<Order> _orders;
        private readonly List<Customer> _customers;
        private readonly DateTime _startDate;
        private readonly DateTime _endDate;

        public SummaryDocument(List<Order> orders, List<Customer> customers, DateTime startDate, DateTime endDate)
        {
            _orders = orders ?? throw new ArgumentNullException(nameof(orders));
            _customers = customers ?? throw new ArgumentNullException(nameof(customers));
            _startDate = startDate;
            _endDate = endDate;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public DocumentSettings GetSettings() => DocumentSettings.Default;

        public void Compose(IDocumentContainer container)
        {
            container
                .Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(40);

                    page.Header().Element(ComposeHeader);
                    page.Content().Element(ComposeContent);
                    page.Footer().Element(ComposeFooter);
                });
        }

        private void ComposeHeader(IContainer container)
        {
            container.Row(row =>
            {
                row.RelativeItem().Column(column =>
                {
                    column.Item().Text("SALES SUMMARY REPORT")
                          .FontSize(24)
                          .Bold()
                          .FontColor(Colors.Green.Medium);

                    column.Item().Text($"Period: {_startDate:MM/dd/yyyy} - {_endDate:MM/dd/yyyy}")
                          .FontSize(14)
                          .SemiBold();

                    column.Item().Text($"Generated: {DateTime.Now:MMMM dd, yyyy}")
                          .FontSize(12);
                });

                row.ConstantItem(150).Column(column =>
                {
                    var totalOrders = _orders.Count;
                    var totalQuantity = _orders.Sum(o => o.Quantity);
                    var totalValue = totalQuantity * 10m; // Sample calculation

                    column.Item().Border(1).Padding(8).Column(innerColumn =>
                    {
                        innerColumn.Item().Text("Summary Stats")
                                   .Bold()
                                   .FontSize(12);

                        innerColumn.Item().Text($"Total Orders: {totalOrders}");
                        innerColumn.Item().Text($"Total Quantity: {totalQuantity}");
                        innerColumn.Item().Text($"Total Value: ${totalValue:F2}");
                    });
                });
            });
        }

        private void ComposeContent(IContainer container)
        {
            container.PaddingVertical(20).Column(column =>
            {
                // Key Metrics
                column.Item().Element(ComposeMetrics);

                column.Item().PaddingVertical(10);

                // Customer Summary
                column.Item().Element(ComposeCustomerSummary);

                column.Item().PaddingVertical(10);

                // Order Summary Table
                column.Item().Element(ComposeOrderSummary);
            });
        }

        private void ComposeMetrics(IContainer container)
        {
            var totalOrders = _orders.Count;
            var totalQuantity = _orders.Sum(o => o.Quantity);
            var avgOrderSize = totalOrders > 0 ? (double)totalQuantity / totalOrders : 0;
            var activeCustomers = _orders.Select(o => o.CustomerId).Distinct().Count();

            container.Row(row =>
            {
                row.RelativeItem().Border(1).Padding(10).Column(column =>
                {
                    column.Item().Text("Total Orders")
                          .Bold()
                          .FontSize(12);
                    column.Item().Text(totalOrders.ToString())
                          .FontSize(18)
                          .Bold()
                          .FontColor(Colors.Blue.Medium);
                });

                row.Spacing(10);

                row.RelativeItem().Border(1).Padding(10).Column(column =>
                {
                    column.Item().Text("Total Quantity")
                          .Bold()
                          .FontSize(12);
                    column.Item().Text(totalQuantity.ToString())
                          .FontSize(18)
                          .Bold()
                          .FontColor(Colors.Green.Medium);
                });

                row.Spacing(10);

                row.RelativeItem().Border(1).Padding(10).Column(column =>
                {
                    column.Item().Text("Avg Order Size")
                          .Bold()
                          .FontSize(12);
                    column.Item().Text($"{avgOrderSize:F1}")
                          .FontSize(18)
                          .Bold()
                          .FontColor(Colors.Orange.Medium);
                });

                row.Spacing(10);

                row.RelativeItem().Border(1).Padding(10).Column(column =>
                {
                    column.Item().Text("Active Customers")
                          .Bold()
                          .FontSize(12);
                    column.Item().Text(activeCustomers.ToString())
                          .FontSize(18)
                          .Bold()
                          .FontColor(Colors.Purple.Medium);
                });
            });
        }

        private void ComposeCustomerSummary(IContainer container)
        {
            var customerOrderCounts = _orders
                .GroupBy(o => o.CustomerId)
                .Select(g => new
                {
                    CustomerId = g.Key,
                    CustomerName = _customers.FirstOrDefault(c => c.Id == g.Key)?.Name ?? "Unknown",
                    OrderCount = g.Count(),
                    TotalQuantity = g.Sum(o => o.Quantity)
                })
                .OrderByDescending(x => x.TotalQuantity)
                .Take(5)
                .ToList();

            container.Column(column =>
            {
                column.Item().Text("Top 5 Customers by Volume")
                      .Bold()
                      .FontSize(14);

                column.Item().PaddingTop(5).Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(3);   // Customer Name
                        columns.ConstantColumn(80);  // Orders
                        columns.ConstantColumn(80);  // Quantity
                        columns.ConstantColumn(100); // Value
                    });

                    // Header
                    table.Header(header =>
                    {
                        header.Cell().Element(CellStyle).Text("Customer");
                        header.Cell().Element(CellStyle).Text("Orders");
                        header.Cell().Element(CellStyle).Text("Quantity");
                        header.Cell().Element(CellStyle).Text("Value");

                        static IContainer CellStyle(IContainer container)
                        {
                            return container
                                .DefaultTextStyle(x => x.Bold())
                                .PaddingVertical(5)
                                .BorderBottom(1)
                                .BorderColor(Colors.Black);
                        }
                    });

                    // Data rows
                    foreach (var customer in customerOrderCounts)
                    {
                        var value = customer.TotalQuantity * 10m;

                        table.Cell().Element(CellStyle).Text(customer.CustomerName);
                        table.Cell().Element(CellStyle).Text(customer.OrderCount.ToString());
                        table.Cell().Element(CellStyle).Text(customer.TotalQuantity.ToString());
                        table.Cell().Element(CellStyle).Text($"${value:F2}");
                    }

                    static IContainer CellStyle(IContainer container)
                    {
                        return container
                            .BorderBottom(1)
                            .BorderColor(Colors.Grey.Lighten2)
                            .PaddingVertical(3);
                    }
                });
            });
        }

        private void ComposeOrderSummary(IContainer container)
        {
            var ordersToShow = _orders
                .OrderByDescending(o => o.OrderDate)
                .Take(10)
                .ToList();

            container.Column(column =>
            {
                column.Item().Text("Recent Orders")
                      .Bold()
                      .FontSize(14);

                column.Item().PaddingTop(5).Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.ConstantColumn(60);  // Order ID
                        columns.RelativeColumn(2);   // Customer
                        columns.ConstantColumn(80);  // Date
                        columns.ConstantColumn(60);  // Quantity
                        columns.ConstantColumn(80);  // Value
                    });

                    // Header
                    table.Header(header =>
                    {
                        header.Cell().Element(CellStyle).Text("Order");
                        header.Cell().Element(CellStyle).Text("Customer");
                        header.Cell().Element(CellStyle).Text("Date");
                        header.Cell().Element(CellStyle).Text("Qty");
                        header.Cell().Element(CellStyle).Text("Value");

                        static IContainer CellStyle(IContainer container)
                        {
                            return container
                                .DefaultTextStyle(x => x.Bold())
                                .PaddingVertical(5)
                                .BorderBottom(1)
                                .BorderColor(Colors.Black);
                        }
                    });

                    // Data rows
                    foreach (var order in ordersToShow)
                    {
                        var customer = _customers.FirstOrDefault(c => c.Id == order.CustomerId);
                        var value = order.Quantity * 10m;

                        table.Cell().Element(CellStyle).Text(order.Id.ToString());
                        table.Cell().Element(CellStyle).Text(customer?.Name ?? "Unknown");
                        table.Cell().Element(CellStyle).Text(order.OrderDate.ToString("MM/dd/yy"));
                        table.Cell().Element(CellStyle).Text(order.Quantity.ToString());
                        table.Cell().Element(CellStyle).Text($"${value:F2}");
                    }

                    static IContainer CellStyle(IContainer container)
                    {
                        return container
                            .BorderBottom(1)
                            .BorderColor(Colors.Grey.Lighten2)
                            .PaddingVertical(3);
                    }
                });
            });
        }

        private void ComposeFooter(IContainer container)
        {
            container.AlignCenter().Text(text =>
            {
                text.DefaultTextStyle(style => style.FontSize(9));
                text.Span("Sales Summary Report • Generated on ");
                text.Span($"{DateTime.Now:MMMM dd, yyyy 'at' h:mm tt}");
                text.Span(" • Page ");
                text.CurrentPageNumber();
                text.Span(" of ");
                text.TotalPages();
            });
        }
    }
}
