using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Talk2Me.Models
{
    /// <summary>
    /// Represents a photo of a comment.
    /// Stores its file.
    /// </summary>
    public class Photo
    {
        [Key]
        [HiddenInput]
        public Guid PhotoId { get; set; } = Guid.NewGuid();


        [Display(Name = "Photo")]
        [Required(ErrorMessage = "The Photo is Required.")]
        public byte[] PhotoFile {  get; set; }

        // Many Photos belong to a Comment
        [Required]
        public Guid CommentId { get; set; }

        public Comment Comment { get; set; } = null!;
    }
}
