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

        public IActionResult Index()
        {
            var message = "";
            var newsArticles = _newsArticalRepository.GetNewsArticles(out message);
            if (!message.IsNullOrEmpty())
            {
                ViewBag.message = message;
            }
            return View(newsArticles);
        }

        public IActionResult Details(string? id)
        {
            var message = "";
            var newsArticle = _newsArticalRepository.GetNewsArticle(id ?? "", out message);
            if (!message.IsNullOrEmpty() || newsArticle == null)
            {
                ModelState.AddModelError(string.Empty, message);
                return View(newsArticle);
            }
            return View(newsArticle);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(NewsArticle newsArticle)
        {
            return View();
        }

        public IActionResult Edit(string? id)
        {
            var message = "";
            var newsArticle = _newsArticalRepository.GetNewsArticle(id ?? "", out message);

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

        [HttpPost]
        public IActionResult Edit(string? id, NewsArticle newsArticleUpdate)
        {
            var message = "";
            _newsArticalRepository.Update(id ?? "", newsArticleUpdate, out message);
            if (!message.IsNullOrEmpty())
            {
                ModelState.AddModelError(string.Empty, message);
                return View(newsArticleUpdate);
            }
            return View(newsArticleUpdate);
        }

        public IActionResult Delete(string? id)
        {
            var message = "";
            _newsArticalRepository.Delete(id ?? "", out message);
            if (!message.IsNullOrEmpty())
            {
                ViewBag.Message = message;
            }
            return View();
        }

    }
}
