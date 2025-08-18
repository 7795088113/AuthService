using AuthService.Services.NotificationService;

namespace AuthService.CustomExceptions
{
    public class OwnException:Exception
    {
        public OwnException()
        {
            
        }
        public OwnException(string message):base(message) 
        {
            
        }

        public OwnException(string message,Exception exception) : base(message,exception)
        {

        }

     
    }
}
