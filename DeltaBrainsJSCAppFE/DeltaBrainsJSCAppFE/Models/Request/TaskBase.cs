using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeltaBrainsJSCAppFE.Models.Request
{
    public class TaskBase
    {
        public int UserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
