using System.ComponentModel.DataAnnotations;

namespace DeltaBrainsJSCAppBE.Models
{
    public class DeviceSession
    {
        [Key]
        public int SessionId { get; set; }
        [Required]
        public int UserId { get; set; }
        public string? ConnectionId { get; set; }
        public bool IsConnected { get; set;}
        public DateTime UpdatedAt { get; set; }

        public virtual User? User { get; set; }
    }
}
