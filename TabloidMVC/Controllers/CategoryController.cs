using Microsoft.AspNetCore.Mvc;
using TabloidMVC.Models;
using TabloidMVC.Repositories;

namespace TabloidMVC.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepo;

        public CategoryController(ICategoryRepository categoryRepo) 
        {
            _categoryRepo = categoryRepo;
        }
        //GET
        public ActionResult Index() 
        {
            List<Category> categories = _categoryRepo.GetAll();
            return View(categories);
        }

    }
}
