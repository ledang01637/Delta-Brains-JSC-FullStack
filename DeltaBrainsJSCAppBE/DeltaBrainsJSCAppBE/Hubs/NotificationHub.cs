using DeltaBrainsJSCAppBE.DTOs.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace DeltaBrainsJSCAppBE.Hubs
{
    public class NotificationHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }
        public async Task SendTaskAssigned(NotificationRes notificationRes)
        {
            await Clients.All.SendAsync("SendTaskAssigned", notificationRes);
        }

        public async Task SendTaskUpdate(string message)
        {
            await Clients.All.SendAsync("TaskUpdate", message);
        }
    }
}
