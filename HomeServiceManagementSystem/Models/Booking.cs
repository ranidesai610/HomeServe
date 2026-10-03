using System.ComponentModel.DataAnnotations;

namespace HomeServiceManagementSystem.Models
{
    public class Booking
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        public User? User { get; set; }

        [Required]
        public int ServiceProviderId { get; set; }

        public ServiceProvider? ServiceProvider { get; set; }

        [Required]
        public int ServiceId { get; set; }

        public Service? Service { get; set; }

        [Required]
        public int AddressId { get; set; }

        public Address? Address { get; set; }

        [Required]
        public DateTime BookingDate { get; set; }

        [Required]
        public string BookingTime { get; set; }

        public string? Description { get; set; }

        [Required]
        public string Status { get; set; }

        [Required]
        public decimal TotalAmount { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public Payment? Payment { get; set; }

        public Invoice? Invoice { get; set; }

        public Review? Review { get; set; }

        public ICollection<Complaint>? Complaints { get; set; }
    }
}