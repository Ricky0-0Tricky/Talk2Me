using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Talk2Me.Models
{
    public class Theme
    {
        [Key]
        [HiddenInput]
        public Guid ThemeId { get; set; }

        [Display(Name = "Theme")]
        [Required(ErrorMessage = "The Theme's Name is Required.")]
        public string ThemeName { get; set; } = string.Empty;

        [Display(Name = "Creation Date")]
        public DateTimeOffset CreationDate = DateTimeOffset.UtcNow;
    }
}
