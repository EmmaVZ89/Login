using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using UsersApp.DataDB.User;
using UsersApp.Models;
using UsersApp.Models.User;

namespace UsersApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var email = Request.Cookies["LoggedInEmail"];
            var token = Request.Cookies["LoggedInToken"];

            if (!String.IsNullOrEmpty(email) && !String.IsNullOrEmpty(token))
            {
                var userLogged = DBUser.ValidateLoggedUser(email, token);

                if (userLogged != null)
                {
                    return View();
                }
            }

            return RedirectToAction("Login", "Start");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}