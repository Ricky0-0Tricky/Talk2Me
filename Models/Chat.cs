using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Talk2Me.Models
{
    public class Chat
    {
        [Key]
        [HiddenInput]
        public Guid ChatId { get; set; }

        [Required]
        public string State { get; set; } = "Active";
    }
}
