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

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            UserDTO user = DBUser.Validate(email, UtilsService.ConvertToSHA26(password));

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
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                ViewBag.Message = "No considences were found with those credentials";
            }

            return View();
        }

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
                user.Password = UtilsService.ConvertToSHA26(user.Password);
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
    }
}
