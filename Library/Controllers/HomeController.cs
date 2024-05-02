using Library.Data;
using Library.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Library.Controllers
{
    public class HomeController : Controller
    {

        private readonly ApplicationDbContext _dbContext;
        public HomeController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IActionResult Index(string search, string Genre, string Author, string Language)
        {
                
                var GenreList = _dbContext.Genres.ToList();
                var AuthorList = _dbContext.Authors.ToList();
                ViewBag.GenreList = GenreList;
                ViewBag.AuthorList = AuthorList;

                IEnumerable<Book> datas = _dbContext.Books.Include(x => x.Author).Include(b=>b.Genre).ToList();
                IEnumerable<Book> reversedData = datas.Reverse();
                if (!string.IsNullOrEmpty(search))
                {
                    reversedData = reversedData.Where(x => x.Title.StartsWith(search, StringComparison.OrdinalIgnoreCase));
                }
                if (!string.IsNullOrEmpty(Genre))
                {
                    reversedData = reversedData.Where(x => x.Genre.Name.StartsWith(Genre, StringComparison.OrdinalIgnoreCase));
                }
                if (!string.IsNullOrEmpty(Author))
                {
                    reversedData = reversedData.Where(x => x.Author.AuthorName.StartsWith(Author, StringComparison.OrdinalIgnoreCase));
                }
                if (!string.IsNullOrEmpty(Language))
                {
                    reversedData = reversedData.Where(x => x.Language.StartsWith(Language, StringComparison.OrdinalIgnoreCase));
                }

                // Convert IEnumerable<Book> to List<Book>
                List<Book> bookList = reversedData.ToList();

                return View(bookList); // Passing List<Book> to the View
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
