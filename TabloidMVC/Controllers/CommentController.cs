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
    public class CommentController : Controller
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IPostRepository _postRepositroy;
        public CommentController(ICommentRepository commentRepository, IPostRepository postRepositroy)
        {
            _commentRepository = commentRepository;
            _postRepositroy = postRepositroy;
        }

        // GET: /Comment/Index/{id}
        public IActionResult Index(int id)
        {
            // Retrieve comments by post ID
            var comments = _commentRepository.GetCommentsByPostId(id);
            return View(comments);
        }

        // GET: /Comment/Details/{id}
        public IActionResult Details(int id)
        {
            var comment = _commentRepository.GetCommentById(id);
            if (comment == null)
            {
                // Comment not found, return NotFound result
                return NotFound();
            }
            return View(comment);
        }

        // GET: /Comment/Create/{id}
        public IActionResult Create(int id)
        {
            var vm = new PostCommentViewModel();
            vm.Post = _postRepositroy.GetPublishedPostById(id);
            return View(vm);
        }

        // POST: /Comment/Create
        [HttpPost]
        public IActionResult Create(PostCommentViewModel vm)
        {
            try
            {
                vm.Comment.CreateDateTime = DateAndTime.Now;
                vm.Comment.UserProfileId = GetCurrentUserProfileId();

                // Add the comment
                _commentRepository.AddComment(vm.Comment);

                // Redirect to the details page of the post
                return RedirectToAction("Details", "Post", new { id = vm.Comment.PostId });
            }
            catch
            {
                vm.Post = _postRepositroy.GetPublishedPostById(vm.Comment.PostId);
                return View(vm);
            }
        }

        // GET: /Comment/Edit/{id}
        public IActionResult Edit(int id)
        {
            var comment = _commentRepository.GetCommentById(id);
            if (comment == null)
            {
                // Comment not found, return NotFound result
                return NotFound();
            }
            return View(comment);
        }

        // POST: /Comment/Edit/{id}
        [HttpPost]
        public IActionResult Edit(int id, Comment comment)
        {
            try
            {
                // Update the comment
                _commentRepository.UpdateComment(comment);

                // Redirect to the details page of the post
                return RedirectToAction("Details", "Post", new { id = comment.PostId });
            }
            catch
            {
                return View(comment);
            }
        }

        // GET: /Comment/Delete/{id}
        public IActionResult Delete(int id)
        {
            var comment = _commentRepository.GetCommentById(id);
            if (comment == null)
            {
                // Comment not found, return NotFound result
                return NotFound();
            }
            return View(comment);
        }

        // POST: /Comment/Delete/{id}
        [HttpPost]
        public IActionResult Delete(int id, Comment comment)
        {
            try
            {
                // Delete the comment
                _commentRepository.DeleteComment(id);

                // Redirect to the details page of the post
                return RedirectToAction("Details", "Post", new { id = comment.PostId });
            }
            catch
            {
                return View(comment);
            }
        }

        // Method to get the current user's profile ID
        private int GetCurrentUserProfileId()
        {
            var currentUser = GetCurrentUser();
            return int.Parse(currentUser.FindFirstValue("id"));
        }

        // Method to get the current user's claims
        private ClaimsPrincipal GetCurrentUser()
        {
            return this.HttpContext.User;
        }
    }
}
