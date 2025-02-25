using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PRN222_Assignment_01.Repositories;
using PRN222_Assignment_01.ViewModel;

namespace PRN222_Assignment_01.Controllers.Authentincation
{
    public class AuthenticationController : Controller
    {
        private readonly ISystemAccountRepository _systemAccountRepository;
        public AuthenticationController(ISystemAccountRepository systemAccountRepository)
        {
            _systemAccountRepository = systemAccountRepository;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(AccountLogin accountLogin)
        {
            var (role, message) = await _systemAccountRepository.Login(accountLogin, HttpContext);

            if (!string.IsNullOrEmpty(message))
            {
                ModelState.AddModelError(string.Empty, message);
                return View(accountLogin);
            }

            return role switch
            {
                1 => RedirectToAction("Index", "Category"), // Staff
                2 => RedirectToAction("Index", "NewsArticleView", new { area = "Lecturer" }), // Lecture
                3 => RedirectToAction("Index", "Account"), // Admin
                _ => NoContent() // Trường hợp không hợp lệ
            };
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            var message = await _systemAccountRepository.Logout(HttpContext);
            if (!string.IsNullOrEmpty(message))
            {
                TempData["Message"] = message;
                return View();
            }
            return RedirectToAction("Index", "NewsArticle");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
