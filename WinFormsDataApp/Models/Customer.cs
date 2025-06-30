using System.ComponentModel.DataAnnotations;

namespace WinFormsDataApp.Models
{
    public class Customer
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;

        [MaxLength(20)]
        public string? Phone { get; set; }

        [MaxLength(200)]
        public string? Address { get; set; }

        // Navigation property
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
