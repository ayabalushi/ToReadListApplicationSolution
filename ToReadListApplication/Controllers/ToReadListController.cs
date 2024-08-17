using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToReadListApplication.DbContextManager;

namespace ToReadListApplication.Controllers
{
    public class ToReadListController : Controller
    {
        private readonly ApplicationDbContext _dbcontext;

        public ToReadListController(ApplicationDbContext context)
        {
            _dbcontext = context;
        }
        public IActionResult Index()
        {
            var toReadList = _dbcontext.ToReadList.Include(t => t.Book).ToList();
            return View(toReadList);
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

