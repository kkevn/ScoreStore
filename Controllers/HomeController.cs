using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ScoreStore.Data;
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
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _logger = logger;
            _userManager = userManager;
            _context = context;
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

        /**** FriendList Functions ****/

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

        public async Task<IActionResult> RemoveFriend(String Id)
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
                var currentUser = await _userManager.FindByIdAsync(userId);

                // remove selected friend Id with a delimiter from user's friend list
                string delimiter = ",";
                if (currentUser.FriendList.Contains(Id))
                    currentUser.FriendList = currentUser.FriendList.Replace(Id + delimiter, "");

                // update user on database
                await _userManager.UpdateAsync(currentUser);

                // logging message for debugging purposes
                System.Diagnostics.Debug.WriteLine("\t==> Current user {" + userId + "} removed user {" + Id + "} as a friend");

                // obtain list of all users in database
                var users = _userManager.Users;

                // return FriendList view with model of subset of users with Id contained in friend list of current user
                return View("FriendList", users.Where(u => currentUser.FriendList.Contains(u.Id)));
            }
        }

        /**** GameList Functions ****/

        public IActionResult GameList()
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

                // obtain list of all games in database
                var games = _context.Game;

                // return subset of games with Id contained in game list of current user
                return View(games.Where(g => currentUser.GameList.Contains("{" + g.Id + "}")));
            }
        }

        public async Task<IActionResult> AddGame()
        {
            // obtain the game Id to add from the route data
            var gameId = Url.ActionContext.RouteData.Values["id"];

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

                // append selected game Id with curly braces to user's game list if it doesn't exist
                string gameToAdd = "{" + gameId + "}";
                if (currentUser.GameList != null) {
                    if (!currentUser.GameList.Contains(gameToAdd))
                        currentUser.GameList += gameToAdd;
                }
                else
                    currentUser.GameList = gameToAdd;

                // update user on database
                await _userManager.UpdateAsync(currentUser);

                // logging message for debugging purposes
                System.Diagnostics.Debug.WriteLine("\t==> Current user {" + userId + "} added game {" + gameId + "} to user's game list");

                // obtain list of all games in database
                var games = _context.Game;

                // return GameList view with model of subset of games with Id contained in game list of current user
                return View("GameList", games.Where(g => currentUser.GameList.Contains("{" + g.Id + "}")));
            }
        }

        public async Task<IActionResult> RemoveGame(String Id)
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
                var currentUser = await _userManager.FindByIdAsync(userId);

                // remove selected game Id with curly braces from user's game list
                if (currentUser.GameList.Contains("{" + Id + "}"))
                    currentUser.GameList = currentUser.GameList.Replace("{" + Id + "}", "");

                // update user on database
                await _userManager.UpdateAsync(currentUser);

                // logging message for debugging purposes
                System.Diagnostics.Debug.WriteLine("\t==> Current user {" + userId + "} removed game {" + Id + "} from user's game list");

                // obtain list of all games in database
                var games = _context.Game;

                // return GameList view with model of subset of games with Id contained in game list of current user
                return View("GameList", games.Where(g => currentUser.GameList.Contains("{" + g.Id + "}")));
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
