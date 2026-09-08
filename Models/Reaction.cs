using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Talk2Me.Models
{
    public class Reaction
    {
        [Key]
        [HiddenInput]
        public Guid ReactionId { get; set; } = Guid.NewGuid();

        [Display(Name = "Reaction")]
        [Required(ErrorMessage = "The Upvote State is Required.")]
        public bool IsUpvote {  get; set; }

        // Many Reactions belong to a Forum
        [Required]
        public Guid ForumId { get; set; }

        public Forum Forum { get; set; } = null!;
    }
}
