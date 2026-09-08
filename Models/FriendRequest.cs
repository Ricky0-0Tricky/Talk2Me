using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Talk2Me.Models
{
    public class FriendRequest
    {
        [Key]
        [HiddenInput]
        public Guid RequestId { get; set; } = Guid.NewGuid();

        [Required]
        public string Status { get; set; } = "Pending";

        public string? Message { get; set; }

        [Display(Name = "Request Date")]
        public DateTimeOffset RequestDate { get; set; } = DateTimeOffset.UtcNow;

        // User sending the request
        [Required]
        public Guid SenderId { get; set; }

        public User Sender { get; set; } = null!;

        // User receiving the request
        [Required]
        public Guid ReceiverId { get; set; }

        public User Receiver { get; set; } = null!;
    }
}
