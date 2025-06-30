using CsvHelper.Configuration.Attributes;

namespace WinFormsDataApp.DTOs
{
    public class CustomerImportDto
    {
        [Name("name", "customer_name", "full_name")]
        public string Name { get; set; } = string.Empty;

        [Name("email", "email_address")]
        public string Email { get; set; } = string.Empty;

        [Name("phone", "phone_number", "telephone")]
        public string Phone { get; set; } = string.Empty;

        [Name("address", "customer_address", "full_address")]
        public string Address { get; set; } = string.Empty;

        // For tracking import validation
        [Ignore]
        public List<string> ValidationErrors { get; set; } = new List<string>();

        [Ignore]
        public bool IsValid => !ValidationErrors.Any();
    }
}
