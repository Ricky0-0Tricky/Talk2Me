using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Talk2Me.ViewModels
{
    public class SignUpViewModel
    {
        [Display(Name = "Profile Picture")]
        [Required(ErrorMessage = "Please select a profile picture.")]
        public IFormFile ProfilePic { get; set; } = null!;

        [Display(Name = "Username")]
        [Required(ErrorMessage = "Username is required.")]
        [RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = "Username can only contain letters and numbers.")]
        [StringLength(20, MinimumLength = 4, ErrorMessage = "Your username should have between 4 and 20 characters.")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(255, MinimumLength = 5, ErrorMessage = "The password should have a minimum of 5 characters.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please confirm your password.")]
        [Compare("Password", ErrorMessage = "The passwords do not match.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}