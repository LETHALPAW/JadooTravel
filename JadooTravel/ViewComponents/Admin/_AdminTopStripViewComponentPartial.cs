using Microsoft.AspNetCore.Mvc;

namespace JadooTravel.ViewComponents.Admin
{
    public class _AdminTopStripViewComponentPartial:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
