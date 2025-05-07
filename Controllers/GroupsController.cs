using Microsoft.AspNetCore.Mvc;
using webapp.Models;
using webapp.ViewModels;

namespace webapp.Controllers
{
    public class GroupsController : Controller
    {
        private readonly AppDbContext _context;

        public GroupsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var profile = _context.UserProfiles.FirstOrDefault();
            
            var vm = new GroupsPageViewModel()
            {
                Profile = profile,
                MemberGroups = _context.Groups.Where(g => g.IsMember).ToList(),
                OtherGroups = _context.Groups.Where(g => !g.IsMember).ToList()
            };

            if (TempData["OpenProfilePopup"] is bool popupFlag && popupFlag)
                ViewBag.OpenProfilePopup = true;

            if (TempData["ProfileData"] is string profileJson)
                vm.Profile = System.Text.Json.JsonSerializer.Deserialize<Profile>(profileJson);

            return View("Groups", vm);
        }

        [HttpPost("/Groups/JoinGroup/{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult JoinGroup(int id)
        {
            var group = _context.Groups.FirstOrDefault(g => g.Id == id);
            if (group != null)
            {
                group.IsMember = true;
                _context.SaveChanges();
            }
            return Ok();
        }
    }
}
