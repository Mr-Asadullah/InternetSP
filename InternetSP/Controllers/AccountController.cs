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
        [Admin]
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
            var users = await _context.Users.Where(x => x.Email.Equals(user.Email)).ToListAsync();
          
                if (users.Count > 0)
                {
                    ViewBag.Duplicate = "That Email is taken. Try another.";
                }
                else
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
            
            return View();
        }
        public IActionResult Logout()
        {
            Response.Cookies.Delete("user-access-token");
            return Redirect("/Home/Index");
        }

        [Admin]
        public async Task<IActionResult> Delete(int? id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
