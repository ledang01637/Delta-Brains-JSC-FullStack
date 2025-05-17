using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeltaBrainsJSCAppFE.Models.Response
{
    public class LoginRes
    {
        public bool SuccsessFull { get; set; }
        public string Error { get; set; }
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }

}
