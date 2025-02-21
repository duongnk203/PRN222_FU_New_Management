using Microsoft.AspNetCore.Mvc;
using PRN222_Assignment_01.Models;
using PRN222_Assignment_01.Repositories;
using Microsoft.AspNetCore.Http; // Thêm using này

namespace PRN222_Assignment_01.Controllers.Staff
{
    public class ProfileController : Controller
    {
        private readonly ISystemAccountRepository _systemAccountRepository;

        public ProfileController(ISystemAccountRepository systemAccountRepository)
        {
            _systemAccountRepository = systemAccountRepository;
        }

        public IActionResult Details()
        {
            if (HttpContext.Session.GetString("AccountID") == null)
            {
                return RedirectToAction("Login", "Account");//Redirect to login if the user is not logged in.
            }

            int accountID = Int32.Parse(HttpContext.Session.GetString("AccountID"));
            var message = "";
            var account = _systemAccountRepository.GetAccount(accountID, out message);
            if (!string.IsNullOrEmpty(message))
            {
                ModelState.AddModelError(string.Empty, message);
                return View(account);
            }
            return View(account);
        }

        public IActionResult Edit()
        {
            if (HttpContext.Session.GetString("AccountID") == null)
            {
                return RedirectToAction("Login", "Account");//Redirect to login if the user is not logged in.
            }

            int accountID = Int32.Parse(HttpContext.Session.GetString("AccountID"));
            var message = "";
            var account = _systemAccountRepository.GetAccount(accountID, out message);
            if (!string.IsNullOrEmpty(message))
            {
                ModelState.AddModelError(string.Empty, message);
                return View(account);
            }
            return View(account);
        }

        [HttpPost] // Thêm [HttpPost] attribute
        public IActionResult Edit(SystemAccount updateAccount)
        {
            if (HttpContext.Session.GetString("AccountID") == null)
            {
                return RedirectToAction("Login", "Account");//Redirect to login if the user is not logged in.
            }

            int accountID = Int32.Parse(HttpContext.Session.GetString("AccountID"));
            var message = "";
            _systemAccountRepository.UpdateAccount(accountID, updateAccount, out message);
            if (!string.IsNullOrEmpty(message))
            {
                ModelState.AddModelError(string.Empty, message);
                return View(updateAccount);
            }
            return RedirectToAction(nameof(Details));
        }
    }
}