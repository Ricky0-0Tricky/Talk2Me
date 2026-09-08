using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Talk2Me.Models
{
    /// <summary>
    /// Represents a forum where users can comment and react to.
    /// Stores its current state and creation date.
    /// </summary>
    public class Forum
    {
        [Key]
        [HiddenInput]
        public Guid ForumId { get; set; } = Guid.NewGuid();

        [Required(ErrorMessage = "The Forum's State is Required.")]
        public string State { get; set; } = "Active";

        [Display(Name = "Creation Date")]
        public DateTimeOffset CreationDate { get; set; } = DateTimeOffset.UtcNow;

        // Many Forums belong to a User
        [Required]
        public Guid UserId { get; set; }

        public User Creator { get; set; } = null!;

        // Many Forums belong to a Theme
        [Required]
        public Guid ThemeId { get; set; }

        public Theme Theme { get; set; } = null!;

        // A Forum can have many Comments (1 -> N)
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();

        // A Forum can have many Reactions (1 -> N)
        public ICollection<Reaction> Reactions { get; set; } = new List<Reaction>();
    }
}
