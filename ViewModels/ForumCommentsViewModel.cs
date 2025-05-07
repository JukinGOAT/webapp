// ViewModels/ForumCommentsViewModel.cs
using System.Collections.Generic;
using webapp.Models;
namespace webapp.ViewModels
{
    public class ForumCommentsViewModel
    {
        public ForumPost Post { get; set; }
        public List<ForumComment> Comments { get; set; }
        public Profile Profile { get; set; }
    }
}
