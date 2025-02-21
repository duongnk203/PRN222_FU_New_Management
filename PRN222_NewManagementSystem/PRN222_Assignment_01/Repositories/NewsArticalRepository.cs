using Microsoft.IdentityModel.Tokens;
using PRN222_Assignment_01.Models;
using System.Runtime.InteropServices;

namespace PRN222_Assignment_01.Repositories
{
    public interface INewsArticalRepository
    {
        void Create(NewsArticle newsArticle, out string message);
        void Update(string id, NewsArticle newsArticleUpdate, out string message);
        void Delete(string id, out string message);
        List<NewsArticle> GetNewsArticles(out string message);
        NewsArticle GetNewsArticle(string id, out string message);
    }

    public interface ITagRepository
    {
        void Create(Tag newsTag, out string message);
        void Delete(int id, out string message);
        Tag GetTag(int id, out string message);
        List<Tag> GetTags(out string message);
    }

    public class TagRepository : ITagRepository
    {
        private readonly FUNewsManagementContext _context;
        public TagRepository(FUNewsManagementContext context)
        {
            _context = context;
        }

        public void Create(Tag newsTag, out string message)
        {
            message = "";
            if (newsTag == null)
            {
                message = "Tag is not create!";
                return;
            }
            if (IsExistTag(newsTag.TagName))
            {
                message = "Tag name is exist!";
                return;
            }
            _context.Tags.Add(newsTag);
            _context.SaveChanges();
        }

        public void Delete(int id, out string message)
        {
            message = "";
            if (id == 0)
            {
                message = "TagId is not exist!";
                return;
            }
            var tag = GetTag(id, out message);
            if (tag == null || message.IsNullOrEmpty())
            {
                message = "Tag is not exist!";
                return;
            }
            _context.Tags.Remove(tag);
            _context.SaveChanges();
        }

        public Tag GetTag(int id, out string message)
        {
            message = "";
            if (id == 0)
            {
                message = "TagId is invalid";
            }
            var tag = _context.Tags.FirstOrDefault(x => x.TagID == id);
            if (tag == null)
            {
                message = "Tag is not exist!";
            }
            return tag;
        }

        public bool IsExistTag(string tagName)
        {
            return _context.Tags.FirstOrDefault(t => t.TagName == tagName) != null;
        }

        public List<Tag> GetTags(out string message)
        {
            message = "";
            var tags = _context.Tags.ToList();
            if (tags.Count == 0)
            {
                message = "The list tag is empty!";
            }
            return tags;
        }
    }

    public class NewsArticalRepository : INewsArticalRepository
    {
        private readonly FUNewsManagementContext _context;
        public NewsArticalRepository(FUNewsManagementContext context)
        {
            _context = context;
        }

        public void Create(NewsArticle newsArticle, out string message)
        {
            message = "";
            if (newsArticle == null)
            {
                message = "News Article is not create!";
                return;
            }

            _context.NewsArticles.Add(newsArticle);
            _context.SaveChanges();
        }

        public void Delete(string id, out string message)
        {
            message = "";
            if (id.IsNullOrEmpty())
            {
                message = "News article is not exist!";
                return;
            }
            var newsArticle = GetNewsArticle(id, out message);
            if (newsArticle == null || !message.IsNullOrEmpty())
            {
                return;
            }
            _context.NewsArticles.Remove(newsArticle);
            _context.SaveChanges();
        }

        public NewsArticle GetNewsArticle(string id, out string message)
        {
            message = "";
            if (id.IsNullOrEmpty())
            {
                message = "News Article id is not exist!";
            }
            var newsArticle = _context.NewsArticles.FirstOrDefault(x => x.NewsArticleID.Equals(id));
            if (newsArticle == null)
            {
                message = "News Article id is not exist!";
            }
            return newsArticle;
        }

        public List<NewsArticle> GetNewsArticles(out string message)
        {
            message = "";
            List<NewsArticle> newsArticles = _context.NewsArticles.ToList();
            if (newsArticles.Count == 0)
            {
                message = "The list news article is empty";
            }
            return newsArticles;
        }

        public void Update(string id, NewsArticle newsArticleUpdate, out string message)
        {
            message = "";
            if (id.IsNullOrEmpty())
            {
                message = "News article is not exist!";
                return;
            }
            var newsArticle = GetNewsArticle(id, out message);
            if (message.IsNullOrEmpty() || newsArticle == null)
            {
                return;
            }

            if (IsExitNewTitle(newsArticleUpdate.NewsTitle) && !newsArticleUpdate.NewsTitle.Equals(newsArticle.NewsTitle))
            {
                message = "Title is exist!";
                return;
            }
            newsArticle = newsArticleUpdate;
            _context.NewsArticles.Update(newsArticle);
            _context.SaveChanges();
        }
        public bool IsExitNewTitle(string title)
        {
            return _context.NewsArticles.FirstOrDefault(x => x.NewsTitle.Equals(title)) != null;
        }
    }
}
