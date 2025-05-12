using System.ComponentModel.DataAnnotations;

namespace DeltaBrainsJSCAppBE.Models
{
    public class Notification
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
        public string? Message { get; set; }
        public DateTime CreatedAt {  get; set; }

        public virtual User? User { get; set; }
    }
}
