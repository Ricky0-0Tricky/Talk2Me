using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Talk2Me.Models
{
    /// <summary>
    /// Represents a chat between two distinct users.
    /// Stores the current state of it and associated messages.
    /// </summary>
    public class Chat
    {
        [Key]
        [HiddenInput]
        public Guid ChatId { get; set; } = Guid.NewGuid();

        [Required]
        public string State { get; set; } = "Active";

        // Many Chats belong to a User A
        [Required]
        public Guid UserAId { get; set; }

        public User UserA { get; set; } = null!;

        // Many Chats belong to a User B
        [Required]
        public Guid UserBId { get; set; }

        public User UserB { get; set; } = null!;
        
        // A Chat can have many Messages (1 -> N)
        public ICollection<Message> Messages { get; set; } = new List<Message>();
    }
}
