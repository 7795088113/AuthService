using AuthService.Handlers.Commands;
using AuthService.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static AuthService.Handlers.Commands.RegisterUserCommand;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace AuthService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Registration : ControllerBase
    {
        private readonly IMediator _mediator;
        public Registration(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("myregister")]
        public async Task<IActionResult> Register(Users command)
        {
            var userId = await _mediator.Send(new CreateRegisterUserCommand(command));
            return Ok(new { UserId = userId });
        }
    }
}
