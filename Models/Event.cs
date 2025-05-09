using System;
using System.ComponentModel.DataAnnotations;

namespace webapp.Models
{
    public class Event
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(200)]
        public string Title { get; set; }

        [Required]
        public string DateRange { get; set; }

        [Required]
        public string DatePublished { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        [StringLength(500)]
        public string Link { get; set; }

        [StringLength(200)]
        public string ImageUrl { get; set; }
    }
}
