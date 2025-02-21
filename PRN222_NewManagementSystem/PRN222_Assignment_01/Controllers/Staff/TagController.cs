using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PRN222_Assignment_01.Models;
using PRN222_Assignment_01.Repositories;

namespace PRN222_Assignment_01.Controllers.Staff
{
    public class TagController : Controller
    {
        private ITagRepository _tagRepository;
        public TagController(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }


        public IActionResult Index()
        {
            var message = "";
            var tags = _tagRepository.GetTags(out message);
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

        public IActionResult Delete(int? id)
        {
            var message = "";
            _tagRepository.Delete(id ?? 0, out message);
            if (!message.IsNullOrEmpty())
            {
                ViewBag.Message = message;
            }
            return View();
        }
    }
}
