using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Talk2Me.ViewModels
{
    public class SettingsViewModel
    {
        [Display(Name = "Profile Picture")]
        public IFormFile? ProfilePic { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "New Password")]
        [StringLength(255, MinimumLength = 5, ErrorMessage = "Password must contain at least 5 characters.")]
        [DataType(DataType.Password)]
        public string? NewPassword { get; set; }

        [Display(Name = "Confirm New Password")]
        [Compare(nameof(NewPassword), ErrorMessage = "The passwords do not match.")]
        [DataType(DataType.Password)]
        public string? ConfirmNewPassword { get; set; }
    }
}