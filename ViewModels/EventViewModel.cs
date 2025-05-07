using webapp.Models;

namespace webapp.ViewModels
{
    public class EventViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string DateRange { get; set; }
     
        public string Description { get; set; }
        public string Link { get; set; }
        public string LinkText { get; set; }
        public string FullDate { get; set; }
        public string ImageUrl { get; set; }
        public bool IsSaved { get; set; }
        public Profile Profile { get; set; }
    }
}
