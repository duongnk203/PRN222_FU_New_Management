﻿using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.IdentityModel.Tokens;
using PRN222_Assignment_01.Models;
using PRN222_Assignment_01.Service;
using System.Runtime.InteropServices;

namespace PRN222_Assignment_01.Repositories
{
    public interface ITagRepository
    {
        void Create(Tag newsTag, out string message);
        void Delete(int id, out string message);
        Tag GetTag(int id, out string message);
        List<Tag> GetTags(out string message);
        void Update(int? id, Tag tagUpdate, out string message);
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
            if (tag == null || !message.IsNullOrEmpty())
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

        public void Update(int? id, Tag tagUpdate, out string message)
        {
            message = "";
            if (id == 0 || tagUpdate == null)
            {
                message = "Tag is not exist!";
                return;
            }
            if (tagUpdate != null)
            {
                var tag = _context.Tags.FirstOrDefault(x => x.TagID == id);
                if (tag == null)
                {
                    message = "Tag not found";
                    return;
                }
                if (IsExistTag(tagUpdate.TagName) && !tag.TagName.Equals(tagUpdate.TagName))
                {
                    message = "Tag name is exist";
                    return;
                }

                // Cập nhật các thuộc tính của tag từ tagUpdate
                tag.TagName = tagUpdate.TagName;
                tag.Note = tagUpdate.Note;
                // Cập nhật các thuộc tính khác nếu cần

                _context.Update(tag); // Không cần thiết, tag đã được theo dõi và thay đổi
                _context.SaveChanges();
            }
        }
    }

    public interface INewsArticalRepository
    {
        void Create(int accountID, NewsArticle newsArticle, out string message);
        void Update(int id, int accountId, NewsArticle newsArticleUpdate, out NewsArticle newsArticleAfterUpdate,out string message);
        void Delete(int id, out string message);
        List<NewsArticle> GetNewsArticles(int role, out string message);
        NewsArticle GetNewsArticle(int id, int role,out string message);
        List<NewsArticle> GetNewsArticlesByCreated(int createdId, out string message);
    }

    public class NewsArticalRepository : INewsArticalRepository
    {
        private readonly FUNewsManagementContext _context;
        private readonly IEmailService _emailService;
        public NewsArticalRepository(FUNewsManagementContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public void Create(int accountID, NewsArticle newsArticle, out string message)
        {
            message = "";
            if (newsArticle == null)
            {
                message = "News Article is not created!";
                return;
            }

            // Gán thông tin người tạo
            newsArticle.CreatedByID = (short)accountID;
            newsArticle.CreatedDate = DateTime.Now;

            // Thêm vào database
            _context.NewsArticles.Add(newsArticle);
            _context.SaveChanges(); // Lúc này ID sẽ được cập nhật

            // Tạo link chi tiết của bài viết
            string articleLink = $"https://localhost:7135/Lecturer/NewsArticleView/Details?id={newsArticle.NewsArticleID}";

            // Nếu bài viết được publish (NewsStatus == true) thì gửi email
            if (newsArticle.NewsStatus == true)
            {
                _emailService.SendEmail(
                    "duongnk203@gmail.com",
                    $"{newsArticle.NewsTitle}",
                    $"Author ID: {newsArticle.CreatedByID}. <br> Click <a href='{articleLink}'>here</a> to view the article.",
                    articleLink
                );
            }
        }


        public void Delete(int id, out string message)
        {
            message = "";
            if (id == 0)
            {
                message = "News article is not exist!";
                return;
            }
            var newsArticle = GetNewsArticle(id, 0, out message);
            if (newsArticle == null || !message.IsNullOrEmpty())
            {
                return;
            }
            _context.NewsArticles.Remove(newsArticle);
            _context.SaveChanges();
        }

        public NewsArticle GetNewsArticle(int id,int role , out string message)
        {
            message = "";
            if (id == 0)
            {
                message = "News Article id is not exist!";
            }
            NewsArticle newsArticle = new NewsArticle(); 
            if(role == 2) newsArticle = _context.NewsArticles.FirstOrDefault(x => x.NewsArticleID.Equals(id) && x.NewsStatus == true);
            newsArticle = _context.NewsArticles.FirstOrDefault(x => x.NewsArticleID.Equals(id));
            if (newsArticle == null)
            {
                message = "News Article id is not exist!";
            }
            return newsArticle;
        }

        public List<NewsArticle> GetNewsArticles(int role, out string message)
        {
            message = "";
            List<NewsArticle> newsArticles = _context.NewsArticles.ToList();
            if(role == 2) newsArticles = newsArticles.Where(x => x.NewsStatus == true).ToList();
            if (newsArticles.Count == 0)
            {
                message = "The list news article is empty";
            }
            return newsArticles;
        }

        public void Update(int id, int accountId, NewsArticle newsArticleUpdate, out NewsArticle newsArticleAfterUpdate, out string message)
        {
            newsArticleAfterUpdate = newsArticleUpdate;
            message = "";
            if (id == 0)
            {
                message = "News article is not exist!";
                return;
            }
            var newsArticle = GetNewsArticle(id, 0,out message);
            if (!message.IsNullOrEmpty() || newsArticle == null)
            {
                return;
            }

            if (IsExitNewTitle(newsArticleUpdate.NewsTitle) && !newsArticleUpdate.NewsTitle.Equals(newsArticle.NewsTitle))
            {
                message = "Title is exist!";
                return;
            }
            newsArticle.Headline = newsArticleUpdate.Headline;
            newsArticle.NewsContent = newsArticleUpdate.NewsContent;
            newsArticle.NewsStatus = newsArticleUpdate.NewsStatus;
            newsArticle.NewsSource = newsArticleUpdate.NewsSource;
            newsArticle.NewsTitle = newsArticleUpdate.NewsTitle;
            newsArticle.NewsTags = newsArticleUpdate.NewsTags;
            newsArticle.UpdatedByID = (short)accountId;
            newsArticle.ModifiedDate = DateTime.Now;
            _context.Update(newsArticle);
            _context.SaveChanges();
            newsArticleAfterUpdate = newsArticleUpdate;
        }
        public bool IsExitNewTitle(string title)
        {
            return _context.NewsArticles.FirstOrDefault(x => x.NewsTitle.Equals(title)) != null;
        }

        public List<NewsArticle> GetNewsArticlesByCreated(int id, out string message)
        {
            message = "";
            List<NewsArticle> newsArticles = _context.NewsArticles.Where(x => x.CreatedByID == id).ToList();
            if (newsArticles.Count == 0)
            {
                message = "The list news article is empty";
            }
            return newsArticles;
        }
    }
}
