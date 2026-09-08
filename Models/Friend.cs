using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Talk2Me.Models
{
    public class Friend
    {
        [Key]
        [HiddenInput]
        public Guid FriendId { get; set; }

        public bool IsFavorite { get; set; }

        public bool IsBlocked { get; set; }

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
