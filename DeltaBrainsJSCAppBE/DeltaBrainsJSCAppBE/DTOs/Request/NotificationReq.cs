using DeltaBrainsJSCAppBE.Models;

namespace DeltaBrainsJSCAppBE.DTOs.Request
{
    public class NotificationReq
    {
        public string? Title { get; set; }
        public string? Message { get; set; }
        public int UserId {  get; set; }
        public int RelatedTaskId {  get; set; }
    }
}
