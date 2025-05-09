using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webapp.Models;
using webapp.ViewModels;

namespace webapp.Controllers
{
    public class ForumController : Controller
    {
        private readonly AppDbContext _context;

        public ForumController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {

            var profile = _context.UserProfiles.FirstOrDefault();
            var currentUser = _context.UserProfiles.FirstOrDefault(); 
            var messages = _context.Messages
                .Where(m => m.SenderId == currentUser.Id || m.ReceiverId == currentUser.Id)
                .Include(m => m.Sender)
                .Include(m => m.Receiver)
                .ToList();

            var chatPartners = messages
                .GroupBy(m => m.SenderId == currentUser.Id ? m.Receiver : m.Sender)
        .Select(g => new { User = g.Key, Last = g.Max(x => x.SentAt) })
        .OrderByDescending(x => x.Last)
        .Select(x => x.User)
        .Take(3)
        .ToList();

            ViewBag.ActiveChats = chatPartners.Take(3).ToList();

            
            var vm = new ForumViewModel
            {
                Profile = profile,
                Posts = _context.ForumPosts
                .Include(p => p.Comments)
                    .ThenInclude(c => c.Profile)
                .ToList()
            };

            if (TempData["OpenProfilePopup"] is bool popupFlag && popupFlag)
                ViewBag.OpenProfilePopup = true;

            return View("Forum", vm);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(string newContent)
        {
            if (!string.IsNullOrWhiteSpace(newContent))
            { 
                var user = _context.UserProfiles.FirstOrDefault();

                _context.ForumPosts.Add(new ForumPost
                {
                    Autor = user?.FullName ?? "Trenutni korisnik",
                    Sadrzaj = newContent,
                    ProfileId = user?.Id ?? 0
                });
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddComment(int postId, string content)
        {
            if (!string.IsNullOrWhiteSpace(content))
            {
                var user = _context.UserProfiles.FirstOrDefault();

                _context.ForumComments.Add(new ForumComment
                {
                    Author = user?.FullName ?? "Trenutni korisnik",
                    Content = content,
                    PostId = postId,
                    ProfileId = user?.Id ?? 0
                });
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }

        
        [HttpGet]
        public IActionResult ViewComments(int id)
        {
            var post = _context.ForumPosts
                               .Include(p => p.Comments)
                               .FirstOrDefault(p => p.Id == id);
            if (post == null) return NotFound();

            var vm = new ForumCommentsViewModel
            {
                Post = post,
                Comments = post.Comments.ToList()
            };

            return View("Comments", vm);
        }
    }
}
