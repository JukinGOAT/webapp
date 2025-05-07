using Microsoft.AspNetCore.Mvc;
using webapp.Models;
using webapp.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace webapp.Controllers
{
    public class MessagesController : Controller
    {
        private readonly AppDbContext _context;
        public MessagesController(AppDbContext context)
            => _context = context;

        [HttpGet]
        public async Task<IActionResult> Index(int? chatWithId)
        {
            var currentUser = await _context.UserProfiles.FirstOrDefaultAsync(); 
            if (currentUser == null) return NotFound();

            
            var messages = await _context.Messages
                .Where(m => m.SenderId == currentUser.Id || m.ReceiverId == currentUser.Id)
                .Include(m => m.Sender)
                .Include(m => m.Receiver)
                .ToListAsync();

            var recentChats = messages
                .GroupBy(m => m.SenderId == currentUser.Id ? m.Receiver : m.Sender)
                .Select(g => new
                {
                    User = g.Key,
                    LastMessageTime = g.Max(m => m.SentAt)
                })
                .OrderByDescending(g => g.LastMessageTime)
                .Select(g => g.User)
                .Take(3)
                .ToList();

            ViewBag.ActiveChats = recentChats;

            var profile = currentUser;

            var convoPartners = messages
                .GroupBy(m => m.SenderId == currentUser.Id ? m.ReceiverId : m.SenderId)
                .Select(g => new
                {
                    UserId = g.Key,
                    LastMessageTime = g.Max(m => m.SentAt)
                })
                .OrderByDescending(g => g.LastMessageTime)
                .ToList();

            var contactIds = convoPartners.Select(c => c.UserId).ToList();

            var contacts = await _context.UserProfiles
                .Where(p => contactIds.Contains(p.Id))
                .ToListAsync();

            contacts = convoPartners
                .Select(c => contacts.First(p => p.Id == c.UserId))
                .ToList();

            List<Message> chatHistory = new();
            if (chatWithId.HasValue)
            {
                chatHistory = await _context.Messages
                    .Where(m =>
                        (m.SenderId == currentUser.Id && m.ReceiverId == chatWithId.Value) ||
                        (m.SenderId == chatWithId.Value && m.ReceiverId == currentUser.Id))
                    .OrderBy(m => m.SentAt)
                    .ToListAsync();

                if (!contactIds.Contains(chatWithId.Value))
                {
                    var userToAdd = await _context.UserProfiles.FirstOrDefaultAsync(p => p.Id == chatWithId.Value);
                    if (userToAdd != null)
                        contacts.Insert(0, userToAdd); 
                }
            }

            var vm = new ChatroomViewModel
            {
                Profile = profile,
                Me = currentUser,
                Contacts = contacts,
                ActiveChatUserId = chatWithId,
                History = chatHistory
            };

            if (TempData["OpenProfilePopup"] is bool show && show)
            {
                ViewBag.OpenProfilePopup = true;
            }

            return View("Messages", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Send(int toUserId, string body)
        {
            var me = await _context.UserProfiles.FirstOrDefaultAsync();
            if (me == null || string.IsNullOrWhiteSpace(body))
                return RedirectToAction(nameof(Index), new { chatWithId = toUserId });

            _context.Messages.Add(new Message
            {
                SenderId = me.Id,
                ReceiverId = toUserId,
                Body = body,
                SentAt = DateTimeOffset.UtcNow
            });

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { chatWithId = toUserId });
        }
    }
}
