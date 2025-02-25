using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PRN222_Assignment_01.Repositories;

namespace PRN222_Assignment_01.Controllers.Lecturer
{
    public class NewsArticleViewController : Controller
    {
        private readonly INewsArticalRepository _newsArticalRepository;
        public NewsArticleViewController(INewsArticalRepository newsArticalRepository)
        {
            _newsArticalRepository = newsArticalRepository;
        }
        public IActionResult Index(string searchString)
        {
            var message = "";
            var newsArticles = _newsArticalRepository.GetNewsArticles(2, out message);

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
            var newsArticle = _newsArticalRepository.GetNewsArticle(id ?? 0, 2,out message);
            if (!message.IsNullOrEmpty() || newsArticle == null)
            {
                ModelState.AddModelError(string.Empty, message);
                return View(newsArticle);
            }
            return View(newsArticle);
        }
    }
}
