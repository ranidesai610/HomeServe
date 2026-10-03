using System.ComponentModel.DataAnnotations;

namespace HomeServiceManagementSystem.Models
{
    public class Service
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int ServiceCategoryId { get; set; }

        public ServiceCategory? ServiceCategory { get; set; }

        public ICollection<ProviderService>? ProviderServices { get; set; }

        public ICollection<Booking>? Bookings { get; set; }
    }
}