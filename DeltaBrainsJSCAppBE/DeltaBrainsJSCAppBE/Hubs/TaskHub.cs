using Microsoft.AspNetCore.SignalR;

namespace DeltaBrainsJSCAppBE.Hubs
{
    public class TaskHub : Hub
    {
        public async Task SendTaskAssigned(string userId, string taskTitle)
        {
            await Clients.User(userId).SendAsync("taskAssigned", taskTitle);
        }
    }
}
