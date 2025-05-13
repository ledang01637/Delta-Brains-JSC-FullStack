using DeltaBrainsJSCAppBE.Models;
using System.ComponentModel.DataAnnotations;

namespace DeltaBrainsJSCAppBE.DTOs.Response
{
    public class TaskRes
    {
        public int Id { get; set; }
        public string? AssigneeName {  get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Updated { get; set; }

    }
}
