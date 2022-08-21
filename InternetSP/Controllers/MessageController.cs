using InternetSP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InternetSP.Controllers
{
    public class MessageController : Controller
    {
        private readonly InternetSPContext _context;
        public MessageController(InternetSPContext context)
        {
            _context = context;
        }
        public IActionResult Index(int id)
        {
            ViewBag.OrderId = id;
            return View(_context.Messages.Where(x => x.ReceiverId == id).Include(x => x.Sender).OrderByDescending(x => x.Id).ToList());
            
        }
    }
}
