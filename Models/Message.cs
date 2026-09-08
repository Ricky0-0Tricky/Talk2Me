using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Talk2Me.Models
{
    public class Message
    {
        [Key]
        [HiddenInput]
        public Guid MessageId { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string Content { get; set; } = string.Empty;

        [Display(Name = "Sent Date")]
        public DateTimeOffset SendDate { get; set; } = DateTimeOffset.UtcNow;

        [Display(Name = "Read Date")]
        public DateTimeOffset? ReadDate { get; set; } 

        // Many Messages belong to a User
        public Guid UserId { get; set; }

        public User User { get; set; } = null!;

        // Many Messages belong to a Chat
        public Guid ChatId { get; set; }

        public Chat Chat { get; set; } = null!;
    }
}
