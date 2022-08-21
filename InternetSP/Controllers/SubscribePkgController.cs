using InternetSP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InternetSP.Controllers
{
    public class SubscribePkgController : Controller
    {
        private readonly InternetSPContext _context;

        public SubscribePkgController(InternetSPContext context)
        {
            _context = context;
        }
        [Admin]
        public async Task<IActionResult> Index()
        {
            return View(await _context.SubscribePkgs.Include(x => x.Packge).Include(x => x.User).ToListAsync());
        }
        [Buyer]
        public async Task<IActionResult> Orders(int? id)
        {
            Packge? packge =await _context.Packges.FindAsync(id);
            var user = new CommonController(_context).GetUserId(Request);
            if (user != null)
            {
                SubscribePkg pkg = new SubscribePkg
                {
                    Dateime = DateTime.UtcNow.AddHours(5),
                    PackgeId = packge.Id,
                    UserId = user.Id,
                    Price= packge.Price
                };

                _context.SubscribePkgs.Add(pkg);
                await _context.SaveChangesAsync();
                return Redirect("/Buyer/Index");
            }
             return Redirect("/Account/Login");
        }
        [Admin]
        public async Task<IActionResult> Delete(int? id)
        {
            var pkg = await _context.SubscribePkgs.FindAsync(id);
            if (pkg != null)
            {
                _context.SubscribePkgs.Remove(pkg);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
