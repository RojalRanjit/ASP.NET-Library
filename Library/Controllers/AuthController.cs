using Library.Data;
using Library.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Library.Controllers
{
    public class AuthController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        public AuthController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(Author author)
        {
            var LogAuthor = _dbContext.Authors.Where(x => x.AuthorEmail == author.AuthorEmail && x.Password == author.Password).FirstOrDefault();
            if (LogAuthor != null)
            {
                HttpContext.Session.SetString("name", LogAuthor.AuthorName);
                HttpContext.Session.SetInt32("id", LogAuthor.AuthorId);
                HttpContext.Session.SetString("role", "author");
                return RedirectToAction("Display");
            }
            else 
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View();
            }
        }
        [HttpGet]
        public IActionResult UserLogin()
        {
            return View();
        }
        [HttpPost]
        public IActionResult UserLogin(User user)
        {
            var LogUser = _dbContext.Users.Where(x => x.UserEmail == user.UserEmail && x.Password == user.Password).FirstOrDefault();
            if (LogUser != null)
            {
                HttpContext.Session.SetString("name", LogUser.UserName);
                HttpContext.Session.SetInt32("id", LogUser.UserId);
                HttpContext.Session.SetString("role", "user");
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View();
            }
        }
        [HttpGet]
        public IActionResult RegisterAuthor() 
        {
            return View();
        }
        [HttpPost]
        public IActionResult RegisterAuthor(Author author)
        {
            _dbContext.Authors.Add(author);
            _dbContext.SaveChanges();
            return RedirectToAction("Login");
        }
        [HttpGet]
        public IActionResult RegisterUser() 
        {
            return View();
        }
        [HttpPost]
        public IActionResult RegisterUser(User user)
        {
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
            return RedirectToAction("Login");
        }

        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("name");
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public IActionResult Display() 
        {
            if(HttpContext.Session.GetString("role") == "user")
            {
                return RedirectToAction("Index", "Home");
            }
            var list = _dbContext.Books.Include(x => x.Genre).Where(x => x.AuthorId == HttpContext.Session.GetInt32("id")).ToList();
            //ViewBag.list = list;
            return View(list);
        }

    }
}
