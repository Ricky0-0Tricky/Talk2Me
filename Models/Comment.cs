using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Talk2Me.Models
{
    public class Comment
    {
        [Key]
        [HiddenInput]
        public Guid CommentId { get; set; } = Guid.NewGuid();

        [Required(ErrorMessage = "The Comment's Content is Required.")]
        [StringLength(250, MinimumLength = 3, ErrorMessage = "The Comment's Content should have a minimum of 3 characters.")]
        public string Content { get; set; } = string.Empty;

        // Many Comments belong to a User
        [Required]
        public Guid UserId { get; set; }

        public User Creator { get; set; } = null!;

        // Parent comment
        public Guid? ParentCommentId { get; set; }

        public Comment? ParentComment { get; set; }

        // Many Comments belong to a Forum
        [Required]
        public Guid ForumId { get; set; }

        public Forum Forum { get; set; } = null!;
    }
}
