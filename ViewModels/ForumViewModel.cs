using System.Collections.Generic;
using webapp.Models;
namespace webapp.ViewModels
{
    public class ForumViewModel
    {
        public Profile Profile { get; set; }
        public string NewContent { get; set; }
        public List<ForumPost> Posts { get; set; }
        
    }
}