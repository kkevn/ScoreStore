using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ScoreStore.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ScoreStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ILogger<HomeController> logger, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult UserInfo()
        {
            // obtain reference to currently logged in user by Id
            var userId = _userManager.GetUserId(HttpContext.User);

            // redirect user to log in if not already signed in
            if (userId == null)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }
            else
            {
                // obtain the user with Id and pass to the view
                ApplicationUser currentUser = _userManager.FindByIdAsync(userId).Result;
                return View(currentUser);
            }
        }

        public IActionResult SearchUsers()
        {
            // redirect user to log in if not already signed in
            if (_userManager.GetUserId(HttpContext.User) == null)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }
            else
            {
                return View();
            }
        }

        public IActionResult SearchUsersResults(String SearchInput)
        {
            // obtain reference to currently logged in user by Id
            var userId = _userManager.GetUserId(HttpContext.User);

            // redirect user to log in if not already signed in
            if (userId == null)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }
            else
            {
                // obtain the user with Id
                var currentUser = _userManager.FindByIdAsync(userId).Result;

                // obtain list of all users in database
                var users = _userManager.Users;

                // filter list of users matching search input phrase
                users = users.Where(u => u.Name.Contains(SearchInput) || u.NormalizedEmail.Contains(SearchInput));

                // filter list of users not already in current user's friend list
                users = users.Where(u => !currentUser.FriendList.Contains(u.Id));

                // filter list of users no not include current user
                users = users.Where(u => !currentUser.Id.Equals(u.Id));

                // return subset of users with the above filters
                return View(users);
            }
        }

        public IActionResult FriendList()
        {
            // obtain reference to currently logged in user by Id
            var userId = _userManager.GetUserId(HttpContext.User);

            // redirect user to log in if not already signed in
            if (userId == null)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }
            else
            {
                // obtain the user with Id
                var currentUser = _userManager.FindByIdAsync(userId).Result;

                // obtain list of all users in database
                var users = _userManager.Users;

                // return subset of users with Id contained in friend list of current user
                return View(users.Where(u => currentUser.FriendList.Contains(u.Id)));
            }
        }

        public async Task<IActionResult> AddFriend()
        {
            // obtain the friend Id to add from the route data
            var friendId = Url.ActionContext.RouteData.Values["id"];

            // obtain reference to currently logged in user by Id
            var userId = _userManager.GetUserId(HttpContext.User);

            // redirect user to log in if not already signed in
            if (userId == null)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }
            else
            {
                // obtain the user with Id
                var currentUser = await _userManager.FindByIdAsync(userId);

                // append selected friend Id with a delimiter to user's friend list
                string delimiter = ",";
                if (currentUser.FriendList != null)
                    currentUser.FriendList += (friendId + delimiter);
                else
                    currentUser.FriendList = friendId + delimiter;

                // update user on database
                await _userManager.UpdateAsync(currentUser);

                // logging message for debugging purposes
                System.Diagnostics.Debug.WriteLine("\t==> Current user {" + userId + "} added user {" + friendId + "} as a friend");

                // obtain list of all users in database
                var users = _userManager.Users;

                // return FriendList view with model of subset of users with Id contained in friend list of current user
                return View("FriendList", users.Where(u => currentUser.FriendList.Contains(u.Id)));
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
