﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ScoreStore.Data;
using ScoreStore.Helpers;
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
                bool gameAdded = false;
                if (currentUser.GameList != null)
                {
                    if (!currentUser.GameList.Contains(gameToAdd))
                    {
                        currentUser.GameList += gameToAdd;
                        gameAdded = true;
                    }
                } else {
                    currentUser.GameList = gameToAdd;
                    gameAdded = true;
                }

                // add a score entry for this user and game
                if (gameAdded)
                {
                    // parse obtained game Id into integer
                    int gameIdVal = Int32.Parse(gameId.ToString());

                    // obtain list of all scores in database
                    var scores = _context.Scores;
                    
                    // add new score entry if one does not exist for this user and game combination
                    if (scores.Where(s => s.UserId.Equals(userId) && s.GameId == gameIdVal).FirstOrDefault() == null)
                    {
                        scores.Add(new Scores(userId, gameIdVal));
                        await _context.SaveChangesAsync();
                    }

                    // update user on database
                    await _userManager.UpdateAsync(currentUser);

                    // logging message for debugging purposes
                    System.Diagnostics.Debug.WriteLine("\t==> Current user {" + userId + "} added game {" + gameId + "} to user's game list");
                } else {
                    // logging message for debugging purposes
                    System.Diagnostics.Debug.WriteLine("\t==> Current user {" + userId + "} already had game {" + gameId + "} added to user's game list");

                    // TODO: notify user game already existed in their game list
                }

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

        public IActionResult ViewGame(String Id)
        {
            // obtain reference to currently logged in user by Id
            var userId = _userManager.GetUserId(HttpContext.User);

            // parse obtained game Id into integer
            int gameIdVal = Int32.Parse(Id.ToString());

            // redirect user to log in if not already signed in
            if (userId == null)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }
            else
            {
                // obtain the user with Id
                var currentUser = _userManager.FindByIdAsync(userId).Result;

                // obtain list of all scores in database
                var scores = _context.Scores;

                // obtain score entry for this user and game combination and store match totals in a view bag
                var score = scores.Where(s => s.UserId.Equals(userId) && s.GameId == gameIdVal).FirstOrDefault();
                ViewBag.Matches = score.Wins + score.Losses;

                // store game title and cover art URL in a viewbag
                var game = _context.Game.Where(g => g.Id == gameIdVal).FirstOrDefault();
                ViewBag.Title = game.Title;
                ViewBag.ImageURL = game.ImageURL;

                //var ratio = ChartHelper.CalculateWinRatio(score.Wins, score.Losses);

                // obtain top 5 score win entries for current user and friends for this game
                var score_cols = scores.Where(s => s.GameId == gameIdVal);
                score_cols = score_cols.Where(s => currentUser.FriendList.Contains(s.UserId) || userId.Equals(s.UserId));
                score_cols = score_cols.OrderByDescending(s => s.Wins).Take(5);

                // store list as string in view data
                var columns = score_cols.Select(s => new { s.UserId, s.Wins }).ToList();
                //columns = ChartHelper.FriendColumn(score_cols);
                ViewData["columns"] = String.Join("_", columns);

                //ViewData["area"] = "chris,kev,chris,ray,chris,";
                //ViewData["area"] = score.StreakList;

                ViewBag.UserName = currentUser.Name;

                // return score entry
                return View(score);
            }
        }

        public IActionResult AddScore(String Id)
        {
            // obtain reference to currently logged in user by Id
            var userId = _userManager.GetUserId(HttpContext.User);

            // parse obtained game Id into integer
            int gameIdVal = Int32.Parse(Id.ToString());

            // redirect user to log in if not already signed in
            if (userId == null)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }
            else
            {
                // store game Id, title and cover art URL of current game in a viewbag
                var game = _context.Game.Where(g => g.Id == gameIdVal).FirstOrDefault();
                ViewBag.GameId = game.Id;
                ViewBag.Title = game.Title;
                //ViewBag.ImageURL = game.ImageURL;

                // obtain the user with Id
                var currentUser = _userManager.FindByIdAsync(userId).Result;

                // obtain list of all users in database
                var users = _userManager.Users;

                // return subset of users with Id contained in friend list of current user
                return View(users.Where(u => currentUser.FriendList.Contains(u.Id)));
            }
        }

        public async Task<IActionResult> SubmitScore(int Game, String Id)
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

                // obtain list of all scores in database
                var scores = _context.Scores;

                // obtain score entry for this user and game combination
                var score = scores.Where(s => s.UserId.Equals(userId) && s.GameId == Game).FirstOrDefault();

                // mark current user ID as default winner
                string winningId = userId;

                // increment Wins counter if current user won, otherwise add loss
                if (Id.Equals("{0}")) {
                    score.Wins++;
                } else {
                    score.Losses++;
                    winningId = Id; // update ID of winning user (will be {x} if 'Other' was selected)
                }

                // append Id of winning user with a delimiter to this score's streak list
                string delimiter = ",";
                if (score.StreakList != null)
                    score.StreakList += (winningId + delimiter);
                else
                    score.StreakList = winningId + delimiter;

                // update score on database
                await _context.SaveChangesAsync();

                // append Id of winning user with a delimiter to this user's streak list
                if (currentUser.StreakList != null)
                    currentUser.StreakList += (winningId + delimiter);
                else
                    currentUser.StreakList = winningId + delimiter;

                // update user on database
                await _userManager.UpdateAsync(currentUser);

                // logging message for debugging purposes
                System.Diagnostics.Debug.WriteLine("\t==> Current user {" + userId + "} added score for game {" + Game + "} with winner {" + winningId + "}");

                // return ViewGame view with game Id of score being submitted for
                return RedirectToAction("ViewGame", new { Id = Game });
            }
        }

        public IActionResult ViewStats()
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

                // obtain list of all scores in database
                var scores = _context.Scores;
                var games = _context.Game;

                // obtain all score entries for this user
                var user_scores = scores.Where(s => s.UserId.Equals(userId));

                // store user's total wins, losses and matches in a viewbag
                var wins = user_scores.Sum(s => s.Wins);
                var losses = user_scores.Sum(s => s.Losses);
                ViewBag.Wins = wins;
                ViewBag.Losses = losses;
                ViewBag.Matches = wins + losses;

                // get selection of user's games and their respective win counts
                var user_wins = user_scores.Select(s => new { s.GameId, s.Wins });

                // join result by game ID to obtain list containing game titles instead
                var user_wins_joined = games.Join(user_wins,
                    g => g.Id,
                    uw => uw.GameId,
                    (g, uw) => new {
                        GameName = g.Title,
                        GameWins = uw.Wins
                    }).ToList();

                // store result in view data
                ViewData["columns"] = String.Join("_", user_wins_joined);

                return View(currentUser);
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
