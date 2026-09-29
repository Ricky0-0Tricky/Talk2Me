using Talk2Me.Models;

namespace Talk2Me.Data.Interfaces
{
    public interface INotificationsService
    {
        Task<IEnumerable<Notification>> GetAllNotifications(string userId);
        Task<Notification?> GetNotificationById(Guid notificationId);
        Task<Notification> SeeNotification(Guid notificationId);
        Task CreateNotification(Notification notification);
        Task DeleteNotification(Guid notificationId);
    }
}
