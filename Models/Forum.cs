using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Talk2Me.Models
{
    public class Forum
    {
        [Key]
        [HiddenInput]
        public Guid ForumId { get; set; } = Guid.NewGuid();

        [Required(ErrorMessage = "The Forum's State is Required.")]
        public string State { get; set; } = "Active";

        [Display(Name = "Creation Date")]
        public DateTimeOffset CreationDate = DateTimeOffset.UtcNow;

        // Many Forums belong to a Theme
        [Required]
        public Guid ThemeId { get; set; }

        public Theme Theme { get; set; } = null!;
    }
}
