using InternetSP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InternetSP.Controllers
{
    public class PaymentController : Controller
    {
        private readonly InternetSPContext _context;
        public PaymentController(InternetSPContext context)
        {
            _context = context;
        }
        [Admin]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Payments.ToListAsync());
        }
        [Admin]
        public async Task<IActionResult> Delete(int? id)
        {
            Payment payment = await _context.Payments.FindAsync(id);
            _context.Payments.Remove(payment);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        [Buyer]
        public async Task<IActionResult> Payment(int? id)
        {
            Packge? packge = await _context.Packges.FindAsync(id);
            var user = new CommonController(_context).GetUserId(Request);
            if (user != null)
            {
                Payment payment = new Payment
                {
                    PaymentTime = DateTime.UtcNow.AddHours(5),
                    PackageId = packge.Id,
                    UserId = user.Id,
                    Price = packge.Price,
                    PaymentStatusId = 1,
                    Image="asdasda"
                };

                _context.Payments.Add(payment);
                await _context.SaveChangesAsync();
                return Redirect("/Buyer/Index");
            }
            return Redirect("/Account/Login");
        }
    }
}
