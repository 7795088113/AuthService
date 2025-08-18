using AuthService.DbContextModels;
using AuthService.Models;
using AuthService.Models.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace AuthService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly  CustomJsonSettings _mySettings;
        private readonly AppDbContext _userDbContext;
        private readonly ILogger<UserController> _logger;
        public UserController(AppDbContext user ,ILogger<UserController> logger, IOptions<CustomJsonSettings> mySettings)
        {
            _userDbContext = user;
            _logger = logger;
            _mySettings = mySettings.Value;
        }


        [HttpGet("Name")]
        public GenericResponse<Users> Registration()
        {
            
            _logger.LogInformation("Inside controller action - TraceId should appear");
            return new GenericResponse<Users>()
            {
                Message = "",
                ResponseMessage = new HttpCustomResponse<Users>()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Message = _userDbContext.Users.ToList(),
                    //TraceId = HttpContext.TraceIdentifier
                }

            };

        }
    }
}
