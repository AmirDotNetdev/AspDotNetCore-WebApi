using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Comments
{
    public class CommnetDto
    {
        public int Id { get; set; }

        [Required]
        [MinLength(5, ErrorMessage = "Title must be 5 chracters.")]
        [MaxLength(280, ErrorMessage = "Title cannot be over 280 chracters.")]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MinLength(5, ErrorMessage = "Content must be 5 chracters.")]
        [MaxLength(280, ErrorMessage = "Content cannot be over 280 chracters.")]
        public string Content { get; set; } = string.Empty;

        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public string CreatedBy { get; set; } = string.Empty;

    }
}