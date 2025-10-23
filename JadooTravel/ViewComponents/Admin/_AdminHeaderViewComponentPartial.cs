using JadooTravel.Services.UserServices;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JadooTravel.ViewComponents.Admin
{
    public class _AdminHeaderViewComponentPartial :ViewComponent
    {
        private readonly IUserService _userService;

        public _AdminHeaderViewComponentPartial(IUserService userService)
        {
            _userService = userService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            string imageUrl;
            string fullName = "Kullanıcı";

            var user = await _userService.GetUserAsync(userId);
            imageUrl = string.IsNullOrEmpty(user?.ImageUrl)
                ? "/Admin/assets/images/defaultuser.png"
                : user.ImageUrl;
            fullName = $"{user.FirstName} {user.LastName}";

            ViewBag.UserImage = imageUrl;
            ViewBag.UserFullName = fullName;

            return View();
        }
    }
}
