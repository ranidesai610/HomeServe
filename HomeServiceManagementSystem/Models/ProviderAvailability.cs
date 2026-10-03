using System.ComponentModel.DataAnnotations;

namespace HomeServiceManagementSystem.Models
{
    public class ProviderAvailability
    {
        public int Id { get; set; }

        [Required]
        public int ServiceProviderId { get; set; }

        public ServiceProvider? ServiceProvider { get; set; }

        [Required]
        public DateTime AvailableDate { get; set; }

        [Required]
        public string StartTime { get; set; }

        [Required]
        public string EndTime { get; set; }

        public bool IsAvailable { get; set; }
    }
}