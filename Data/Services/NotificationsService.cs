using Microsoft.EntityFrameworkCore;
using Talk2Me.Data.Interfaces;
using Talk2Me.Models;
namespace Talk2Me.Data.Services
{
    public class NotificationsService : INotificationsService
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Notifications Service Constructor. 
        /// Initializes the service with the provided AppDbContext.
        /// </summary>
        /// <param name="context">App's Database Context</param>
        public NotificationsService(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtains all of the user's notifications from the database.
        /// </summary>
        public async Task<IEnumerable<Notification>> GetAllNotifications(string userId)
        {
            var notifications = await _context.Notifications
                .Include(n => n.User)
                .Where(n => n.UserId.ToString() == userId && n.IsRead == false)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
            return notifications;
        }

        /// <summary>
        /// Obtains a notification from the database based on the provided ID.
        /// </summary>
        /// <param name="notificationId">Notification's ID</param>
        public async Task<Notification?> GetNotificationById(Guid notificationId)
        {
            var notification = _context.Notifications.FirstOrDefault(n => n.NotificationId == notificationId);
            return notification;
        }

        /// <summary>
        /// Marks a notification as read in the database based on the provided ID.
        /// </summary>
        /// <param name="notificationId">Notification's ID</param>
        public async Task<Notification> SeeNotification(Guid notificationId)
        {
            var notification = await _context.Notifications.FindAsync(notificationId);
            if (notification != null)
            {
                notification.IsRead = true;
                await _context.SaveChangesAsync();
            }
            return notification;
        }

        /// <summary>
        /// Creates a new notification in the database. 
        /// The provided Notification object is added to the context and changes are saved asynchronously.
        /// </summary>
        /// <param name="notification">Notification Object</param>
        public async Task CreateNotification(Notification notification)
        {
            _context.Add(notification);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes a notification from the database based on the provided ID. 
        /// If the notification is found, it is removed from the context and changes are saved to the database.
        /// </summary>
        /// <param name="notificationId">Notification's ID</param>
        public async Task DeleteNotification(Guid notificationId)
        {
            var notification = await _context.Notifications.FindAsync(notificationId);
            if (notification != null)
            {
                _context.Notifications.Remove(notification);
            }
            await _context.SaveChangesAsync();
        }
    }
}
