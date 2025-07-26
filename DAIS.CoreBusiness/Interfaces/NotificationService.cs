using DAIS.CoreBusiness.Interfaces;
using DAIS.DataAccess.Entities;
using DAIS.DataAccess.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace DAIS.CoreBusiness.Interfaces
{ }
public class NotificationService : INotificationService
{
    private readonly IGenericRepository<Notification> _notification;
    private readonly IHubContext<NotificationHub> _hubContext;

    public NotificationService(IHubContext<NotificationHub> hubContext,
                               IGenericRepository<Notification> notification)
    {
        _hubContext = hubContext;
        _notification = notification;
    }

    public async Task<Notification> CreateNotification(Notification notification)
    {
        await _notification.Add(notification).ConfigureAwait(false);


        var newCount = await _notification.CountWhere(n => n.UserId == notification.UserId && !n.IsRead);

        await _hubContext.Clients.User(notification.UserId).SendAsync("ReceiveNotificationCount", newCount);

        return notification;
    }

    public async Task<IEnumerable<Notification>> GetUserNotifications(string userId)
    {
        return await _notification.GetWhere(n => n.UserId == userId);
    }

    public async Task MarkAsRead(int id)
    {
        var notification = await _notification.FindAsync(id) ?? throw new Exception("Notification not found");
        notification.IsRead = true;
        await _notification.Update(notification);

        var newCount = await _notification.CountWhere(n => n.UserId == notification.UserId && !n.IsRead);
       
        await _hubContext.Clients.User(notification.UserId).SendAsync("ReceiveNotificationCount", newCount);
    }
}


