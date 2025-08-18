using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers
{
    //[Route("api/[controller]")]
    public class vinuController
    {
        [HttpGet]
        public string getSimpleText()
        {
            return "simpletext";
        }
    }
}
