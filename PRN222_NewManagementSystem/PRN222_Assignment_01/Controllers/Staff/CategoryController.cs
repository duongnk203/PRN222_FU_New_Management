using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;
using PRN222_Assignment_01.Models;
using PRN222_Assignment_01.Repositories;
using System.Collections.Generic;

namespace PRN222_Assignment_01.Controllers.Staff
{
    [Authorize(Roles = "Staff")]
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
            ViewBag.ParentCategoryId = _categoryRepository.GetCategories_1(out message);

            return View();
        }

        [HttpPost]
        public IActionResult Create(Category newCategory)
        {
            var message = "";
            ViewBag.ParentCategoryId = _categoryRepository.GetCategories_1(out message);

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
            ViewBag.ParentCategoryId = _categoryRepository.GetCategories_1(out message);
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
            ViewBag.ParentCategoryId = _categoryRepository.GetCategories_1(out message);

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
            ViewBag.ParentCategoryId = _categoryRepository.GetCategories_1(out message);
            if (!message.IsNullOrEmpty())
            {
                ModelState.AddModelError(string.Empty, message);
                return View(category);
            }
            return View(category);
        }
    }
}
