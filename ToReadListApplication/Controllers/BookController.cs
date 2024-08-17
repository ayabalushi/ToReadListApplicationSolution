using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ToReadListApplication.DbContextManager;
using ToReadListApplication.Models;

namespace ToReadListApplication.Controllers
{
    public class BookController : Controller
    {
        private readonly ApplicationDbContext _dbcontext;

        public BookController(ApplicationDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public IActionResult Index()
        {
            var books = _dbcontext.Books.Include(b => b.Category).ToList();
            return View(books);
        }

        // Create
        public IActionResult Create()
        {
            // Pass categories to the view for selection
            ViewBag.CategoryId = new SelectList(_dbcontext.Categories, "Id", "Name" +
                "");
            return View();
        }

        [HttpPost]
        public IActionResult Create(Book obj)
        {
            if (obj == null)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {
                _dbcontext.Books.Add(obj);
                _dbcontext.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.CategoryId = new SelectList(_dbcontext.Categories, "Id", "Name", obj.CategoryId);
            return View(obj);
        }

        // Detailed Book
        public IActionResult Details(int id)
        {
            var book = _dbcontext.Books.Include(b => b.Category).FirstOrDefault(b => b.Id == id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        // Update
        public IActionResult Update(int id)
        {
            var book = _dbcontext.Books.Find(id);
            if (book == null)
            {
                return NotFound();
            }

            ViewBag.CategoryId = new SelectList(_dbcontext.Categories, "Id", "Name", book.CategoryId);
            return View(book);
        }


        [HttpPost]
        public IActionResult Update(Book obj)
        {
            if (obj == null)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                var existingBook = _dbcontext.Books.Find(obj.Id);
                if (existingBook == null)
                {
                    return NotFound();
                }

                // Update only the fields that are provided
                if (!string.IsNullOrEmpty(obj.Name))
                {
                    existingBook.Name = obj.Name;
                }

                if (!string.IsNullOrEmpty(obj.Description))
                {
                    existingBook.Description = obj.Description;
                }

                if (obj.Rate > 0) // Assuming rate should be a positive number
                {
                    existingBook.Rate = obj.Rate;
                }

                if (obj.CategoryId > 0) // Ensure a valid CategoryId
                {
                    // Validate CategoryId
                    if (_dbcontext.Categories.Any(c => c.Id == obj.CategoryId))
                    {
                        existingBook.CategoryId = obj.CategoryId;
                    }
                    else
                    {
                        ModelState.AddModelError("CategoryId", "Selected category does not exist.");
                        ViewBag.CategoryId = new SelectList(_dbcontext.Categories, "Id", "Name", obj.CategoryId);
                        return View(obj);
                    }
                }

                _dbcontext.Books.Update(existingBook);
                _dbcontext.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.CategoryId = new SelectList(_dbcontext.Categories, "Id", "Name", obj.CategoryId);
            return View(obj);
        }

        // Delete
        public IActionResult Delete(int id)
        {
            var book = _dbcontext.Books.Find(id);
            if (book == null)
            {
                return NotFound();
            }

            _dbcontext.Books.Remove(book);
            _dbcontext.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult AddToReadList(int id)
        {
            var book = _dbcontext.Books.Find(id); // Fetch the book from the database

            if (book == null)
            {
                return NotFound();
            }

            // Check if the book is already in the ToReadList
            var existingItem = _dbcontext.ToReadList
                .FirstOrDefault(t => t.BookId == book.Id);

            if (existingItem == null)
            {
                // Create a new ToReadList item
                var toReadListItem = new ToReadList
                {
                    BookId = book.Id,
                    Status = "Pending" // Default status
                };

                _dbcontext.ToReadList.Add(toReadListItem);
                _dbcontext.SaveChanges();
            }

            return RedirectToAction("Index", "ToReadList"); // Redirect to ToReadList Index page
        }

        [HttpPost]
        public IActionResult UpdateStatus(int id, string newStatus)
        {
            var item = _dbcontext.ToReadList.Find(id);

            if (item == null)
            {
                return NotFound();
            }

            item.Status = newStatus;
            _dbcontext.SaveChanges();

            return RedirectToAction("Index");
        }


    }
}
