using Microsoft.AspNetCore.Mvc;
using PRN222_Assignment_01.Models;
using PRN222_Assignment_01.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using PRN222_Assignment_01.Service;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering; // Thêm using này

namespace PRN222_Assignment_01.Controllers.Staff
{
    [Authorize(Roles = "Staff")]
    public class ProfileController : Controller
    {
        private readonly ISystemAccountRepository _systemAccountRepository;
        private readonly RoleService _roleService;

        public ProfileController(ISystemAccountRepository systemAccountRepository, RoleService roleService, IConfiguration configuration)
        {
            _systemAccountRepository = systemAccountRepository;
            _roleService = roleService;
        }

        public IActionResult Details()
        {
            if((User.FindFirst("AccountID")?.Value).IsNullOrEmpty())
            {
                return RedirectToAction("AccessDenied", "Authentication");
            }
            int accountID = Int32.Parse(User.FindFirst("AccountID")?.Value);
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
            if ((User.FindFirst("AccountID")?.Value).IsNullOrEmpty())
            {
                return RedirectToAction("AccessDenied", "Authentication");
            }
            int accountID = Int32.Parse(User.FindFirst("AccountID")?.Value);
            var message = "";
            var account = _systemAccountRepository.GetAccount(accountID, out message);
            ViewBag.RoleList = _roleService.GetRoles();
            if (!string.IsNullOrEmpty(message))
            {
                ModelState.AddModelError(string.Empty, message);
                return View(account);
            }
            return View(account);
        }

        [HttpPost] 
        public IActionResult Edit(SystemAccount updateAccount)
        {
            if ((User.FindFirst("AccountID")?.Value).IsNullOrEmpty())
            {
                return RedirectToAction("AccessDenied", "Authentication");
            }
            int accountID = Int32.Parse(User.FindFirst("AccountID")?.Value);
            var message = "";
            _systemAccountRepository.UpdateAccount(accountID, updateAccount, out message);
            ViewBag.RoleList = _roleService.GetRoles();
            if (!string.IsNullOrEmpty(message))
            {
                ModelState.AddModelError(string.Empty, message);
                return View(updateAccount);
            }
            return RedirectToAction(nameof(Details));
        }
    }
}