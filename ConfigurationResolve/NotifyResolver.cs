using AuthService.CustomExceptions;
using AuthService.Services.NotificationService;

namespace AuthService.ConfigurationResolve
{
    public static class NotifyResolver
    {
        public static INotify ExecNotify(this IServiceProvider serviceProvider,string Type)
        {
            switch (Type)
            {
                case "Mail":
                    return serviceProvider.GetRequiredService<Mail>();
                case "SMS":
                    return serviceProvider.GetRequiredService<Message>();
                default:
                    throw new OwnException();
            }
        }
    }
}
