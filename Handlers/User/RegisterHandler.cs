using AuthService.DbContextModels;
using AuthService.Handlers.Commands;
using AuthService.Models;
using MediatR;
using static AuthService.Handlers.Commands.RegisterUserCommand;
using static AuthService.Handlers.Events.UserRegistrationEvent;

namespace AuthService.Handlers.User
{
    public class RegisterHandler : IRequestHandler<CreateRegisterUserCommand, int>
    {
        private readonly AppDbContext _userDbContext;
        private readonly IMediator _mediator;
        public RegisterHandler(AppDbContext userDbContext,IMediator mediator)
        {
            _userDbContext = userDbContext;
            _mediator = mediator;
        }

       public async Task<int>  Handle(CreateRegisterUserCommand request, CancellationToken cancellationToken)
        {
            var user = new Users
            {
                Username = request.Users.Username,
                Email = request.Users.Email
            };

            _userDbContext.Users.Add(user);
            await _userDbContext.SaveChangesAsync(cancellationToken);

            await _mediator.Publish(new UserRegisteredEvent(user.Id, user.Email), cancellationToken);
            
            return user.Id;
        }


    }
    
}
