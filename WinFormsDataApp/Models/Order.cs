using System.ComponentModel.DataAnnotations;

namespace WinFormsDataApp.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Range(1, 1000)]
        public int Quantity { get; set; }

        [Required]
        public DateTime OrderDate { get; set; } = DateTime.Now;

        // Foreign key
        [Required]
        public int CustomerId { get; set; }

        // Navigation property
        public virtual Customer Customer { get; set; } = null!;

        // Soft delete flag
        public bool IsDeleted { get; set; } = false;
    }
}
