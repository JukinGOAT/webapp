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
       
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            

           
            
            modelBuilder.Entity<Profile>().HasData(
     new Profile
     {
         Id = 3,
         FullName = "Ivana Valić",
         Faculty = "Grafički fakultet",
         ProfileImageUrl = "/images/avatars/ivana.png",
         IsOnline = true
     },
     new Profile
     {
         Id = 2,
         FullName = "Marko Marić",
         Faculty = "Elektrotehnički fakultet",
         ProfileImageUrl = "/images/avatars/marko.png",
         IsOnline = false
     },
     new Profile
     {
         Id = 5,
         FullName = "Ana Janić",
         Faculty = "Grafički fakultet",
         ProfileImageUrl = "/images/avatars/ana.png",
         IsOnline = true
     },
     new Profile
     {
         Id = 4,
         FullName = "Suzana Ilić",
         Faculty = "Ekonomski fakultet",
         ProfileImageUrl = "/images/avatars/suzana.png",
         IsOnline = false
     }
 );
            modelBuilder.Entity<Friendship>().HasData(
    new Friendship { Id = 1, UserId = 1, FriendId = 2 },
    new Friendship { Id = 2, UserId = 1, FriendId = 3 },
    new Friendship { Id=3, UserId=1,FriendId=6}
);


            modelBuilder.Entity<Message>().HasData(
     new Message
     {
         Id = 3,
         SenderId = 3,
         ReceiverId = 1,
         SentAt = DateTime.SpecifyKind(new DateTime(2025, 1, 1, 12, 0, 0), DateTimeKind.Utc),
         Body = "Bok, kako si?"
     },
     new Message
     {
         Id = 2,
         SenderId = 2,
         ReceiverId = 1,
         SentAt = DateTime.SpecifyKind(new DateTime(2025, 1, 1, 12, 0, 0), DateTimeKind.Utc),
         Body = "Super, hvala – a ti?"
     }
 );
        }
    }
}
