using System.ComponentModel.DataAnnotations;

namespace DeltaBrainsJSCAppBE.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int RoleId {  get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        [EmailAddress]
        public string? Email {  get; set; }
        [Required]
        public string? Password { get; set; }
        public string? DeviceToken {  get; set; }
        public ICollection<Notification>? Notifications {  get; set; }
        public ICollection<Task>? AssignedTasks { get; set; }
        public ICollection<Task>? CreatedTasks { get; set; }
        public virtual Role? Role {  get; set; }
    }
}
