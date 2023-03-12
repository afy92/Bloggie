using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Bloggie.Web.Controllers
{
    public class AdminTagsController : Controller
    {
        private BloggieDbContext _bloggieDbContext;
        //Dependency Injection 
        public AdminTagsController(BloggieDbContext bloggieDbContext)
        {
            _bloggieDbContext = bloggieDbContext;
        }
        [HttpGet]
        //Tag ekleme get metodu
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ActionName("Add")]
        //public IActionResult SubmitTag()
        //{
        //    var name=Request.Form["name"];
        //    var display=Request.Form["display"];
        //    return View("Add");
        //}

        public IActionResult Add(AddTagRequest addTagRequest)
        {
            //var name = addTagRequest.Name;
            //var display = addTagRequest.DisplayName;
            var tag = new Tag
            {
                Name = addTagRequest.Name,
                DisplayName = addTagRequest.DisplayName,
            };
            _bloggieDbContext.Add(tag);
            _bloggieDbContext.SaveChanges();
            return RedirectToAction("List");
        }
        [HttpGet]
        //Tag listeleme metodu
        public IActionResult List()
        {
            var tags = _bloggieDbContext.Tags.ToList();
            return View(tags);
        }

        //edit sayfasının görüntülenme action'ı
        [HttpGet]
        public IActionResult Edit(Guid id)
        {
            //var tag = _bloggieDbContext.Tags.Find(); -- aşağıdakinin aynısı
            var tag = _bloggieDbContext.Tags.FirstOrDefault(t => t.Id == id);
            return View();
        }
    }
}
