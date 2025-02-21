using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;
using PRN222_Assignment_01.Models;
using PRN222_Assignment_01.Repositories;

namespace PRN222_Assignment_01.Controllers.Staff
{
    [Route("Staff/NewsArticleView")]
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

        [Route("Index")]
        public IActionResult Index(string searchString)
        {
            var message = "";
            var newsArticles = _newsArticalRepository.GetNewsArticles(0, out message);
            if(!string.IsNullOrEmpty(searchString) && newsArticles.Count > 0)
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

        [Route("Details")]
        public IActionResult Details(int? id)
        {
            var message = "";
            var newsArticle = _newsArticalRepository.GetNewsArticle(id ?? 0, 0,out message);
            if (!message.IsNullOrEmpty() || newsArticle == null)
            {
                ModelState.AddModelError(string.Empty, message);
                return View(newsArticle);
            }
            return View(newsArticle);
        }

        [Route("Create")]
        public IActionResult Create()
        {
            var message = "";
            var categoryIds = _categoryRepository.GetCategoryIds(out message);

            ViewBag.CategoryId = categoryIds.Select(x => new SelectListItem
            {
                Value = x.ToString(),
                Text = x.ToString()
            }).ToList();
            ViewBag.CreatedById = _systemAccountRepository.GetAccountIds(out message).Select(x => new SelectListItem
            {
                Value = x.ToString(),
                Text = x.ToString()
            }).ToList();
            return View();
        }

        [Route("Create")]
        [HttpPost]
        public IActionResult Create(NewsArticle newsArticle)
        {
            var message = "";
            if (HttpContext.Session.GetString("AccountID") == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var accountId = Int32.Parse(HttpContext.Session.GetString("AccountID"));
            _newsArticalRepository.Create(accountId, newsArticle, out message);
            if (!string.IsNullOrEmpty(message))
            {
                ModelState.AddModelError(string.Empty, message);
                return View(newsArticle);
            }
            return View(newsArticle);
        }

        [Route("Edit")]
        public IActionResult Edit(int? id)
        {
            var message = "";
            var newsArticle = _newsArticalRepository.GetNewsArticle(id ?? 0, 0, out message);

            var categoryIds = _categoryRepository.GetCategoryIds(out message);

            ViewBag.CategoryId = categoryIds.Select(x => new SelectListItem
            {
                Value = x.ToString(),
                Text = x.ToString()
            }).ToList();
            ViewBag.CreatedById = _systemAccountRepository.GetAccountIds(out message).Select(x => new SelectListItem
            {
                Value = x.ToString(),
                Text = x.ToString()
            }).ToList();

            if (newsArticle == null || !message.IsNullOrEmpty())
            {
                ModelState.AddModelError(string.Empty, message);
                return View(newsArticle);
            }

            return View(newsArticle);
        }

        [Route("Edit")]
        [HttpPost]
        public IActionResult Edit(int? id, NewsArticle newsArticleUpdate)
        {
            var message = "";
            if (HttpContext.Session.GetString("AccountID") == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var categoryIds = _categoryRepository.GetCategoryIds(out message);

            ViewBag.CategoryId = categoryIds.Select(x => new SelectListItem
            {
                Value = x.ToString(),
                Text = x.ToString()
            }).ToList();
            var accountId = Int32.Parse(HttpContext.Session.GetString("AccountID"));
            _newsArticalRepository.Update(id ?? 0, accountId, newsArticleUpdate, out newsArticleUpdate, out message);
            if (!message.IsNullOrEmpty())
            {
                ModelState.AddModelError(string.Empty, message);
                return View(newsArticleUpdate);
            }
            return View(newsArticleUpdate);
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
