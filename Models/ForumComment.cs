using System;
namespace webapp.Models
{
    public class ForumComment
    {
        public int Id { get; set; }
        public string Author { get; set; } = null!;
        public string Content { get; set; } = null!;
        public int PostId { get; set; }
        public ForumPost Post { get; set; } = null!;
        public int ProfileId { get; set; }
        public Profile Profile { get; set; } = null!;
    }
}
