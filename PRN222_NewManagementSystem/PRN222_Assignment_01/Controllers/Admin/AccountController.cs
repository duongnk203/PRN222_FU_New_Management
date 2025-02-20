using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PRN222_Assignment_01.Models;
using PRN222_Assignment_01.Repositories;

namespace PRN222_Assignment_01.Controllers.Admin
{
    public class AccountController : Controller
    {
        private readonly ISystemAccountRepository _systemAccountRepository;

        public AccountController(ISystemAccountRepository systemAccountRepository)
        {
            _systemAccountRepository = systemAccountRepository;
        }

        public IActionResult Index()
        {
            string message = "";
            var accounts = _systemAccountRepository.GetAccounts(out message);
            if (!message.IsNullOrEmpty())
            {

            }
            return View(accounts);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(SystemAccount newAccount)
        {
            string message = "";
            if (ModelState.IsValid)
            {
                _systemAccountRepository.CreateAccount(newAccount, out message);
                if (!message.IsNullOrEmpty())
                {
                    ModelState.AddModelError(string.Empty, message);
                    return View(newAccount);
                }
            }

            return View(newAccount);
        }
    }
}
