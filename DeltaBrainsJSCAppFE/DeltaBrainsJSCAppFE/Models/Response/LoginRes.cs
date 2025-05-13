using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeltaBrainsJSCAppFE.Models.Response
{
    public class LoginRes
    {
        public int Code { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public DataRes Data { get; set; }
    }

    public class DataRes
    {
        public bool SuccsessFull { get; set; }
        public string Error { get; set; }
        public string Token { get; set; }
        public string Expiration { get; set; }
    }

}
