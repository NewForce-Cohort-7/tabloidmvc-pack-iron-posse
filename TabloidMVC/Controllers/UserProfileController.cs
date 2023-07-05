using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TabloidMVC.Models;
using TabloidMVC.Repositories;

namespace TabloidMVC.Controllers
{
    public class UserProfileController : Controller
    {

        // GET: UserProfileController
        private readonly IUserProfileRepository _userProfileRepo;

        public UserProfileController(IUserProfileRepository userProfileRepository)
        {
            _userProfileRepo = userProfileRepository;
        }
        public ActionResult Index()
        {
            List<UserProfile> userProfiles = _userProfileRepo.GetAllUserProfiles();

            return View(userProfiles);
        }

        public IActionResult Details(int id)
        {
            var userProfile = _userProfileRepo.GetUserProfileById(id);
            if (userProfile == null)
            {
                userProfile = _userProfileRepo.GetUserProfileById(id);
                if (userProfile == null)
                {
                    return NotFound();
                }
            }
            return View(userProfile);
        }

    }
}
