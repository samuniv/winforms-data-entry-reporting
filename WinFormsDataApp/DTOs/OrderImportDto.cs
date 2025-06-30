using CsvHelper.Configuration.Attributes;

namespace WinFormsDataApp.DTOs
{
    public class OrderImportDto
    {
        [Name("customer_id", "customerId")]
        public int? CustomerId { get; set; }

        [Name("customer_name", "customer", "name")]
        public string CustomerName { get; set; } = string.Empty;

        [Name("quantity", "qty")]
        public int? Quantity { get; set; }

        [Name("order_date", "date", "orderdate")]
        public DateTime? OrderDate { get; set; }

        // For tracking import validation
        [Ignore]
        public List<string> ValidationErrors { get; set; } = new List<string>();

        [Ignore]
        public bool IsValid => !ValidationErrors.Any();

        [Ignore]
        public int? ResolvedCustomerId { get; set; }
    }
}
