using Library.Data;
using Library.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

namespace Library.Controllers
{
    public class BookController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public BookController(ApplicationDbContext dbContext, IWebHostEnvironment webHostEnvironment)
        {
            _dbContext = dbContext;
            _webHostEnvironment = webHostEnvironment;
        }
        [HttpGet]
        public IActionResult AddBook()
        {
            var gen = _dbContext.Genres.ToList();
            return View(gen);
        }
        [HttpPost]
        public IActionResult AddBook(Book book,IFormFile Image)
        {
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            if (Image != null)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(Image.FileName);
                string productPath = Path.Combine(wwwRootPath, @"Uploads");
                string filePath = Path.Combine(productPath, fileName);
                string uploadFolder = Path.Combine(wwwRootPath, "Uploads");

             

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    Image.CopyTo(fileStream);
                }

                book.Image = @"/Uploads/" + fileName;
            }
            book.AuthorId = HttpContext.Session.GetInt32("id") ?? 0;
            if (book.AuthorId == 0)
            {
                return RedirectToAction("AddBook");
            }
            _dbContext.Books.Add(book);
            _dbContext.SaveChanges();
            return RedirectToAction("Display","Auth");
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var list = _dbContext.Books.Include(x => x.Genre).Where(bk => bk.BookId == id).FirstOrDefault();
            return View(list);
        }
        [HttpPost]
        public IActionResult Edit(Book book, IFormFile Image)
        {
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            if (Image != null)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(Image.FileName);
                string productPath = Path.Combine(wwwRootPath, @"Uploads");
                string filePath = Path.Combine(productPath, fileName);
                string uploadFolder = Path.Combine(wwwRootPath, "Uploads");



                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    Image.CopyTo(fileStream);
                }

                book.Image = @"/Uploads/" + fileName;
            }
            book.AuthorId = HttpContext.Session.GetInt32("id") ?? 0;
            var update = _dbContext.Books.Find(book.BookId);
            update.Title = book.Title;
            update.Genre = book.Genre;
            update.Image = book.Image;
            update.Language = book.Language;
            _dbContext.SaveChanges();
            return RedirectToAction("Display", "Auth");
        }
        public IActionResult Delete(int Id)
        {
            var data = _dbContext.Books.Find(Id);
            _dbContext.Books.Remove(data);
            _dbContext.SaveChanges();
            return RedirectToAction("Display","Auth");
        }
        [HttpGet]
        public IActionResult AddGenre()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddGenre(Genre genre, IFormFile Image)
        {
            foreach (var Data in genre.Books)
            {
                Data.AuthorId = 1;
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (Image != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(Image.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"Uploads");
                    string filePath = Path.Combine(productPath, fileName);
                    string uploadFolder = Path.Combine(wwwRootPath, "Uploads");

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        Image.CopyTo(fileStream);
                    }

                    Data.Image = @"/Uploads/" + fileName;
                }
                Data.AuthorId = HttpContext.Session.GetInt32("id") ?? 0;
                if (Data.AuthorId == 0)
                {
                    return RedirectToAction("AddBook");
                }
            }
            _dbContext.Genres.Add(genre);
            _dbContext.SaveChanges();
            return RedirectToAction("AddBook");
        }
        [HttpGet]
        public IActionResult BookDetails(int id)
        {
            var data = _dbContext.Books.Include(x => x.Genre).Include(x => x.Author).Where(x => x.BookId == id).FirstOrDefault();
            return View(data);
        }
    }
}
