using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToReadListApplication.DbContextManager;
using ToReadListApplication.Models;

namespace ToReadListApplication.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _dbcontext;

        public CategoryController(ApplicationDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        public IActionResult Index()
        {
            var Categories = _dbcontext.Categories.ToList();
            return View(Categories);
        }

        // Create 
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category obj)
        {
            if (obj == null)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {
                _dbcontext.Categories.Add(obj);
                _dbcontext.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(obj);
        }
      
        // Update
        public IActionResult Update(int id)
        {
            var category = _dbcontext.Categories.Find(id); 

            return View(category);
        }

        [HttpPost]
        public IActionResult Update(Category obj) 
        {
            if (obj == null)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {
                _dbcontext.Categories.Update(obj); 
                _dbcontext.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return View(obj);
        }

        // Remove
        public IActionResult Delete(int id)
        {
            var category = _dbcontext.Categories.Find(id); 
            if (category is not null)
            {
                _dbcontext.Categories.Remove(category); 
                _dbcontext.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return NotFound();
        }
    }
}
 
