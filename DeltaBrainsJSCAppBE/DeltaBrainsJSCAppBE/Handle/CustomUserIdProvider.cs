using Microsoft.AspNetCore.SignalR;
using System.Diagnostics;
using System.Security.Claims;
using System.Xml.Linq;

namespace DeltaBrainsJSCAppBE.Handle
{
    public class CustomUserIdProvider : IUserIdProvider
    {
        //Ko chạy đc @@
        public CustomUserIdProvider()
        {
            Debug.WriteLine("CustomUserIdProvider Initialized"); 
        }

        public string GetUserId(HubConnectionContext connection)
        {
            var userId = connection.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Debug.WriteLine("User ID: " + userId);
            return userId;
        }
    }
}
