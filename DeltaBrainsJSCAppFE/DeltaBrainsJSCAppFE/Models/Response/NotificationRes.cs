using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeltaBrainsJSCAppFE.Models.Response
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
