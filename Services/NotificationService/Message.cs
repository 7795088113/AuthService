namespace AuthService.Services.NotificationService
{
    public class Message : INotify
    {
        public void Send()
        {
            Console.WriteLine("Notified via Message succesfully");
        }
    }
}
