using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webapp.Models;
using webapp.ViewModels;

public class FakultetController : Controller
{
    private readonly AppDbContext _context;

    public FakultetController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Index(string tab, int? kolegijId)
    {
        var currentUser = _context.UserProfiles.FirstOrDefault(); // simulacija prijavljenog
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
        var profile = _context.UserProfiles.FirstOrDefault();
        var instrukcije = _context.Instrukcije.ToList();
        var kolegiji = GetMockKolegiji();

        var vm = new FakultetViewModel()
        {
            Profile = profile,
            FAQs = new List<FAQItem>
            {
                new FAQItem {
                    Pitanje = "Vodič za brucoše – najčešća pitanja",
                    Odgovor = "U prilogu se nalazi kratki vodič za brucoše Grafičkog fakulteta u kojem možete pronaći sve informacije vezane za studiranje, linkove na pravilnike i odluke, kao i popis projekata na kojima možete sudjelovati."
                },
                new FAQItem {
                    Pitanje = "Što je kolokvij?",
                    Odgovor = "Ispit ili test koji se polaže tijekom semestra, a cilj mu je provjeriti znanje i razumijevanje materijala koji je dosad obrađen na nastavi. Neke kolegije je moguće položiti putem kolokvija."
                },
                new FAQItem {
                    Pitanje = "Kako se prijavljuju i odjavljuju ispiti?",
                    Odgovor = "Prijava i odjava ispita obavlja se putem Studomata. Kako biste prijavili ispit, potrebno je odabrati odgovarajući ispitni rok i potvrditi prijavu." +
                    " Prijava ispita moguća je najkasnije do tri dana prije samog ispita." +
                    "\r\n\r\nUkoliko se predomislite ili niste u mogućnosti pristupiti ispitu, odjavu također možete izvršiti najkasnije tri dana prije ispita. Važno je napomenuti da, ukoliko ispit ne odjavite na vrijeme, dolazak na ispit neće biti važeći."
                },
                new FAQItem {
                    Pitanje = "Gdje pronaći literaturu?",
                    Odgovor = "Službena web stranica fakulteta - Najčešće se preporuča literatura nalazi u silabusima kolegija na web stranici fakulteta." +
                    "\r\nE-učenje i Studomat - Neki profesori postavljaju materijale na sustavu za e-učenje ili Studomat." +
                    "\r\nFakultetska knjižnica - Knjižnica Grafičkog fakulteta nudi udžbenike, skripte i znanstvene radove koji su potrebni za studij." +
                    "\r\nSkripte i bilješke starijih studenata - Studenti često dijele skripte putem društvenih mreža ili studentskih grupa. Sada je ovaj dio puno lakši jer je stupio na snagu tvoj UniConnect." +
                    "\r\nOnline resursi - Znanstvene baze podataka poput Google Scholar-a, ResearchGate-a ili platformi poput Scribda mogu imati korisne materijale."
                },
                new FAQItem {
                    Pitanje = "Kako i kada se prijaviti za stipendije?",
                    Odgovor = "Kako se prijaviti:\r\nPratiti natječaje - Stipendije objavljuju fakulteti, gradovi, općine, ministarstva i privatne institucije." +
                    " Natječaji su obično dostupni na službenim web stranicama." +
                    "\r\nPrikupiti potrebne dokumente - To uključuje prijavni obrazac, potvrdu o upisu, prijepis ocjena, dokaz o prihodima i eventualne preporuke." +
                    "\r\nPodnijeti prijavu - Prijava se može slati online ili fizički, ovisno o pravilima natječaja." +
                    "\r\nPratiti rezultate - Nakon predaje, važno je redovito provjeravati objave rezultata i eventualno dopuniti dokumentaciju ako se traži." +
                    "\r\n\r\nKada se prijaviti?\r\nDržavne i gradske stipendije - Najčešće su u rujnu, listopadu i studentom." +
                    "\r\nSveučilišne stipendije - Ovisno o fakultetu, ali obično krajem semestra.\r\nPrivatne i međunarodne stipendije - Tijekom cijele godine, ovisno o organizatoru."
                },
                new FAQItem {
                    Pitanje = "Što su ECTS bodovi i čemu služe?",
                    Odgovor = "ECTS bodovi (European Credit Transfer and Accumulation System) su europski sustav prijenosa i akumulacije bodova koji omogućuje standardizaciju visokoškolskog obrazovanja u Europi." +
                    "\r\n\r\nČemu služe ECTS bodovi?\r\nMjerenje opterećenja studenata - Svaki kolegij nosi određeni broj bodova koji održava količinu rada potrebnu za njegovo uspješno savladavanje (predavanje, seminar, vježbe, samostalno učenje)." +
                    "\r\nLakša usporedba programa - Omogućuje usporedivost studijskih programa između različitih sveučilišta i država." +
                    "\r\nPrijenos bodova - Ako student prelazi s jednog fakulteta na drugi (unutar ili izvan zemlje), ECTS bodovi omogućuju priznavanje položenih kolegija." +
                    "\r\nZavršetak studija - Svaka razina studija zahtijeva određeni broj ECTS bodova:\r\n                        - Preddiplomski studij: 180 ECTS (3 godine)\r\n                        - Diplomski studij: 120 ECTS (2 godine)\r\n                        - Integrirani studij: 300 ECTS (35 godine)\r\n \r\nJedna akademska godina obično iznosi 60 ECTS bodova, što znači da student mora steći dovoljno bodova kako bi završio studij u predviđenom roku."
                }
            },
            Kolegiji = kolegiji,
            AktivniKolegij = kolegijId.HasValue ? kolegiji.FirstOrDefault(k => k.Id == kolegijId) : null,
            ActiveTab = tab ?? "Opce",
             Instrukcije = instrukcije
        };

        if (TempData["OpenProfilePopup"] is bool popupFlag && popupFlag)
        {
            ViewBag.OpenProfilePopup = true;
        }

     

        ViewBag.ActiveTab = vm.ActiveTab;
        return View("Fakultet", vm);
    }

