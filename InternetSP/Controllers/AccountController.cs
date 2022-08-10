using InternetSP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InternetSP.Controllers
{
    public class AccountController : Controller
    {
        private readonly InternetSPContext _context;
        
        public AccountController(InternetSPContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.Users.Include(x => x.Role).ToListAsync());
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(User user)
        {
            User? dbuser = _context.Users.Where(x => x.Email.ToLower().Equals(user.Email) && x.Password.Equals(user.Password)).FirstOrDefault();
            if (dbuser == null)
            {
                ViewBag.Error = "Your Email & Password is incorrect";
                return View();
            }
            CookieOptions options = new CookieOptions();
            options.Expires = DateTime.UtcNow.AddDays(5);
            Response.Cookies.Append("user-access-token", dbuser.AccessToken, options);
            return Redirect("/Home/Index");
        }

        [HttpPost]
        public async Task<IActionResult> Register(User user)
        {
            user.RoleId = 2;
            user.Joining = DateTime.UtcNow.AddHours(5);
            user.AccessToken = Convert.ToBase64String(Guid.NewGuid().ToByteArray()) + DateTime.UtcNow.Ticks;
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            CookieOptions options = new CookieOptions();
            options.Expires = DateTime.UtcNow.AddDays(5);
            Response.Cookies.Append("user-access-token", user.AccessToken, options);
            return Redirect("/Home/Index");
        }
        public IActionResult Logout()
        {
            Response.Cookies.Delete("user-access-token");
            return Redirect("/Home/Index");
        }
    }
}
