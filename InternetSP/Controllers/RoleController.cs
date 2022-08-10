using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InternetSP.Models;


namespace InternetSP.Controllers
{
    [Admin]
    public class RoleController : Controller
    {
        private readonly InternetSPContext _context;
        public RoleController(InternetSPContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.Roles.ToListAsync());
        }

        public async Task<IActionResult> Delete(int? id)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role != null)
            {
                _context.Roles.Remove(role);
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
                return View(await _context.Roles.Where(x => x.Id == id).FirstOrDefaultAsync());
        }

        [HttpPost]
        public async Task<IActionResult> CreateUpdate(Role role, int? id)
        {

            if (role.Id == id)

            {
                bool rolesnull = await _context.Roles.Where(x => x.Name.Equals(role.Name) && x.Id != role.Id).AnyAsync();
                if (!rolesnull)
                {
                    _context.Roles.Update(role);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Duplicate = "That Role is taken. Try another.";
                }
            }
            else
            {
                var newrole = await _context.Roles.Where(x => x.Name.Equals(role.Name)).ToListAsync();
                if (ModelState.IsValid)
                {
                    if (newrole.Count > 0)
                    {
                        ViewBag.Duplicate = "That Role is taken. Try another.";
                    }
                    else
                    {
                        _context.Roles.Update(role);
                        await _context.SaveChangesAsync();
                        return RedirectToAction("Index");
                    }
                }

            }
            return View();
        }
    }
}
