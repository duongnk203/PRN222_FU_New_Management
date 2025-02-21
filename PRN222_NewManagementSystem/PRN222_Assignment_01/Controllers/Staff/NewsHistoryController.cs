using Microsoft.AspNetCore.Mvc;
using PRN222_Assignment_01.Models;
using PRN222_Assignment_01.Repositories;

namespace PRN222_Assignment_01.Controllers.Staff
{
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
            if (HttpContext.Session.GetString("AccountID") == null)
            {
                return RedirectToAction("Login", "Account");//Redirect to login if the user is not logged in.
            }

            int accountID = Int32.Parse(HttpContext.Session.GetString("AccountID"));
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
