using JadooTravel.Dtos.UserDtos;
using JadooTravel.Services.UserServices;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace JadooTravel.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login");

            var user = await _userService.GetUserAsync(userId);
            return View(user);
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult CreateUser()
        {
            return View(new CreateUserDto());
        }
       
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserDto createUserDto)
        {
            if (string.IsNullOrEmpty(createUserDto.ImageUrl))
                createUserDto.ImageUrl = "/Admin/assets/images/defaultuser.png";
            await _userService.CreateUserAsync(createUserDto);
            return RedirectToAction("Login");
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginUserDto loginUserDto)
        {
            var user = await _userService.LoginUserAsync(loginUserDto);

            if (user == null)
            {
                ModelState.AddModelError("", "Kullanıcı adı veya şifre hatalı!");
                return View(loginUserDto);
            }

         
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true, 
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(2)
            };

         
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties
            );
            HttpContext.Session.SetString("UserName", user.UserName);
          
            return RedirectToAction("Index", "Admin");
        }

        public async Task<IActionResult> Logout()
        {

            HttpContext.Session.Remove("UserName");
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "User");

        }


        [HttpGet]
        public async Task<IActionResult> UpdateUser()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var value = await _userService.GetUserByIdAsync(userId);
            return View(value);
        }


        [HttpPost]
        public async Task<IActionResult> UpdateUser(UpdateUserDto updateUserDto)
        {
            await _userService.UpdateUserAsync(updateUserDto);
            TempData["SuccessMessage"] = "Bilgiler güncellendi!";
            return RedirectToAction("Index", "User");
        }


        [HttpGet]
        public async Task<IActionResult> UpdatePassword()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var model = new UpdateUserPasswordDto { UserId = userId };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePassword(UpdateUserPasswordDto updateUserPasswordDto)
        {
            if (!ModelState.IsValid)
                return View(updateUserPasswordDto);

            try
            {
                await _userService.UpdatePasswordAsync(updateUserPasswordDto);
                ViewBag.SuccessMessage = "Şifre başarıyla güncellendi!";
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
            }

            return View(updateUserPasswordDto);
        }
    }
}
