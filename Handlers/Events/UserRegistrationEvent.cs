using MediatR;

namespace AuthService.Handlers.Events
{
    public class UserRegistrationEvent
    {
        public record UserRegisteredEvent(int UserId, string Email) : INotification;
    }
}
