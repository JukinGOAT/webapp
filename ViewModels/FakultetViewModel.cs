using System.Collections.Generic;
using webapp.Models;

namespace webapp.ViewModels
{
    public class FAQItem
    {
        public string Pitanje { get; set; }
        public string Odgovor { get; set; }
    }

    public class FakultetViewModel
    {
        public List<FAQItem> FAQs { get; set; }
        public Profile Profile { get; set; }
        public string ActiveTab { get; set; }
        public List<Kolegij> Kolegiji { get; set; }
        public Kolegij? AktivniKolegij { get; set; } // Za detaljni prikaz
        public List<Group> MemberGroups { get; set; } = new();

        public List<Group> OtherGroups { get; set; } = new();
        public List<Instrukcija> Instrukcije { get; set; } = new();
        public List<int> RequestedInstructionIds { get; set; } = new List<int>();

    }
    
}
