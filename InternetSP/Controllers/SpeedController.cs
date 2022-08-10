using InternetSP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InternetSP.Controllers
{
    [Admin]
    public class SpeedController : Controller
    {
        private readonly InternetSPContext _context;
        public SpeedController(InternetSPContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.Speeds.ToListAsync());
        }

        public async Task<IActionResult> Delete(int? id)
        {
            var speed = await _context.Speeds.FindAsync(id);
            if (speed != null)
            {
                _context.Speeds.Remove(speed);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> CreateUpdate(int? id)
        {
            if (id == null)
            {
                return View();
            }
            else
                return View(await _context.Speeds.Where(x => x.Id == id).FirstOrDefaultAsync());
        }

        [HttpPost]
        public async Task<IActionResult> CreateUpdate(Speed speed, int? id)
        {

            if (speed.Id == id)

            {
                bool speednull = await _context.Speeds.Where(x => x.Name.Equals(speed.Name) && x.Id != speed.Id).AnyAsync();
                if (!speednull)
                {
                    _context.Speeds.Update(speed);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Duplicate = "That Speed is taken. Try another.";
                }
            }
            else
            {
                var newspeed = await _context.Speeds.Where(x => x.Name.Equals(speed.Name)).ToListAsync();
                if (ModelState.IsValid)
                {
                    if (newspeed.Count > 0)
                    {
                        ViewBag.Duplicate = "That Speed is taken. Try another.";
                    }
                    else
                    {
                        _context.Speeds.Update(speed);
                        await _context.SaveChangesAsync();
                        return RedirectToAction("Index");
                    }
                }

            }
            return View();
        }
    }
}
