using System.ComponentModel.DataAnnotations;

namespace HomeServiceManagementSystem.Models
{
    public class ProviderService
    {
        public int Id { get; set; }

        [Required]
        public int ServiceProviderId { get; set; }

        public ServiceProvider? ServiceProvider { get; set; }

        [Required]
        public int ServiceId { get; set; }

        public Service? Service { get; set; }
    }
}