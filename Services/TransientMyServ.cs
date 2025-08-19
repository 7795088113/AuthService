using Azure;

namespace AuthService.Services
{
    public class TransientMyServ : IMyServTrans
    {
         

        
        public string DoSomething_Vin()
        {
            //return new();
            
            return "TransientMyServ";
        }
    }
}