    private List<Kolegij> GetMockKolegiji()
    {
        return new List<Kolegij>
        {
            new Kolegij { Id = 1, Naziv = "Ambalaža 2", Opis = "Opis kolegija Ambalaža 2.", Lekcije = new List<string> { "Praktični zadatci", "Teorija" } },
            new Kolegij { Id = 2, Naziv = "Digitalni tisak", Opis = "Opis kolegija Digitalni tisak.", Lekcije = new List<string> { "Praktični zadatci", "Teorija" } },
            new Kolegij { Id = 3, Naziv = "Holografija", Opis = "Opis kolegija Holografija.", Lekcije = new List<string> { "Praktični zadatci", "Teorija" } },
            new Kolegij { Id = 4, Naziv = "Knjigovodstvo 2", Opis = "Opis kolegija Knjigovodstvo 2.", Lekcije = new List<string> { "Praktični zadatci", "Teorija" } },
            new Kolegij { Id = 5, Naziv = "WEB Dizajn 1", Opis = "Opis kolegija WEB Dizajn 1.", Lekcije = new List<string> { "Praktični zadatci", "Teorija" } },
            new Kolegij { Id = 6, Naziv = "Marketing 2", Opis = "Opis kolegija Marketing 2.", Lekcije = new List<string> { "Praktični zadatci", "Teorija" } },
            new Kolegij { Id = 7, Naziv = "Multimedijske komunikacije 2", Opis = "Opis kolegija Multimedijske komunikacije 2.", Lekcije = new List<string> { "Praktični zadatci", "Teorija" } },
            new Kolegij { Id = 8, Naziv = "Tisak ambalaže", Opis = "Opis kolegija Tisak ambalaže.", Lekcije = new List<string> { "Praktični zadatci", "Teorija" } },
            new Kolegij { Id = 9, Naziv = "Digitalni Multimedij 2", Opis = "Opis kolegija Digitalni Multimedij 2.", Lekcije = new List<string> { "Praktični zadatci", "Teorija" } },
            new Kolegij { Id = 10, Naziv = "Optoelektronički sustavi 2", Opis = "Opis kolegija Optoelektronički sustavi 2.", Lekcije = new List<string> { "Praktični zadatci", "Teorija" } },
            new Kolegij { Id = 11, Naziv = "WEB Dizajn 2", Opis = "Opis kolegija WEB Dizajn 2.", Lekcije = new List<string> { "Praktični zadatci", "Teorija" } },
            new Kolegij { Id = 12, Naziv = "CTP Tehnologija", Opis = "Opis kolegija CTP Tehnologija.", Lekcije = new List<string> { "Praktični zadatci", "Teorija" } },
            new Kolegij { Id = 13, Naziv = "Umjetnička fotografija 1", Opis = "Opis kolegija Umjetnička fotografija 1.", Lekcije = new List<string> { "Praktični zadatci", "Teorija" } },
            new Kolegij { Id = 14, Naziv = "Računarska grafika", Opis = "Opis kolegija Računarska grafika.", Lekcije = new List<string> { "Praktični zadatci", "Teorija" } },
            new Kolegij { Id = 15, Naziv = "Mehaničke simulacije u računalnim animacijama", Opis = "Opis kolegija Mehaničke simulacije u računalnim animacijama.", Lekcije = new List<string> { "Praktični zadatci", "Teorija" } }
        };
    }
    [HttpPost]
    
    public IActionResult DodajInstrukciju(string sadrzaj)
    {
        if (!string.IsNullOrWhiteSpace(sadrzaj))
        {
            var user = _context.UserProfiles.FirstOrDefault();
            var instrukcija = new Instrukcija
            {
                Autor = user?.FullName ?? "Trenutni korisnik",
                Sadrzaj = sadrzaj
            };
            _context.Instrukcije.Add(instrukcija);
            _context.SaveChanges();
        }

       
        return RedirectToAction("Index", new { tab = "Instrukcije" });
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult ZatraziInstrukciju(int id)
    {
       
        var instrukcija = _context.Instrukcije.FirstOrDefault(i => i.Id == id);
        if (instrukcija != null)
        {
            
            _context.SaveChanges();
        }

        return RedirectToAction("Index", new { tab = "Instrukcije" });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult OtkaziInstrukciju(int id)
    {
       

        return RedirectToAction("Index", new { tab = "Instrukcije" });
    }


}
