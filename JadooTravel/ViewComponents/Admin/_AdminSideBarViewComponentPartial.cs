using Microsoft.AspNetCore.Mvc;

namespace JadooTravel.ViewComponents.Admin
{
    public class _AdminSideBarViewComponentPartial :ViewComponent

    {
        public IViewComponentResult Invoke()
        {
            return View();
        }

    }
}
