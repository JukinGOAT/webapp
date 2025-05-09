using Microsoft.AspNetCore.Mvc;
using webapp.Models;
using webapp.ViewModels;
using webapp.Extensions;
using Microsoft.EntityFrameworkCore;

public class HomeController : Controller
{
    private readonly AppDbContext _context;

    public HomeController(AppDbContext context)
    {
        _context = context;
    }
    private const string SAVED_KEY = "SavedEvents";

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var profile = await _context.UserProfiles.FirstOrDefaultAsync();
        var saved = HttpContext.Session.GetObject<List<int>>(SAVED_KEY) ?? new List<int>();
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
        
        var vm = new HomePageViewModel
        {
            Profile = profile,
            Events = _context.Events
            .OrderBy(e => e.Id)
            
        .Select(e => new EventViewModel
        {
            Id = e.Id,
            Title = e.Title,
            DateRange = e.DateRange,
            Description = e.Description,
            Link = e.Link,
            LinkText = "Za više informacija, pogledaj njihovu stranicu.",
            FullDate = e.DatePublished,
            ImageUrl = e.ImageUrl,
            IsSaved = saved.Contains(e.Id)
        }).ToList()
        };
        var all = await _context.Events
                    .OrderByDescending(e => e.Id)
                    .ToListAsync();

        if (TempData["OpenProfilePopup"] is bool show && show)
            ViewBag.OpenProfilePopup = true;

        return View("Home", vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult ToggleSave(int id)
    {
        var saved = HttpContext.Session.GetObject<List<int>>(SAVED_KEY)
                    ?? new List<int>();

        if (saved.Contains(id)) saved.Remove(id);
        else saved.Add(id);

        HttpContext.Session.SetObject(SAVED_KEY, saved);

        return Redirect(Request.Headers["Referer"].ToString());
    }

    [HttpGet]
    public async Task<IActionResult> Saved()
    {
        var profile = await _context.UserProfiles.FirstOrDefaultAsync();
        var savedIds = HttpContext.Session.GetObject<List<int>>(SAVED_KEY) ?? new List<int>();

        var evs = await _context.Events
                          .Where(e => savedIds.Contains(e.Id))
                          
                          .ToListAsync();
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


        ViewBag.ActiveChats = chatPartners;
        var vm = new HomePageViewModel
        {
            Profile = profile,
            Events = evs.Select(e => new EventViewModel
            {
                Id = e.Id,
                Title = e.Title,
                DateRange = e.DateRange,  
                Description = e.Description,
                Link = e.Link,
                LinkText = "Za više informacija…",
                FullDate = e.DatePublished,
                ImageUrl = e.ImageUrl,
                IsSaved = true
            }).ToList()
        };
        if (TempData["OpenProfilePopup"] is bool show && show)
        {
            ViewBag.OpenProfilePopup = true;
        }

        return View("Saved", vm);
    }

    public IActionResult Fakultet()
    {
        return View();
    }
    [HttpPost]
    public IActionResult ClosePopup()
    {
        return RedirectToAction("Index", new { openProfile = false });
    }
}





