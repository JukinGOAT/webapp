using webapp.Models;

namespace webapp.ViewModels
{
    public class ChatroomViewModel
    {
        public int ConversationId { get; set; }
        public int CurrentUserId { get; set; }
        public string ReceiverName { get; set; }
        public int ReceiverId { get; set; }

        public string NewMessage { get; set; }
        public List<Message> Messages { get; set; }
        public List<Profile> Contacts { get; set; } = new();  // prijatelji
        public int? ActiveChatUserId { get; set; }            // s kim razgovaramo
        public List<Message> History { get; set; } = new();   // poruke u razgovoru
        public Profile Me { get; set; }
        public Profile Profile { get; set; }
        public string Body { get; set; }
    }
}