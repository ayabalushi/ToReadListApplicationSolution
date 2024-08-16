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
            return View();
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
      
        // تحديث الفئة
        public IActionResult Update(int id)
        {
            var category = _dbcontext.Categories.Find(id); // البحث عن الفئة للتعديل

            return View(category);
        }

        [HttpPost]
        public IActionResult Update(Category obj) // تغيير النوع إلى Category
        {
            if (obj == null)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {
                _dbcontext.Categories.Update(obj); // تحديث الفئة
                _dbcontext.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return View(obj);
        }

        // حذف الفئة
        public IActionResult Delete(int id)
        {
            var category = _dbcontext.Categories.Find(id); // البحث عن الفئة للحذف
            if (category is not null)
            {
                _dbcontext.Categories.Remove(category); // حذف الفئة
                _dbcontext.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return NotFound();
        }
    }
}
 
