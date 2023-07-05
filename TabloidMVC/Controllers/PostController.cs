using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using System.Security.Claims;
using TabloidMVC.Models.ViewModels;
using TabloidMVC.Repositories;
using TabloidMVC.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Xml.Linq;

namespace TabloidMVC.Controllers
{
    [Authorize]
    public class PostController : Controller
    {
        private readonly IPostRepository _postRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly IUserProfileRepository _userProfileRepository;

        // ASP.NET will give us an instance of our Post Repository. This is called "Dependency Injection"
        public PostController(IPostRepository postRepository, ICategoryRepository categoryRepository, ICommentRepository commentRepository, IUserProfileRepository userProfileRepository)
        {
            _postRepository = postRepository;
            _categoryRepository = categoryRepository;
            _commentRepository = commentRepository;
            _userProfileRepository = userProfileRepository;
        }

        // GET: PostController

        public IActionResult Index()
        {
            var posts = _postRepository.GetAllPublishedPosts();
            return View(posts);
        }

        public IActionResult MyIndex()
        {
            int userId = GetCurrentUserProfileId();
            var posts = _postRepository.GetAllUserPosts(userId);
            return View(posts);
        }

        //GET: PostController/Details/5

        public IActionResult Details(int id)
        {
            // Retrieve the post and related comments from your data source
            var post = _postRepository.GetPublishedPostById(id);
            var comments = _commentRepository.GetCommentsByPostId(id);
            //var user = _commentRepository.GetUserDisplayName(id);

            // Create an instance of the PostCommentViewModel and assign the post and comments
            var viewModel = new PostCommentViewModel
            {
                Post = post,
                Comments = comments,
               
            };

            return View(viewModel);
        }



        //GET: PostController/Create
        public IActionResult Create()
        {
            var vm = new PostCreateViewModel();
            vm.CategoryOptions = _categoryRepository.GetAll();
            return View(vm);
        }

        //POST: PostController/Create

        [HttpPost]
        public IActionResult Create(PostCreateViewModel vm)
        {
            try
            {
                vm.Post.CreateDateTime = DateAndTime.Now;
                vm.Post.IsApproved = true;
                vm.Post.UserProfileId = GetCurrentUserProfileId();

                _postRepository.Add(vm.Post);

                return RedirectToAction("Details", new { id = vm.Post.Id });
            }
            catch
            {
                vm.CategoryOptions = _categoryRepository.GetAll();
                return View(vm);
            }
        }

        // GET: Posts/Edit/5
        public IActionResult Edit(int id) 
        {
            var vm = new PostEditViewModel();
            vm.Post = _postRepository.GetPublishedPostById(id);
            vm.CategoryOptions = _categoryRepository.GetAll();

            if (vm.Post == null)
            {
                return RedirectToAction("Index");
            }
            return View(vm);

            
        }

        //POST: Post/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit (int id, Post post)
        {
            try
            {
                _postRepository.UpdatePost(post);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(post);
            }
        }


        //GET: Post/Delete/5
        public IActionResult Delete(int id) 
        {
            Post post = _postRepository.GetPublishedPostById(id);
            return View(post);
        }

        //POST: Post/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, Post post)
        {
            try
            {
                _postRepository.DeletePost(id);
                return RedirectToAction("Index");
            }
            catch (Exception ex) 
            {
                return View(post);
            }
        }

        private int GetCurrentUserProfileId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }
    }
}