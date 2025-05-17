
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeltaBrainsJSCAppBE.Models
{
    public class Task
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get ; set; }
        public int AssignedBy { get ; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public bool IsCurrent {  get; set; }

        [Range(0, 2)]
        public Enum.TaskStatus Status { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Updated { get; set;}

        [ForeignKey(nameof(UserId))]
        public virtual User? Assignee { get; set; }

        [ForeignKey(nameof(AssignedBy))]
        public virtual User? AssignedByUser { get; set; }

    }
}
