﻿using Microsoft.AspNetCore.Mvc;
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
        //GET for listing all
        public ActionResult Index() 
        {
            List<Category> categories = _categoryRepo.GetAll();
            return View(categories);
        }
        //GET for Creating categories
        public IActionResult Create() 
        {
            return View();
        } 
        //POST for creating categories
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create (Category category) 
        {
            try 
            {
                _categoryRepo.Add(category);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(category);
            }

        }
        //GET for deleting categories 
        public ActionResult Delete(int id) 
        {
            Category category = _categoryRepo.GetCategoryById(id);
            return View(category);
        }
        //POST for deleting categories
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete (int id, Category category)
        {
            try
            {
                _categoryRepo.DeleteCategory(id);
                return RedirectToAction("Index");
            }
            catch (Exception ex) 
            {
                return View(category);
            }
        }

    }
}
