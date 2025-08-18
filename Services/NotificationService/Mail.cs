namespace AuthService.Services.NotificationService
{
    public class Mail:INotify
    {
        public void Send()
        {
            Console.WriteLine("Notified via Mail succesfully");
        }
    }
}
