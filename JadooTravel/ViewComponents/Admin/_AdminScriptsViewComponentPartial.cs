using Microsoft.AspNetCore.Mvc;

namespace JadooTravel.ViewComponents.Admin
{
    public class _AdminScriptsViewComponentPartial :ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
