using AuthService.Models;
using MediatR;

namespace AuthService.Handlers.Commands
{
    public class RegisterUserCommand
    {
        public record CreateRegisterUserCommand(Users Users) : IRequest<int>;
    }
}
