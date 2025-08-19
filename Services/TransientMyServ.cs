using Azure;

namespace AuthService.Services
{
    public class TransientMyServ : IMyServTrans
    {

        private readonly Func<IMyServScoped> _myServScoped;

        public TransientMyServ(Func<IMyServScoped> myServScoped)
        {
            _myServScoped = myServScoped;
        }
        public string DoSomething_Vin()
        {
            //return new();
            
            return "TransientMyServ";
        }
    }
}
