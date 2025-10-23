using Microsoft.AspNetCore.Mvc;

namespace JadooTravel.ViewComponents.Admin
{
    public class _AdminHeadViewComponentPartial :ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
