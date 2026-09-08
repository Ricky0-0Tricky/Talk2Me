using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Talk2Me.Models
{
    public class Notification
    {
        [Key]
        [HiddenInput]
        public Guid NotificationId { get; set; }

        [Required]
        public string Type { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "The notification's content should have a minimum of 5 characters.")]
        public string Content { get; set; } = string.Empty;

        [Display(Name = "Is Read")]
        public bool IsRead { get; set; } = false;

        [Display(Name = "Creation Date")]
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    }
}
