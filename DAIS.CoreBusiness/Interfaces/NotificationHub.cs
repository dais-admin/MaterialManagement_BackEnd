using Microsoft.AspNetCore.SignalR;

namespace DAIS.CoreBusiness.Interfaces
{
    public class NotificationHub : Hub
    {
        public async Task SendAsync(string userId, int newCount)
        {
            await Clients.User(userId).SendAsync("ReceiveNotificationCount", newCount);
        }
    }
}