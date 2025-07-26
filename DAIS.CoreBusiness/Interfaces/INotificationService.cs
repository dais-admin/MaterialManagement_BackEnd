using DAIS.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAIS.CoreBusiness.Interfaces
{
    public interface INotificationService
    {
        Task<Notification> CreateNotification(Notification notification);
        Task<IEnumerable<Notification>> GetUserNotifications(string userId);
        Task MarkAsRead(int id);
    }
}
