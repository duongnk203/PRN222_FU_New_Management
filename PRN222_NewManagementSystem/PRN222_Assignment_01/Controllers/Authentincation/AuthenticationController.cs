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

        [Route("Login")]
        [HttpPost]
        public IActionResult Login(AccountLogin accountLogin)
        {
            string message = "";
            int role = _systemAccountRepository.Login(accountLogin, out message);
            if (!message.IsNullOrEmpty())
            {
                ModelState.AddModelError(string.Empty, message);
                return View(accountLogin);
            }
            switch (role)
            {
                case 1: // Staff
                    return RedirectToAction("Index", "Category");
                case 2: //Lecture
                    return View();
                case 3: //Admin
                    return RedirectToAction("Index", "Account");
                default:
                    return NoContent();
            }
        }
    }
}
