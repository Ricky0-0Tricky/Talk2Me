using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Talk2Me.Models
{
    /// <summary>
    /// Represents a theme a forum can have.
    /// Stores its name and creation date.
    /// </summary>
    public class Theme
    {
        [Key]
        [HiddenInput]
        public Guid ThemeId { get; set; } = Guid.NewGuid();

        [Display(Name = "Theme")]
        [Required(ErrorMessage = "The Theme's Name is Required.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "The Theme's Name must be between 3 and 100 characters.")]
        public string ThemeName { get; set; } = string.Empty;

        [Display(Name = "Creation Date")]
        public DateTimeOffset CreationDate { get; set; } = DateTimeOffset.UtcNow;

        // A Theme can have many Forums (1 -> N)
        public ICollection<Forum> Forums { get; set; } = new List<Forum>();
    }
}
