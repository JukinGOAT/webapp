using System;
using System.ComponentModel.DataAnnotations;

namespace webapp.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }
        public int SenderId { get; set; }
        public Profile Sender { get; set; } = null!;
        public int ReceiverId { get; set; }
        public Profile Receiver { get; set; } = null!;
        [Required]
        public string Body { get; set; } = null!;
        public DateTime SentAt { get; set; }
        
    }
}
