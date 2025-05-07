using webapp.Models;

namespace webapp.ViewModels
{
    public class GroupsPageViewModel
    {
        public Profile Profile { get; set; }
        public List<Group> MemberGroups { get; set; }
        public List<Group> OtherGroups { get; set; }
    }

}
