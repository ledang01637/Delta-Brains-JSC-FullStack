using System.ComponentModel.DataAnnotations;

namespace DeltaBrainsJSCAppBE.DTOs.Response
{
    public class NotificationRes
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string? Title { get; set; }
        public string? Message { get; set; }
        public int? RelatedTaskId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
