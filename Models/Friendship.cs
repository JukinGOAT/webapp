using System.ComponentModel.DataAnnotations;

namespace webapp.Models
{
    public class Friendship
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }         // trenutni korisnik
        public int FriendId { get; set; }       // njegov prijatelj

        public Profile User { get; set; }
        public Profile Friend { get; set; }
    }
}