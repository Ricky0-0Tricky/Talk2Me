using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Talk2Me.Models
{
    /// <summary>
    /// Represents a user of the platform.
    /// Stores the expected user info.
    /// </summary>
    public class User
    {
        [Key]
        [HiddenInput]
        public Guid UserId { get; set; } = Guid.NewGuid();

        [Display(Name = "Profile Image")]
        public byte[] ProfilePic { get; set; } = null!;

        [Display(Name = "Username")]
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = "Username can only contain letters and numbers.")]
        [StringLength(20, MinimumLength = 4, ErrorMessage = "Your username should have a minimum of 4 characters.")]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [StringLength(255, MinimumLength = 5, ErrorMessage = "The password should have a minimum of 5 characters.")]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        public string Role { get; set; } = string.Empty;

        [Display(Name = "Last Time Online")]
        public DateTimeOffset? LastTimeOnline { get; set; }

        [Display(Name = "Suspended")]
        public bool IsSuspended { get; set; }

        [Display(Name = "Suspension Type")]
        public string? SuspensionType { get; set; }

        [Display(Name = "Suspension Date")]
        public DateTimeOffset? SuspensionDate { get; set; }

        [Display(Name = "Joining Date")]
        public DateTimeOffset JoinDate { get; set; } = DateTimeOffset.UtcNow;

        // A User can have many Sent Chats (1 -> N)
        public ICollection<Chat> SentChats { get; set; } = new List<Chat>();

        // A User can have many Received Chats (1 -> N)
        public ICollection<Chat> ReceivedChats { get; set; } = new List<Chat>();

        // A User can have many Messages (1 -> N)
        public ICollection<Message> Messages { get; set; } = new List<Message>();

        // A User can have many Notifications (1 -> N)
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();

        // A User can have many Friends (1 -> N)
        public ICollection<Friend> Friends { get; set; } = new List<Friend>();

        // A User can have many Sent Friend Requests (1 -> N)
        public ICollection<FriendRequest> SentFriendRequests { get; set; } = new List<FriendRequest>();

        // A User can have many Received Friend Requests (1 -> N)
        public ICollection<FriendRequest> ReceivedFriendRequests { get; set; } = new List<FriendRequest>();

        // A User can have many Forums (1 -> N)
        public ICollection<Forum> Forums { get; set; } = new List<Forum>();

        // A User can have many Reactions (1 -> N)
        public ICollection<Reaction> Reactions { get; set; } = new List<Reaction>();
    }
}
