using Microsoft.EntityFrameworkCore;
using webapp.Models;

namespace webapp.Models
{

    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }
        public DbSet<Friendship> Friendships { get; set; }
        public DbSet<Friend> Friends { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Profile> UserProfiles { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Instrukcija> Instrukcije { get; set; }
        public DbSet<ForumPost> ForumPosts { get; set; }
        public DbSet<ForumComment> ForumComments { get; set; }
        public DbSet<Message> Messages { get; set; }
    
    }
}
