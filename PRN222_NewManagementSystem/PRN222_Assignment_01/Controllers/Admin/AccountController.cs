using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;
using PRN222_Assignment_01.Models;
using PRN222_Assignment_01.Repositories;
using PRN222_Assignment_01.ViewModel;

namespace PRN222_Assignment_01.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    public class AccountController : Controller
    {
        private readonly ISystemAccountRepository _systemAccountRepository;
        private readonly IConfiguration _configuration;

        public AccountController(ISystemAccountRepository systemAccountRepository, IConfiguration configuration)
        {
            _systemAccountRepository = systemAccountRepository;
            _configuration = configuration;
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
            var roleMapping = _configuration.GetSection("AccountRole").Get<Dictionary<string, int>>().Where(r => !r.Key.Equals("Admin"));
            ViewBag.RoleList = roleMapping.Select(r => new SelectListItem
            {
                Value = r.Value.ToString(),
                Text = r.Key
            });
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
                var roleMapping = _configuration.GetSection("AccountRole").Get<Dictionary<string, int>>().Where(r => !r.Key.Equals("Admin"));
                ViewBag.RoleList = roleMapping.Select(r => new SelectListItem
                {
                    Value = r.Value.ToString(),
                    Text = r.Key
                });
                if (!message.IsNullOrEmpty())
                {
                    ModelState.AddModelError(string.Empty, message);
                    return View(newAccount);
                }
                
            }
            return RedirectToAction(nameof(Index));
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
                var roleMapping = _configuration.GetSection("AccountRole").Get<Dictionary<string, int>>().Where(r => !r.Key.Equals("Admin"));
                var account = _systemAccountRepository.GetAccount(id ?? 0, out message);
                if (!message.IsNullOrEmpty())
                {
                    ModelState.AddModelError(string.Empty, message);
                    return View(account);
                }
                ViewBag.RoleList = roleMapping.Select(r => new SelectListItem
                {
                    Value = r.Value.ToString(),
                    Text = r.Key
                });
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
                var roleMapping = _configuration.GetSection("AccountRole").Get<Dictionary<string, int>>().Where(r => !r.Key.Equals("Admin"));
                _systemAccountRepository.UpdateAccount(id ?? 0, accountUpdate, out message);
                if (!message.IsNullOrEmpty())
                {
                    ModelState.AddModelError(string.Empty, message);
                    return View(accountUpdate);
                }
                ViewBag.RoleList = roleMapping.Select(r => new SelectListItem
                {
                    Value = r.Value.ToString(),
                    Text = r.Key
                });
            }
            return RedirectToAction(nameof(Index));
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
