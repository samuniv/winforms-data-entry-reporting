using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using WinFormsDataApp.Models;

namespace WinFormsDataApp.Reports
{
    public class InvoiceDocument : IDocument
    {
        private readonly Order _order;
        private readonly Customer _customer;

        public InvoiceDocument(Order order, Customer customer)
        {
            _order = order ?? throw new ArgumentNullException(nameof(order));
            _customer = customer ?? throw new ArgumentNullException(nameof(customer));
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
                    column.Item().Text("INVOICE")
                          .FontSize(24)
                          .Bold()
                          .FontColor(Colors.Blue.Medium);

                    column.Item().Text($"Invoice #{_order.Id}")
                          .FontSize(16)
                          .SemiBold();

                    column.Item().Text($"Date: {DateTime.Now:MMMM dd, yyyy}")
                          .FontSize(12);
                });

                row.ConstantItem(150).Column(column =>
                {
                    column.Item().Border(1).Padding(8).Column(innerColumn =>
                    {
                        innerColumn.Item().Text("Order Details")
                                   .Bold()
                                   .FontSize(12);

                        innerColumn.Item().Text($"Order ID: {_order.Id}");
                        innerColumn.Item().Text($"Order Date: {_order.OrderDate:MM/dd/yyyy}");
                        innerColumn.Item().Text($"Quantity: {_order.Quantity}");
                    });
                });
            });
        }

        private void ComposeContent(IContainer container)
        {
            container.PaddingVertical(20).Column(column =>
            {
                // Customer Information
                column.Item().Element(ComposeCustomerInfo);

                column.Item().PaddingVertical(10);

                // Order Details Table
                column.Item().Element(ComposeOrderTable);

                column.Item().PaddingVertical(10);

                // Total Section
                column.Item().Element(ComposeTotalSection);
            });
        }

        private void ComposeCustomerInfo(IContainer container)
        {
            container.Column(column =>
            {
                column.Item().Text("Bill To:")
                      .Bold()
                      .FontSize(14);

                column.Item().PaddingLeft(10).Column(innerColumn =>
                {
                    innerColumn.Item().Text(_customer.Name)
                               .SemiBold();
                    innerColumn.Item().Text(_customer.Email);
                    innerColumn.Item().Text(_customer.Phone);
                    innerColumn.Item().Text(_customer.Address);
                });
            });
        }

        private void ComposeOrderTable(IContainer container)
        {
            container.Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.ConstantColumn(50);  // Item #
                    columns.RelativeColumn(3);   // Description
                    columns.ConstantColumn(80);  // Qty
                    columns.ConstantColumn(100); // Unit Price
                    columns.ConstantColumn(100); // Total
                });

                // Header
                table.Header(header =>
                {
                    header.Cell().Element(CellStyle).Text("Item");
                    header.Cell().Element(CellStyle).Text("Description");
                    header.Cell().Element(CellStyle).Text("Qty");
                    header.Cell().Element(CellStyle).Text("Unit Price");
                    header.Cell().Element(CellStyle).Text("Total");

                    static IContainer CellStyle(IContainer container)
                    {
                        return container
                            .DefaultTextStyle(x => x.Bold())
                            .PaddingVertical(5)
                            .BorderBottom(1)
                            .BorderColor(Colors.Black);
                    }
                });

                // Sample item row (in a real application, you'd have order items)
                table.Cell().Element(CellStyle).Text("1");
                table.Cell().Element(CellStyle).Text("Product/Service");
                table.Cell().Element(CellStyle).Text(_order.Quantity.ToString());
                table.Cell().Element(CellStyle).Text("$10.00");
                table.Cell().Element(CellStyle).Text($"${_order.Quantity * 10:F2}");

                static IContainer CellStyle(IContainer container)
                {
                    return container
                        .BorderBottom(1)
                        .BorderColor(Colors.Grey.Lighten2)
                        .PaddingVertical(5);
                }
            });
        }

        private void ComposeTotalSection(IContainer container)
        {
            var subtotal = _order.Quantity * 10m; // Sample calculation
            var tax = subtotal * 0.08m;
            var total = subtotal + tax;

            container.AlignRight().Column(column =>
            {
                column.Item().Row(row =>
                {
                    row.ConstantItem(100).Text("Subtotal:");
                    row.ConstantItem(80).Text($"${subtotal:F2}").AlignRight();
                });

                column.Item().Row(row =>
                {
                    row.ConstantItem(100).Text("Tax (8%):");
                    row.ConstantItem(80).Text($"${tax:F2}").AlignRight();
                });

                column.Item().Row(row =>
                {
                    row.ConstantItem(100).Text("Total:")
                          .Bold()
                          .FontSize(14);
                    row.ConstantItem(80).Text($"${total:F2}")
                          .Bold()
                          .FontSize(14)
                          .AlignRight();
                });
            });
        }

        private void ComposeFooter(IContainer container)
        {
            container.AlignCenter().Text(text =>
            {
                text.DefaultTextStyle(style => style.FontSize(9));
                text.Span("Generated on ");
                text.Span($"{DateTime.Now:MMMM dd, yyyy 'at' h:mm tt}");
                text.Span(" • Page ");
                text.CurrentPageNumber();
                text.Span(" of ");
                text.TotalPages();
            });
        }
    }
}
