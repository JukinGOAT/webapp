using System.Collections.Generic;
using webapp.Models;

namespace webapp.ViewModels
{
    public class HomePageViewModel
    {
        public Profile Profile { get; set; }
        public List<EventViewModel> Events { get; set; }
    }
}