namespace DeltaBrainsJSCAppBE.DTOs.Request
{
    public class TaskReq
    {
        public int UserId { get; set; }
        public int AssignedBy { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }

    }
}
