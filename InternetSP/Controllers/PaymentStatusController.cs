using InternetSP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InternetSP.Controllers
{
    [Admin]
    public class PaymentStatusController : Controller
    {
        private readonly InternetSPContext _context;
        public PaymentStatusController(InternetSPContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.PaymentStatuses.ToListAsync());
        }

        public async Task<IActionResult> Delete(int? id)
        {
            var paymentStatus = await _context.PaymentStatuses.FindAsync(id);
            if (paymentStatus != null)
            {
                _context.PaymentStatuses.Remove(paymentStatus);
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
                return View(await _context.PaymentStatuses.Where(x => x.Id == id).FirstOrDefaultAsync());
        }

        [HttpPost]
        public async Task<IActionResult> CreateUpdate(PaymentStatus paymentStatus, int? id)
        {

            if (paymentStatus.Id == id)

            {
                bool paymentStatusnull = await _context.PaymentStatuses.Where(x => x.Name.Equals(paymentStatus.Name) && x.Id != paymentStatus.Id).AnyAsync();
                if (!paymentStatusnull)
                {
                    _context.PaymentStatuses.Update(paymentStatus);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Duplicate = "That Payment Status is taken. Try another.";
                }
            }
            else
            {
                var newpaymentStatus = await _context.PaymentStatuses.Where(x => x.Name.Equals(paymentStatus.Name)).ToListAsync();
                if (ModelState.IsValid)
                {
                    if (newpaymentStatus.Count > 0)
                    {
                        ViewBag.Duplicate = "That Payment Status is taken. Try another.";
                    }
                    else
                    {
                        _context.PaymentStatuses.Update(paymentStatus);
                        await _context.SaveChangesAsync();
                        return RedirectToAction("Index");
                    }
                }

            }
            return View();
        }
    }
}
