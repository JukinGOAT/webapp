using webapp.Models;
namespace webapp.Models
{
    public class ForumPost
    {
        public int Id { get; set; }
        public string Autor { get; set; } = null!;
        public string Sadrzaj { get; set; } = null!;
        public int ProfileId { get; set; }
        public Profile Profile { get; set; } = null!;
        public ICollection<ForumComment> Comments { get; set; } = new List<ForumComment>();
    }
}