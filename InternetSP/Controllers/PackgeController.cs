using InternetSP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace InternetSP.Controllers
{
    public class PackgeController : Controller
    {
        private readonly InternetSPContext _context;

        public PackgeController(InternetSPContext context)
        {
            _context = context;
        }
        [Admin]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Packges.Include(x => x.Volume).Include(x => x.Speed).ToListAsync());
        }

        public async Task<IActionResult> View(int Id)
        {
            return View(await _context.Packges.Include(x => x.Volume).Include(x => x.Speed).Where(x => x.Id == Id).FirstOrDefaultAsync());
        }

        [Admin]
        public async Task<IActionResult> Delete(int? id)
        {
            var packge = await _context.Packges.FindAsync(id);
            if (packge != null)
            {
                _context.Packges.Remove(packge);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [Admin]

        [HttpGet]
        public async Task<IActionResult> CreateUpdate(int? id)
        {
            ViewBag.Speed = new SelectList(_context.Speeds.ToList(), "Id", "Name");
            ViewBag.Volume = new SelectList(_context.Volumes.ToList(), "Id", "Name");
            if (id == null)
            {
                return View();
            }
            else
                return View(await _context.Packges.Where(x => x.Id == id).FirstOrDefaultAsync());
        }
        [HttpPost]
        public async Task<IActionResult> CreateUpdate(Packge packge)
        {
            packge.UserId = 1;
            _context.Packges.Update(packge);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
