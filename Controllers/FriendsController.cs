using Microsoft.AspNetCore.Mvc;
using webapp.Models;
using webapp.ViewModels;
using System.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace webapp.Controllers
{

    public class FriendsController : Controller
    {
        private readonly AppDbContext _context;

        public FriendsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            // privremeni korisnik
            
            var currentUser = _context.UserProfiles.FirstOrDefault(); // simulacija prijavljenog
            var messages = _context.Messages
                .Where(m => m.SenderId == currentUser.Id || m.ReceiverId == currentUser.Id)
                .Include(m => m.Sender)
                .Include(m => m.Receiver)
                .ToList();

            var chatPartners = messages
                .Select(m => m.SenderId == currentUser.Id ? m.Receiver : m.Sender)
                .Distinct()
                .Take(3) // do 3 najnovija
                .ToList();

            ViewBag.ActiveChats = chatPartners.Take(3).ToList();

            // svi profili osim sebe
            var allUsers = _context.UserProfiles.Where(p => p.Id != currentUser.Id).ToList();

            // prijatelji
            var friendIds = _context.Friendships
                .Where(f => f.UserId == currentUser.Id)
                .Select(f => f.FriendId)
                .ToList();

            var friends = allUsers.Where(p => friendIds.Contains(p.Id)).ToList();
            var recommended = allUsers.Where(p => !friendIds.Contains(p.Id)).ToList();

            var profile = _context.UserProfiles.FirstOrDefault(p => p.Id == currentUser.Id);

            var vm = new FriendsPageViewModel
            {
                Profile = profile,
                Friends = friends,
                RecommendedUsers = recommended
            };

            if (TempData["OpenProfilePopup"] is bool popupFlag && popupFlag)
            {
                ViewBag.OpenProfilePopup = true;
            }

            return View("Friends", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult FollowUser(int id)
        {
            var currentUser = _context.UserProfiles.FirstOrDefault(); // simulacija prijavljenog
            

            if (!_context.Friendships.Any(f => f.UserId == currentUser.Id && f.FriendId == id))
            {
                _context.Friendships.Add(new Friendship
                {
                    UserId = currentUser.Id,
                    FriendId = id
                });
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

    }
}
