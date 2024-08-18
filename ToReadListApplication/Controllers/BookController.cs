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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, [Bind("Id,Name,Description,Author,Rate,ImageUrl,PublishDate,CategoryId")] Book book)
        {
            if (id != book.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingBook = await _dbcontext.Books.FindAsync(id);

                    if (existingBook == null)
                    {
                        return NotFound();
                    }

                    // Update only changed fields
                    _dbcontext.Entry(existingBook).CurrentValues.SetValues(book);

                    await _dbcontext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        private bool BookExists(int id)
        {
            return _dbcontext.Books.Any(e => e.Id == id);
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
