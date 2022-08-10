using InternetSP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InternetSP.Controllers
{
    [Admin]
    public class VolumeController : Controller
    {
        private readonly InternetSPContext _context;
        public VolumeController(InternetSPContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.Volumes.ToListAsync());
        }

        public async Task<IActionResult> Delete(int? id)
        {
            var volume = await _context.Volumes.FindAsync(id);
            if (volume != null)
            {
                _context.Volumes.Remove(volume);
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
                return View(await _context.Volumes.Where(x => x.Id == id).FirstOrDefaultAsync());
        }

        [HttpPost]
        public async Task<IActionResult> CreateUpdate(Volume volume, int? id)
        {

            if (volume.Id == id)

            {
                bool volumenull = await _context.Volumes.Where(x => x.Name.Equals(volume.Name) && x.Id != volume.Id).AnyAsync();
                if (!volumenull)
                {
                    _context.Volumes.Update(volume);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Duplicate = "That Volume is taken. Try another.";
                }
            }
            else
            {
                var newvolume = await _context.Volumes.Where(x => x.Name.Equals(volume.Name)).ToListAsync();
                if (ModelState.IsValid)
                {
                    if (newvolume.Count > 0)
                    {
                        ViewBag.Duplicate = "That Volume is taken. Try another.";
                    }
                    else
                    {
                        _context.Volumes.Update(volume);
                        await _context.SaveChangesAsync();
                        return RedirectToAction("Index");
                    }
                }

            }
            return View();
        }
    }
}
