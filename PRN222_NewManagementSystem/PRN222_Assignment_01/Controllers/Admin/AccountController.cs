using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PRN222_Assignment_01.Models;
using PRN222_Assignment_01.Repositories;
using PRN222_Assignment_01.ViewModel;

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
                ViewBag.Message = message;
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

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            var message = "";
            if (id == null || id == 0)
            {
                return NotFound();
            }
            else
            {
                var account = _systemAccountRepository.GetAccount(id ?? 0, out message);
                if (!message.IsNullOrEmpty())
                {
                    ModelState.AddModelError(string.Empty, message);
                    return View(account);
                }
                return View(account);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int? id, SystemAccount accountUpdate)
        {
            var message = "";
            if (id != accountUpdate.AccountID)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                _systemAccountRepository.UpdateAccount(id ?? 0, accountUpdate, out message);
                if (!message.IsNullOrEmpty())
                {
                    ModelState.AddModelError(string.Empty, message);
                    return View(accountUpdate);
                }
            }
            return View(accountUpdate);
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            var message = "";
            _systemAccountRepository.DeleteAccount(id ?? 0, out message);
            if(!message.IsNullOrEmpty())
            {
                ViewBag.Delete = message;
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
