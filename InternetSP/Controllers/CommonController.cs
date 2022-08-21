using InternetSP.Models;
using Microsoft.AspNetCore.Mvc;

namespace InternetSP.Controllers
{
    public class CommonController : Controller
    {
        private readonly InternetSPContext _context;

        public CommonController(InternetSPContext context)
        {
            _context = context;
        }
        public User GetUserId(HttpRequest httpRequest)
        {
            string accessToken = "";
            if (httpRequest.Cookies["user-access-token"] != null)
            accessToken = httpRequest.Cookies["user-access-token"];

            return _context.Users.Where(x => x.AccessToken.Equals(accessToken)).FirstOrDefault();

        }
    }
}
