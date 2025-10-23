using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JadooTravel.Controllers
{
    [AllowAnonymous]
    [IgnoreAntiforgeryToken]
    public class LanguageController : Controller
    {
        [HttpPost]
        public IActionResult ChangeLanguage(string lang)
        {
            if (string.IsNullOrWhiteSpace(lang))
                lang = "tr";

            HttpContext.Session.SetString("lang", lang);
            HttpContext.Session.CommitAsync().Wait(); 

            var referer = Request.Headers["Referer"].ToString();
            if (!string.IsNullOrEmpty(referer))
                return Redirect(referer);

            return RedirectToAction("Index", "Default");
        }
    }
}
