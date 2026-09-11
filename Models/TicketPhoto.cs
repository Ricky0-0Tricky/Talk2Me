using System.ComponentModel.DataAnnotations;

namespace Talk2Me.Models
{
    /// <summary>
    /// Represents a photo from a submited ticket.
    /// Stores the image itself.
    /// </summary>
    public class TicketPhoto
    {
        public Guid TicketPhotoId { get; set; } = Guid.NewGuid();

        [Required]
        public byte[] Image { get; set; }

        // Many TicketPhotos belong to a Ticket
        public Guid TicketId { get; set; }
       
        public Ticket Ticket { get; set; } = null!;
    }
}
