using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Talk2Me.Models
{
    /// <summary>
    /// Represents a friend of a user.
    /// Stores the favorite/blocked state and block date.
    /// </summary>
    public class Friend
    {
        [Key]
        [HiddenInput]
        public Guid FriendId { get; set; }

        [Display(Name = "Favorite?")]
        public bool IsFavorite { get; set; }

        [Display(Name = "Blocked?")]
        public bool IsBlocked { get; set; }

        [Display(Name = "Block Date")]
        public DateTimeOffset? BlockedDate { get; set; } = null;

        // The User itself
        [Required]
        public Guid UserId { get; set; }

        public User User { get; set; } = null!;

        // The User's friend
        [Required]
        public Guid FriendUserId { get; set; }

        public User FriendUser { get; set; } = null!;
    }
}
