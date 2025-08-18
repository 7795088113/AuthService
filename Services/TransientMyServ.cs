using Azure;
using XYZ.DataProvider.Entities;

namespace AuthService.Services
{
    public class TransientMyServ : IMyServTrans
    {

        private readonly Func<IMyServScoped> _myServScoped;

        public TransientMyServ(Func<IMyServScoped> myServScoped)
        {
            _myServScoped = myServScoped;
        }
        public List<Order> DoSomething_Vin()
        {
            //return new();
            return _myServScoped().DoSomething();
            //return "TransientMyServ";
        }
    }
}
