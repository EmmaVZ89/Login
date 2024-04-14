using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using UsersApp.Models;
using UsersApp.DataDB;
using UsersApp.Services;
using UsersApp.Models.User;
using UsersApp.DataDB.User;
using Microsoft.AspNetCore.Http.Extensions;
using UsersApp.Models.Email;

namespace UsersApp.Controllers
{
    public class StartController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public StartController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public ActionResult Login()
        {
            var email = Request.Cookies["LoggedInEmail"];
            var token = Request.Cookies["LoggedInToken"];

            if (!String.IsNullOrEmpty(email) && !String.IsNullOrEmpty(token))
            {
                var userLogged = DBUser.ValidateLoggedUser(email, token);

                if (userLogged != null)
                {
                    userLogged.LastLoggedInAt = DateTime.Now;

                    var result = DBUser.Update(userLogged);
                    if (result)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }

            return View();
        }

        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            UserDTO user = DBUser.Validate(email, UtilsService.ConvertToSHA256(password));

            if (user != null)
            {
                if (!user.ConfirmPassword)
                {
                    ViewBag.Message = $"Your account needs to be confirmed, an email has been sent to {email}";
                }
                else if (user.RestorePassword)
                {
                    ViewBag.Message = $"Your account has been requested to be reset, please check your email {email}";
                }
                else
                {
                    var cookieOptions = new CookieOptions
                    {
                        Expires = DateTime.UtcNow.AddMonths(1),
                        HttpOnly = true, // Mark the cookie as HttpOnly to prevent it from being accessed from JavaScript
                        IsEssential = true // Mark the cookie as essential to ensure it is added even if cookies are disabled in the browser
                    };

                    // If you are in a production environment, you can also set Secure to true so that the cookie is only sent through
                    if (Request.IsHttps)
                    {
                        cookieOptions.Secure = true; // Mark the cookie as Secure so that it is only sent over HTTPS connections
                    }

                    Response.Cookies.Append("LoggedInEmail", email ?? "", cookieOptions);
                    Response.Cookies.Append("LoggedInToken", user.Token ?? "", cookieOptions);

                    user.LastLoggedInAt = DateTime.Now;

                    var result = DBUser.Update(user);
                    if (result)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            else
            {
                ViewBag.Message = "No considences were found with those credentials";
            }

            return View();
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(UserDTO user)
        {
            if (user.Password != user.ConfirmedPassword)
            {
                ViewBag.FirstName = user.FirstName;
                ViewBag.LastName = user.LastName;
                ViewBag.Email = user.Email;
                ViewBag.Message = "Passwords do not match";
                return View();
            }

            if (DBUser.Get(user.Email) == null)
            {
                user.Password = UtilsService.ConvertToSHA256(user.Password);
                user.Token = UtilsService.GenerateToken();
                user.RestorePassword = false;
                user.ConfirmPassword = false;
                bool result = DBUser.Register(user);

                if (result)
                {
                    string path = Path.Combine(_webHostEnvironment.ContentRootPath, "Templates", "Confirm.html");
                    string content = System.IO.File.ReadAllText(path);
                    string url = string.Format("{0}://{1}{2}", HttpContext.Request.Scheme, Request.Headers["host"], "/Start/Confirm?token=" + user.Token);

                    string htmlBody = string.Format(content, user.FirstName, url);

                    EmailDTO emailDTO = new EmailDTO()
                    {
                        To = user.Email,
                        About = "Confirmation email",
                        Content = htmlBody
                    };

                    bool sent = EmailService.Send(emailDTO);
                    ViewBag.Created = true;
                    ViewBag.Message = $"Your account has been created. We have sent a message to {user.Email} to confirm your account.";
                }
                else
                {
                    ViewBag.Message = "Your account could not be created";
                }
            }
            else
            {
                ViewBag.Message = "The email is already registered";
            }

            return View();
        }

        [HttpGet]
        public ActionResult Confirm(string token)
        {
            ViewBag.Response = DBUser.Confirm(token);
            return View();
        }

        [HttpGet]
        public ActionResult Restore()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Restore(string email)
        {
            UserDTO user = DBUser.Get(email);
            ViewBag.Email = email;
            if (user != null)
            {
                bool result = DBUser.RestoreUpdate(1, user.Password, user.Token);
                if (result)
                {
                    string path = Path.Combine(_webHostEnvironment.ContentRootPath, "Templates", "Restore.html");
                    string content = System.IO.File.ReadAllText(path);
                    string url = string.Format("{0}://{1}{2}", HttpContext.Request.Scheme, Request.Headers["host"], "/Start/Update?token=" + user.Token);

                    string htmlBody = string.Format(content, user.FirstName, url);

                    EmailDTO emailDTO = new EmailDTO()
                    {
                        To = email,
                        About = "Restore account",
                        Content = htmlBody
                    };

                    bool sent = EmailService.Send(emailDTO);
                    ViewBag.Restored = true;
                }
                else
                {
                    ViewBag.Message = "Account could not be reset";
                }
            }
            else
            {
                ViewBag.Message = "No matches found for email";
            }
            return View();
        }

        [HttpGet]
        public ActionResult Update(string token)
        {
            ViewBag.Token = token;
            return View();
        }

        [HttpPost]
        public ActionResult Update(string token, string password, string confirmedPassword)
        {
            ViewBag.Token = token;
            if (password != confirmedPassword)
            {
                ViewBag.Message = "Passwords do not match";
                return View();
            }

            bool result = DBUser.RestoreUpdate(0, UtilsService.ConvertToSHA256(password), token);

            if (result)
            {
                ViewBag.Restored = true;
            }
            else
            {
                ViewBag.Message = "Could not update";
            }

            return View();
        }

    }
}
