using Microsoft.AspNetCore.Mvc;
using webapp.Models;
using webapp.ViewModels;




public class ProfileController : Controller
{
    public IActionResult Index()
    {
        ViewBag.OpenProfilePopup = false; 
        return View();
    }

    [HttpPost]
    public IActionResult OtvoriProfil()
    {
        var profile = new Profile
        {
            FullName = "Mateja Milanović",
            Faculty = "Grafički fakultet",
            Tag = "GRF",
            Tags = "fotografija,web dizajn,game development,tenis,3D modeliranje,grafički dizajn",
            About = "Studentica sam 4. godine Grafičkog fakulteta, smjer Multimedija. " +
            "Volim fotografiju i web dizajn - uvijek istražujem nove ideje i kreativne načine izražavanja." +
            " Uz to, tenis mi je super način za opuštanje i održavanje fokusa. " +
            "Uvijek sam za nova poznanstva i zanimljive projekte!",
            ProfileImageUrl = "prof.jpg",
            CoverImageUrl = "back.jpg"
        };

        TempData["OpenProfilePopup"] = true;
        TempData["ProfileData"] = System.Text.Json.JsonSerializer.Serialize(profile);
        return Redirect(Request.Headers["Referer"].ToString());
    }

}
