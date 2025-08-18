using AuthService.Handlers.Events;
using MediatR;
using static AuthService.Handlers.Events.UserRegistrationEvent;

namespace AuthService.Handlers.User
{
    public class SendMailHandler : INotificationHandler<UserRegisteredEvent>
    {
        public Task Handle(UserRegisteredEvent notification, CancellationToken cancellationToken)
        {
            Console.WriteLine($"📧 Email sent to: {notification.Email}");
            return Task.CompletedTask;
        }
    }
}
