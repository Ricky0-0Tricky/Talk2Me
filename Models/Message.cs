using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Talk2Me.Models
{
    public class Message
    {
        [Key]
        [HiddenInput]
        public Guid MessageId { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string Content { get; set; } = string.Empty;

        [Display(Name = "Sent Date")]
        public DateTimeOffset SendDate { get; set; } = DateTimeOffset.Now;

        [Display(Name = "Read Date")]
        public DateTimeOffset ReadDate { get; set; } 
    }
}
