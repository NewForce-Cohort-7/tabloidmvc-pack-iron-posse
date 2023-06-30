using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using System.Security.Claims;
using TabloidMVC.Models;
using TabloidMVC.Models.ViewModels;
using TabloidMVC.Repositories;

namespace TabloidMVC.Controllers
{
    [Authorize]
    public class TagController : Controller
    {
        private readonly ITagRepository _tagRepo;
    
        public TagController(ITagRepository tagRepository)
        {
            _tagRepo = tagRepository;
        }

        public IActionResult Index()
        {
            //Need to correct the get all statement
            var tags = _tagRepo.GetAllTags();

            return View(tags);
        }

        public IActionResult Details(int id)
        {
            //Need to correct the get all statement to be get by id
            Tag tag = _tagRepo.GetTagById(id);
            if (tag == null)
            {
                return NotFound();
            }

            return View(tag);
        }

        //GET : Tag/Create
        public IActionResult Create()
        {
            //List<Tag> tags = _tagRepo.GetAll();

            //return View(tags);
            return View();
        }

        //POST:TagController/Create
        //POST:Tag/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Tag tag)
        {
            try
            {
                _tagRepo.AddTag(tag);

                return RedirectToAction("Index");
            }
            catch(Exception ex) 
            {
                return View(tag);
            }
        }

        //GET: TagController/Edit/id
        public IActionResult Edit(int id)
        {
            Tag tag = _tagRepo.GetTagById(id);

            if (tag == null)
            {
                return NotFound();
            }

            return View(tag);
        }

        //POST: Tag/Edit/Id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Tag tag)
        {
            try
            {
                _tagRepo.UpdateTag(tag);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(tag);
            }
        }

        // GET: Tag/Delete/Id
        public ActionResult Delete(int id)
        {
            Tag tag = _tagRepo.GetTagById(id);

            return View(tag);
        }

        //POST: Tag/Delete/Id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, Tag tag)
        {
            try
            {
                _tagRepo.DeleteTag(id);

                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                return View(tag);
            }
        }
    }
}
