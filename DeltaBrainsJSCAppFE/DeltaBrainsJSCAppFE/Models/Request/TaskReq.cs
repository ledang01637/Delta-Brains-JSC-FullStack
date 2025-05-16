using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeltaBrainsJSCAppFE.Models.Request
{
    public class TaskReq : TaskBase
    {
        public int AssignedBy { get; set; }
    }
}
