using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Talk2Me.Models
{
    public class Photo
    {
        [Key]
        [HiddenInput]
        public Guid PhotoId { get; set; }

        [Display(Name = "Photo")]
        [Required(ErrorMessage = "The Photo is Required.")]
        public byte[] PhotoFile {  get; set; }
    }
}
