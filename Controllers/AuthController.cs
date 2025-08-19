using AuthService.ConfigurationResolve;
using AuthService.DbContextModels;
using AuthService.DTO_s;
using AuthService.Models;
using AuthService.Services;
using AuthService.Services.NotificationService;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System.Security.Cryptography;
using System.Text;
using XYZ.DataProvider.Entities;


namespace AuthService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    // [Route("api/v{version:apiVersion}/[controller]")]
    //[ApiVersion("1.0")]
    //[ApiVersion("2.0")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly TokenService _tokenService;
        private readonly ILogger<AuthController> _logger;
        private readonly IMyServTrans _myServTrans;
        //private readonly Func<string, INotify> _notify;
        private readonly IServiceProvider _serviceProvider;


        public AuthController(AppDbContext context, TokenService tokenService, ILogger<AuthController> logger, IMyServTrans myServTrans, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _context = context;
            _tokenService = tokenService;
            _myServTrans = myServTrans;
            //_notify = notify;
            _serviceProvider = serviceProvider;
        }



        [HttpGet("TestV1")]
        //[MapToApiVersion("2.0")]
        //[Authorize]
        public string TestV1()
        {
           //var mm= _serviceProvider.GetRequiredService<NotifyResolver>();
            //HttpContext.Response.WriteAsync("This is a custom response written directly.");
            _logger.LogInformation("TestV1 V.1 endpoint hit at {Time}", DateTime.UtcNow);
            _serviceProvider.ExecNotify("Mail").Send();
            _serviceProvider.ExecNotify("SMS").Send();

            //return _myServTrans.DoSomething_Vin();
            return "test V1";
        }
        [HttpGet("TestV2")]
        //[MapToApiVersion("3.0")]
        //[Route("TestV2")]
        public string TestV2()
        {
            _logger.LogInformation("TestV2 V.2 endpoint hit at {Time}", DateTime.UtcNow);
            return "test V2";
        }


        
        //[Authorize]
        [HttpGet("test")]
        public async Task<string> test1()
        {
            _logger.LogInformation("Test endpoint hit at {Time}", DateTime.UtcNow);
            return "helklo";
        }

        [Authorize]
        [HttpPut("updated")]
        //public async Task<IActionResult> UpdateUser(UserRegisterDto dto)
        //public async Task<IActionResult> UpdateUser(string Username, string Role, string Email, string password)
        //public async Task<IActionResult> UpdateUser([FromQuery]string Username, string Role, string Email, string password)
        //{
        //    var ID =   _context.Users.Where(up=>up.Username==Username).Select(x=>x.Id).FirstOrDefault();
        //    if (ID <=0)
        //    {
        //        return NotFound(new Users() { Username= ID.ToString() });
        //    }
        //    var userToUpdate =  await _context.Users.FindAsync(ID);
        //    if (userToUpdate != null   )
        //    {
        //        userToUpdate.Username = Username;
        //        userToUpdate.Role = Role;
        //        userToUpdate.Email = Email;
        //    }
        //    var userToUpdate_0 = await _context.Users.FindAsync(ID);

        //    await _context.SaveChangesAsync();
        //    return Ok(userToUpdate_0);
        //}



        public async Task<IActionResult> UpdateUser(UserRegisterDto dto)
        {
            _logger.LogInformation("UpdateUser endpoint hit at {Time}", DateTime.UtcNow);
            var ID = _context.Users.Where(up => up.Username == dto.Username).Select(x => x.Id).FirstOrDefault();
            if (ID <= 0)
            {
                return NotFound(new Users() { Username = ID.ToString() });
            }
            var userToUpdate = await _context.Users.FindAsync(ID);
            if (userToUpdate != null && dto != null)
            {
                userToUpdate.Username = dto.Username;
                userToUpdate.Role = dto.Role;
                userToUpdate.Email = dto.Email;
            }
            var userToUpdate_0 = await _context.Users.FindAsync(ID);

            await _context.SaveChangesAsync();
            return Ok(userToUpdate_0);
        }

        [HttpPost("register")]
    
        public async Task<IActionResult> Register(UserRegisterDto dto)
        {
            _logger.LogInformation("Register endpoint hit at {Time}", DateTime.UtcNow);
            if (await _context.Users.AnyAsync(u => u.Username == dto.Username))
                return BadRequest("User  already exists");

            using var hmac = new HMACSHA256();
            var user = new Users
            {
                Username = dto.Username,
                PasswordHash =hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.Password)),
                Role = dto.Role,
                Email=dto.Email,
                PasswordSalt=hmac.Key
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return Ok("User registered");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDto dto)
        {
             _logger.LogInformation("Login endpoint hit at {Time}", DateTime.UtcNow);
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == dto.Username);
            if (user == null)
                return Unauthorized("Invalid username");

            using var hmac = new HMACSHA256(user.PasswordSalt);
            var computedHash =  hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.Password));



            if (!computedHash.SequenceEqual(user.PasswordHash))
                return Unauthorized("Invalid password");
            

            var token = _tokenService.CreateToken(user);
            Response.Cookies.Append("access_token", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true, // Set to true in production (requires HTTPS)
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddSeconds(1000)
            });
            return Ok(new { token });
        }

        [HttpPost("google-login")]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginRequest request)
        {
            var payload = await GoogleJsonWebSignature.ValidateAsync(request.IdToken);

            var user =  await _context.Users.FirstOrDefaultAsync(x=>x.Email== payload.Email);

            if (user == null)
            {
                // Option 1: Auto-register
                user = new Users
                {
                    Email = payload.Email,
                    Username = payload.Name,
                    Role="G_Admin"
                };
                  _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }
            
            //// Generate JWT / Secure cookie
            ///
            var token = _tokenService.CreateToken(user);
            Response.Cookies.Append("access_token", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true, // Set to true in production (requires HTTPS)
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddSeconds(15)
            });
            return Ok(new { token });

             
        }

    }
}
