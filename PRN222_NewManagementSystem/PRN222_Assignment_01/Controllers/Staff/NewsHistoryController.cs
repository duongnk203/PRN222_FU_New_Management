using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PRN222_Assignment_01.Models;
using PRN222_Assignment_01.Repositories;

namespace PRN222_Assignment_01.Controllers.Staff
{
    [Authorize(Roles = "Staff")]
    public class NewsHistoryController : Controller
    {
        private readonly INewsArticalRepository _newsArticalRepository;
        public NewsHistoryController(INewsArticalRepository newsArticalRepository)
        {
            _newsArticalRepository = newsArticalRepository;
        }

        public IActionResult NewsHistory(string searchString)
        {
            var message = "";
            if ((User.FindFirst("AccountID")?.Value).IsNullOrEmpty())
            {
                return RedirectToAction("AccessDenied", "Authentication");
            }
            int accountID = Int32.Parse(User.FindFirst("AccountID")?.Value);
            var newsHistory = _newsArticalRepository.GetNewsArticlesByCreated(accountID, out message);
            if (!string.IsNullOrEmpty(searchString) && newsHistory.Count > 0)
            {
                newsHistory = newsHistory
                    .Where(n => n.NewsTitle.Contains(searchString, StringComparison.OrdinalIgnoreCase)
                             || n.Headline.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }
            if (!string.IsNullOrEmpty(message))
            {
                ModelState.AddModelError(string.Empty, message);
            }
            return View(newsHistory);
        }
    }
}
