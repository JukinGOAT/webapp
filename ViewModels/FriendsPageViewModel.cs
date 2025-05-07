using System.Collections.Generic;
using webapp.Models;

namespace webapp.ViewModels
{
    public class FriendsPageViewModel
    {
        public List<Profile> Friends { get; set; }
        public List<Profile> RecommendedUsers { get; set; }

        public Profile Profile { get; set; }
    }
}
