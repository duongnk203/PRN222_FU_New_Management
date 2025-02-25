using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;
using PRN222_Assignment_01.Models;
using PRN222_Assignment_01.Repositories;

namespace PRN222_Assignment_01.Controllers.Staff
{
    public class NewsArticleController : Controller
    {
        private readonly INewsArticalRepository _newsArticalRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ISystemAccountRepository _systemAccountRepository;
        public NewsArticleController(INewsArticalRepository newsArticalRepository, ICategoryRepository categoryRepository, ISystemAccountRepository systemAccountRepository)
        {
            _newsArticalRepository = newsArticalRepository;
            _categoryRepository = categoryRepository;
            _systemAccountRepository = systemAccountRepository;
        }

        public IActionResult Index(string searchString)
        {
            var message = "";
            var newsArticles = _newsArticalRepository.GetNewsArticles(0, out message);
            if (!string.IsNullOrEmpty(searchString) && newsArticles.Count > 0)
            {
                newsArticles = newsArticles
                    .Where(n => n.NewsTitle.Contains(searchString, StringComparison.OrdinalIgnoreCase)
                             || n.Headline.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            if (!string.IsNullOrEmpty(message))
            {
                ModelState.AddModelError(string.Empty, message);
            }
            return View(newsArticles);
        }

        public IActionResult Details(int? id)
        {
            var message = "";
            var newsArticle = _newsArticalRepository.GetNewsArticle(id ?? 0, 0, out message);
            ViewBag.CategoryName = _categoryRepository.GetCategory((int)newsArticle.CategoryID, out message).CategoryName;
            ViewBag.CreatedByName = _systemAccountRepository.GetAccountName((int)(newsArticle.CreatedByID), out message);
            ViewBag.UpdateByName = newsArticle.UpdatedByID == null ? "" : _systemAccountRepository.GetAccountName((int)newsArticle.UpdatedByID, out message);

            if (!message.IsNullOrEmpty() || newsArticle == null)
            {
                ModelState.AddModelError(string.Empty, message);
                return View(newsArticle);
            }
            return View(newsArticle);
        }

        [Authorize(Roles = "Staff")]
        public IActionResult Create()
        {
            var message = "";
            ViewBag.CategoryID = _categoryRepository.GetCategories_1(out message);

            
            return View();
        }

        [Authorize(Roles = "Staff")]
        [HttpPost]
        public IActionResult Create(NewsArticle newsArticle)
        {
            var message = "";
            int accountID = Int32.Parse(User.FindFirst("AccountID")?.Value);
            _newsArticalRepository.Create(accountID, newsArticle, out message);
            ViewBag.CategoryID = _categoryRepository.GetCategories_1(out message);

            if (!string.IsNullOrEmpty(message))
            {
                ModelState.AddModelError(string.Empty, message);
                return View(newsArticle);
            }
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Staff")]
        [Route("Edit")]
        public IActionResult Edit(int? id)
        {
            var message = "";
            var newsArticle = _newsArticalRepository.GetNewsArticle(id ?? 0, 0, out message);
            ViewBag.CategoryID = _categoryRepository.GetCategories_1(out message);

            ViewBag.CreatedByName = _systemAccountRepository.GetAccountName((int)(newsArticle.CreatedByID), out message);
            ViewBag.UpdateByName = newsArticle.UpdatedByID == null ? "": _systemAccountRepository.GetAccountName((int)newsArticle.UpdatedByID, out message);

            if (newsArticle == null || !message.IsNullOrEmpty())
            {
                ModelState.AddModelError(string.Empty, message);
                return View(newsArticle);
            }

            return View(newsArticle);
        }

        [Authorize(Roles = "Staff")]
        [HttpPost]
        public IActionResult Edit(NewsArticle newsArticleUpdate)
        {
            var message = "";
            int accountID = Int32.Parse(User.FindFirst("AccountID")?.Value);
            ViewBag.CategoryID = _categoryRepository.GetCategories_1(out message);

            _newsArticalRepository.Update(newsArticleUpdate.NewsArticleID, accountID, newsArticleUpdate, out newsArticleUpdate, out message);
            if (!message.IsNullOrEmpty())
            {
                ModelState.AddModelError(string.Empty, message);
                return View(newsArticleUpdate);
            }
            return RedirectToAction(nameof(Index));
        }

        [Route("Delete")]
        public IActionResult Delete(int? id)
        {
            var message = "";
            _newsArticalRepository.Delete(id ?? 0, out message);
            if (!message.IsNullOrEmpty())
            {
                ViewBag.Message = message;
            }
            return RedirectToAction(nameof(Index));
        }

    }
}
