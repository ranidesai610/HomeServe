using System.ComponentModel.DataAnnotations;

namespace HomeServiceManagementSystem.Models
{
    public class Payment
    {
        public int Id { get; set; }

        [Required]
        public int BookingId { get; set; }

        public Booking? Booking { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public string PaymentMethod { get; set; }

        [Required]
        public string PaymentStatus { get; set; }

        public DateTime PaymentDate { get; set; } = DateTime.Now;
    }
}