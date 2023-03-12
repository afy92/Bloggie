using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Controllers
{
    public class AdminTagsController : Controller
    {
        private BloggieDbContext bloggieDbContext;
		//Dependency Injection 
		public AdminTagsController(BloggieDbContext bloggieDbContext)
		{
			this.bloggieDbContext = bloggieDbContext;
		}

		//Tag ekleme get metodu
		[HttpGet]
		public IActionResult Add()
		{
			return View();
		}

		//tag ekleme post metodu
		[HttpPost]
		[ActionName("Add")]
		public async Task<IActionResult> Add(AddTagRequest addTagRequest)
		{
			//var name= Request.Form["name"];
			//var displayName = Request.Form["displayName"];

			//var name = addTagRequest.Name;
			//var display = addTagRequest.DisplayName;
			var tag = new Tag
			{
				Name = addTagRequest.Name,
				DisplayName = addTagRequest.DisplayName
			};

			
			return RedirectToAction("List");
		}


		//tagleri listeleme metodu
		[HttpGet]
		public async Task<IActionResult> List()
		{
			var tags = await bloggieDbContext.Tags.ToListAsync();
			return View(tags);
		}

		//Edit Sayfasının görüntülenme(get) actionu
		[HttpGet]
		public async Task<IActionResult> Edit(Guid id)
		{
			//1. metot
			//var tag = bloggieDbContext.Tags.Find(id);

			//2.metot
			var tag = await bloggieDbContext.Tags.FirstOrDefaultAsync(t => t.Id == id);

			if (tag != null)
			{
				var editTagReq = new EditTagRequest
				{
					Id = tag.Id,
					Name = tag.Name,
					DisplayName = tag.DisplayName
				};
				return View(editTagReq);
			}

			return View(null);

		}

		[HttpPost]
		public async Task<IActionResult> Edit(EditTagRequest editTagRequest)
		{
			var tag = new Tag
			{
				Id = editTagRequest.Id,
				Name = editTagRequest.Name,
				DisplayName = editTagRequest.DisplayName
			};

			var existingTag = await bloggieDbContext.Tags.FindAsync(tag.Id);

			if (existingTag != null)
			{
				existingTag.Name = tag.Name;
				existingTag.DisplayName = tag.DisplayName;

				await bloggieDbContext.SaveChangesAsync();

				//return RedirectToAction("Edit", new { id = editTagRequest.Id});
				return RedirectToAction("List");
			}

			return RedirectToAction("Edit", new { id = editTagRequest.Id });

		}

		[HttpPost]
		public async Task<IActionResult> Delete(EditTagRequest editTagRequest)
		{
			var tag = await bloggieDbContext.Tags.FindAsync(editTagRequest.Id);

			if (tag != null)
			{
				bloggieDbContext.Remove(tag);
				await bloggieDbContext.SaveChangesAsync();

				return RedirectToAction("List");
			}

			return RedirectToAction("Edit", new { id = editTagRequest.Id });
		}

	}
}