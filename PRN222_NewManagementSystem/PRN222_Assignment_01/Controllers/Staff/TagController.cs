using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PRN222_Assignment_01.Models;
using PRN222_Assignment_01.Repositories;

namespace PRN222_Assignment_01.Controllers.Staff
{
    [Authorize(Roles = "Staff")]
    public class TagController : Controller
    {
        private ITagRepository _tagRepository;
        public TagController(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }


        public IActionResult Index(string searchString)
        {
            var message = "";
            var tags = _tagRepository.GetTags(out message);
            if (!string.IsNullOrEmpty(searchString) && tags.Count > 0)
            {
                tags = tags
                    .Where(n => n.TagName.Contains(searchString, StringComparison.OrdinalIgnoreCase)
                             || n.Note.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }
            if (tags == null || !message.IsNullOrEmpty())
            {
                ModelState.AddModelError(string.Empty,message); 
            }
            return View(tags);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Tag newTag)
        {
            var message = "";
            if (!ModelState.IsValid)
            {
                return View(newTag);
            }
            _tagRepository.Create(newTag, out message);
            if (!message.IsNullOrEmpty())
            {
                ModelState.AddModelError(string.Empty, message);
                return View(newTag);
            }
            return View(newTag);
        }

        public IActionResult Edit(int? id)
        {
            var message = "";
            var tag = _tagRepository.GetTag(id ?? 0, out message);
            if(tag == null || !message.IsNullOrEmpty())
            {
                message = "Tag is not exist!";
            }
            return View(tag);
        }

        [HttpPost]
        public IActionResult Edit(int? id, Tag tagUpdate)
        {
            var message = "";
            if (!ModelState.IsValid)
            {
                message = "Tag is invalid";
                return View(tagUpdate);
            }
            _tagRepository.Update(id??0,tagUpdate, out message);
            if (!message.IsNullOrEmpty())
            {
                ModelState.AddModelError(string.Empty, message);
                return View(tagUpdate);
            }

            return View(tagUpdate);
        }

        public IActionResult Delete(int? id)
        {
            var message = "";
            _tagRepository.Delete(id ?? 0, out message);
            if (!message.IsNullOrEmpty())
            {
                ViewBag.Message = message;
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
