using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Talk2Me.Models
{
    /// <summary>
    /// Represents a ticket submited by a User.
    /// Stores its title, description, status, relevant dates and the user's .
    /// </summary>
    public class Ticket
    {
        [Key]
        [HiddenInput]
        public Guid TicketId { get; set; } = Guid.NewGuid();

        [Required(ErrorMessage = "The Ticket's Title is Required.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "The Ticket's Description is Required.")]
        public string Description { get; set; }

        public string Status { get; set; } = "Open";

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
            
        public DateTimeOffset? ResolvedAt { get; set; }

        // A Ticket can have many Ticket Photos (1 -> N)
        public ICollection<TicketPhoto> Photos { get; set; } = new List<TicketPhoto>();

        // Many Tickets belong to an Admin
        public Guid AdminID { get; set; }
        
        public User Admin { get; set; } = null!;

        // Many Tickets belong to a User    
        public Guid UserId { get; set; }

        public User User { get; set; }

    }
}
