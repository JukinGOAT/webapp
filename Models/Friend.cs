using System.ComponentModel.DataAnnotations;

namespace webapp.Models
{
    public class Friend
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
        public string Faculty { get; set; }
        public string ImageUrl { get; set; }
        public bool IsFriend { get; set; } 
    }
}
