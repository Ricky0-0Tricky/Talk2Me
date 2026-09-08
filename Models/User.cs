using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Talk2Me.Models
{
    public class User
    {
        [Key]
        [HiddenInput]
        public Guid UserId { get; set; }

        [Display(Name = "Username")]
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = "Username can only contain letters and numbers.")]
        [StringLength(20, MinimumLength = 4, ErrorMessage = "Your username should have a minimum of 4 characters.")]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [StringLength(255, MinimumLength = 5, ErrorMessage = "The password should have a minimum of 5 characters.")]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        public string Role { get; set; } = string.Empty;

        [Display(Name = "Last Time Online")]
        public DateTimeOffset? LastTimeOnline { get; set; }

        [Display(Name = "Suspended")]
        public bool IsSuspended { get; set; }

        [Display(Name = "Suspension Type")]
        public string? SuspensionType { get; set; }

        [Display(Name = "Suspension Date")]
        public DateTimeOffset? SuspensionDate { get; set; }

        [Display(Name = "Joining Date")]
        public DateTimeOffset JoinDate { get; set; } = DateTimeOffset.UtcNow;
    }
}
