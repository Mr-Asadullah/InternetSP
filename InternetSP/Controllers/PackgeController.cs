using InternetSP.Models;
using InternetSP.Controllers;
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
            return View(await _context.Packges.Include(x => x.Volume).Include(x => x.Speed).Include(x => x.User).ToListAsync());
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
        public async Task<IActionResult> CreateUpdate(Packge packge, IFormFile file)
        {
            string? accessToken = HttpContext.Request.Cookies["user-access-token"];
            User? user = _context.Users.Where(x => x.AccessToken.Equals(accessToken)).FirstOrDefault();
            if (user != null)
            {
                packge.UserId = user.Id;
                string name = $"{DateTime.UtcNow.Ticks}-";
                if (file == null || file.Length == 0)
                    return Content("file not selected");

                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images", name + file.FileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                    string content_type = file.ContentType;
                }
                packge.Img = name + file.FileName;
                _context.Packges.Update(packge);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            else
            {
                return Redirect("/Account/Login");
            }
            
        }
    }
}
