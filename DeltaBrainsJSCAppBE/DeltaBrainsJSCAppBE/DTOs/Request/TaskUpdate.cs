using System.ComponentModel.DataAnnotations;

namespace DeltaBrainsJSCAppBE.DTOs.Request
{
    public class TaskUpdate
    {
        public int Id { get; set; }
        public int AssignedTo { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Status{ get; set; }
    }
}
