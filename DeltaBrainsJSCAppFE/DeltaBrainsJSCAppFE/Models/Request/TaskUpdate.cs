using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeltaBrainsJSCAppFE.Models.Request
{
    public class TaskUpdate : TaskBase
    {
        public int Id { get; set; }
        public int AssignedBy { get; set; }
    }
}
