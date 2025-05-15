using DeltaBrainsJSCAppBE.DTOs.Response;
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
        public async Task SendTaskAssigned(string userId, NotificationRes notificationRes)
        {
            await Clients.All.SendAsync("TaskAssigned", notificationRes);
        }

        public async Task SendGeneralNotification(string userId, string title, string message)
        {
            await Clients.User(userId).SendAsync("ReceiveNotification", new { title, message });
        }
    }
}
