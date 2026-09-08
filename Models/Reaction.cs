using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Talk2Me.Models
{
    public class Reaction
    {
        [Key]
        [HiddenInput]
        public Guid ReactionId { get; set; }

        [Display(Name = "Reaction")]
        [Required(ErrorMessage = "The Upvote State is Required.")]
        public bool IsUpvote {  get; set; }
    }
}
