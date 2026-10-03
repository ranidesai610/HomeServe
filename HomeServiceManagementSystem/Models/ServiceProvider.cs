using System.ComponentModel.DataAnnotations;

namespace HomeServiceManagementSystem.Models
{
    public class ServiceProvider
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        public User? User { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public string Address { get; set; }

        public bool IsApproved { get; set; }

        public bool IsAvailable { get; set; }

        public ICollection<ProviderService>? ProviderServices { get; set; }

        public ICollection<ProviderAvailability>? ProviderAvailabilities { get; set; }

        public ICollection<Booking>? Bookings { get; set; }
    }
}