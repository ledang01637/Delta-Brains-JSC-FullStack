
using System.ComponentModel.DataAnnotations;

namespace DeltaBrainsJSCAppBE.Models
{
    public class Task
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int AssignedTo { get ; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public Enum.TaskStatus? Status { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Updated { get; set;}

        public virtual User? User {  get; set; }
    }
}
