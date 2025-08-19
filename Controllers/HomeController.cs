using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers
{
    public class HomeController : Controller
    {
        public string Index()
        {
            return "Home controller";
        }
    }
}
