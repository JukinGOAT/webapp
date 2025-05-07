using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace webapp.Models
{
    public class Profile 
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required]
        public string Faculty { get; set; } = string.Empty;

        public string? Tag { get; set; } = string.Empty;
        public string? Tags { get; set; } = string.Empty;

        public string? About { get; set; } = string.Empty;

        public string ProfileImageUrl { get; set; } = string.Empty;

        public string? CoverImageUrl { get; set; } = string.Empty;
        public bool IsOnline { get; set; } = false;

    }
}