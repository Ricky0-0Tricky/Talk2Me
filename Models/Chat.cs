using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Talk2Me.Models
{
    public class Chat
    {
        [Key]
        [HiddenInput]
        public Guid ChatId { get; set; } = Guid.NewGuid();

        [Required]
        public string State { get; set; } = "Active";

        // A Chat can have many Messages (1 -> N)
        public ICollection<Message> Messages { get; set; } = new List<Message>();
    }
}
