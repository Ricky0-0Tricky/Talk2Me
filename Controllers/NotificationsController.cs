using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using System.Security.Claims;
using Talk2Me.Data;
using Talk2Me.Data.Interfaces;
using Talk2Me.Models;

namespace Talk2Me.Controllers
{
    [Authorize]
    public class NotificationsController : Controller
    {
        /// <summary>
        /// Notifications Service used to manage Notifications in the database.
        /// </summary>
        private readonly INotificationsService _notificationsService;

        /// <summary>
        /// Accounts Service used to manage Accounts in the database.
        /// </summary>
        private readonly IAccountsService _accountsService;

        /// <summary>
        /// Notifications Controller Constructor.
        /// Initializes the controller with the provided INotificationsService and IAccountService.
        public NotificationsController(INotificationsService notificationsService, IAccountsService accountsService)
        {
            _notificationsService = notificationsService;
            _accountsService = accountsService;
        }

        /// <summary>
        /// Shows the Notifications Index View.
        /// </summary>
        public async Task<IActionResult> Index()
        {
            var notifications = await _notificationsService.GetAllNotifications(User.FindFirstValue(ClaimTypes.NameIdentifier));
            return View(notifications);
        }

        /// <summary>
        /// Shows the Notifications Create View.
        /// </summary>
        public async Task<IActionResult> Create()
        {
            ViewData["Type"] = new SelectList(new List<SelectListItem>
            {
                new SelectListItem { Value = "Warning", Text = "Warning"}, 
                new SelectListItem { Value = "Info", Text = "Info" }, 
                new SelectListItem { Value = "Success", Text = "Success" }
            }, "Value", "Text");
            ViewData["UserId"] = new SelectList(await _accountsService.GetAllUsers(), "UserId", "UserName");
            return View();
        }

        /// <summary>
        /// Sets a Notification as read and redirects to the Notifications Index View.
        /// </summary>
        /// <param name="id">Notification's ID</param>
        [HttpPost]
        public async Task<IActionResult> SeeNotification(Guid id)
        {
            var notification = await _notificationsService.SeeNotification(id);

            if (notification == null)
            {
                return NotFound();
            }

            return Ok();
        }


        /// <summary>
        /// Creates a new Notification and redirects to the Notifications Index View.
        /// </summary>
        /// <param name="notification">Notification Object</param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NotificationId,Type,Content,CreatedAt,UserId")] Notification notification)
        {
            var user = await _accountsService.GetUserById(notification.UserId);
            notification.User = user;

            ModelState.Remove(nameof(Notification.User));
            if (ModelState.IsValid)
            {
                notification.NotificationId = Guid.NewGuid();
                notification.IsRead = false;
                notification.CreatedAt = DateTimeOffset.UtcNow;
                await _notificationsService.CreateNotification(notification);
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(await _accountsService.GetAllUsers(), "UserId", "UserName");
            return View(notification);
        }

        /// <summary>
        /// Deletes a Notification and redirects to the Notifications Index View.
        /// </summary>
        /// <param name="id">Notification's ID</param>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _notificationsService.DeleteNotification(id);
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Checks if a Notification exists by its ID.
        /// </summary>
        /// <param name="id">Notification's ID</param>
        /// <returns></returns>
        private bool NotificationExists(Guid id)
        {
            return _notificationsService.GetNotificationById(id) != null;
        }
    }
}
