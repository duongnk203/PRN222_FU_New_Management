using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;
using PRN222_Assignment_01.Models;
using PRN222_Assignment_01.Repositories;
using System.Collections.Generic;

namespace PRN222_Assignment_01.Controllers.Staff
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public IActionResult Index(string searchString)
        {
            var message = "";
            var categories = _categoryRepository.GetCategories(out message);
            if (!string.IsNullOrEmpty(searchString) && categories.Count > 0)
            {
                categories = categories
                    .Where(n => n.CategoryName.Contains(searchString, StringComparison.OrdinalIgnoreCase)
                             || n.CategoryDesciption.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            if (!message.IsNullOrEmpty())
            {
                ViewBag.Message = message;
                return View(categories);
            }
            return View(categories);
        }
        [HttpGet]
        public IActionResult Create()
        {
            string message = "";
            List<int> categoryIds = _categoryRepository.GetCategoryIds(out message);

            if (!string.IsNullOrEmpty(message))
            {
                // Xử lý lỗi (ví dụ: ghi log, hiển thị thông báo)
            }

            List<SelectListItem> selectListItems = categoryIds.Select(x => new SelectListItem
            {
                Value = x.ToString(),
                Text = x.ToString() // Hoặc lấy tên danh mục từ cơ sở dữ liệu
            }).ToList();

            ViewBag.ParentCategoryId = selectListItems;

            return View();
        }

        [HttpPost]
        public IActionResult Create(Category newCategory)
        {
            var message = "";
            _categoryRepository.Create(newCategory, out message);
            if (!message.IsNullOrEmpty())
            {
                ModelState.AddModelError(string.Empty, message);
                return View(newCategory);
            }
            return View(newCategory);
        }

        [HttpGet]
        public IActionResult Update(int? id)
        {
            var message = "";
            var category = _categoryRepository.GetCategory(id ?? 0, out message);
            if (!message.IsNullOrEmpty())
            {
                return NotFound();
            }
            List<int> categoryIds = _categoryRepository.GetCategoryIds(out message);

            if (!string.IsNullOrEmpty(message))
            {
                // Xử lý lỗi (ví dụ: ghi log, hiển thị thông báo)
            }

            List<SelectListItem> selectListItems = categoryIds.Select(x => new SelectListItem
            {
                Value = x.ToString(),
                Text = x.ToString() // Hoặc lấy tên danh mục từ cơ sở dữ liệu
            }).ToList();

            ViewBag.ParentCategoryId = selectListItems;
            return View(category);
        }
        [HttpPost]
        public IActionResult Update(int? id, Category updateCategory)
        {
            var message = "";
            _categoryRepository.Update(id ?? 0, updateCategory, out message);
            if (!message.IsNullOrEmpty())
            {
                ModelState.AddModelError(string.Empty, message);
                return View(updateCategory);
            }
            return View(updateCategory);
        }

        public IActionResult Delete(int? id)
        {
            var message = "";
            _categoryRepository.Delete(id ?? 0, out message);
            if (!message.IsNullOrEmpty())
            {
                TempData["Message"] = message;
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Detail(int? id)
        {
            var message = "";
            var category = _categoryRepository.GetCategory(id ?? 0, out message);
            if(!message.IsNullOrEmpty())
            {
                ModelState.AddModelError(string.Empty, message);
                return View(category);
            }
            return View(category);
        }
    }
}
