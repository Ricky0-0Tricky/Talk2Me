using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Talk2Me.Models
{
    /// <summary>
    /// Represents a notification destined to a user.
    /// Stores its type, content, read state and the creation date.
    /// </summary>
    public class Notification
    {
        [Key]
        [HiddenInput]
        public Guid NotificationId { get; set; } = Guid.NewGuid();

        [Required]
        public string Type { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "The notification's content should have a minimum of 5 characters.")]
        public string Content { get; set; } = string.Empty;

        [Display(Name = "Is Read")]
        public bool IsRead { get; set; } = false;

        [Display(Name = "Creation Date")]
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

        // Many Notifications belong to a User
        public Guid UserId { get; set; }

        public User User { get; set; } = null!;
    }
}
