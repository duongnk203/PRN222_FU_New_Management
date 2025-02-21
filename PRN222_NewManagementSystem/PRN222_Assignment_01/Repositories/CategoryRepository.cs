using PRN222_Assignment_01.Models;
using System.Diagnostics.Eventing.Reader;
using System.Linq;

namespace PRN222_Assignment_01.Repositories
{
    public interface ICategoryRepository
    {
        void Create(Category newCategory, out string message);
        void Update(int id, Category newCategory, out string message);
        void Delete(int id, out string message);
        List<Category> GetCategories(out string messsage);
        Category GetCategory(int id, out string message);
        List<int> GetCategoryIds(out string messsage);
    }
    public class CategoryRepository : ICategoryRepository
    {
        private readonly FUNewsManagementContext _context;
        public CategoryRepository(FUNewsManagementContext context)
        {
            _context = context;
        }

        public void Create(Category newCategory, out string message)
        {
            message = "";
            if (newCategory == null)
            {
                message = "Category is invalid!";
                return;
            }
            if (IsExitCategory(newCategory.CategoryName))
            {
                message = "Category Name is exits!";
                return;
            }
            _context.Categories.Add(newCategory);
            _context.SaveChanges();
        }

        public void Delete(int id, out string message)
        {
            message = "";
            var category = _context.Categories.FirstOrDefault(x => x.CategoryID == id);
            if(id == 0 || category == null)
            {
                message = "Category is not exist!";
                return;
            }
            if(IsExitCategoryInNewsArticle(id))
            {
                message = "Cannot delete category because there is an article in use.";
                return;
            }
            if (category != null)
            {
                _context.Categories.Remove(category);
                _context.SaveChanges();
            }
        }

        public List<Category> GetCategories(out string messsage)
        {
            messsage = "";
            List<Category> categories = _context.Categories.ToList();
            if(categories == null)
            {
                messsage = "The list of categories is empty!";
            }
            return categories;
        }

        public Category GetCategory(int id, out string message)
        {
            message = "";
            var category = _context.Categories.FirstOrDefault(x => x.CategoryID == id);
            if (id == 0)
            {
                message = "Category is not exits!";
            }
            return category;
        }

        public List<int> GetCategoryIds(out string messsage)
        {
            messsage = "";
            List<short> categoryIds = _context.Categories.Select(x => x.CategoryID).ToList();
            return categoryIds.ConvertAll(x => (int)x);
        }

        public void Update(int id, Category newCategory, out string message)
        {
            message = "";
            if(id == 0)
            {
                message = "CategoryId is not exist!";
                return;
            }
            var category = _context.Categories.FirstOrDefault(x => x.CategoryID == id);
            if (category == null)
            {
                message = "Category is not exist!";
                return;
            }
            if (IsExitCategory(newCategory.CategoryName) && !newCategory.CategoryName.Equals(category.CategoryName))
            {
                message = "Category Name is exist!";
                return;
            }
            category.CategoryName = newCategory.CategoryName;
            category.ParentCategoryID = newCategory.ParentCategoryID;
            category.CategoryDesciption = newCategory.CategoryDesciption;
            category.IsActive = newCategory.IsActive;
            _context.Categories.Update(category);
            _context.SaveChanges();
        }

        private bool IsExitCategory(string categoryName)
        {
            if (categoryName == null) return false;
            var category = _context.Categories.FirstOrDefault(x => x.CategoryName.Equals(categoryName));
            if (category == null) return false;
            return true;
        }

        private bool IsExitCategoryInNewsArticle(int categoryId)
        {
            return _context.NewsArticles.Any(x => x.CategoryID == categoryId);
        }
    }
}
