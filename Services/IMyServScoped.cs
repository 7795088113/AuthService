
using XYZ.DataProvider.Entities;

namespace AuthService.Services
{
    public interface IMyServScoped
    {
        public List<Order> DoSomething();
        public string vinu()
        {
            return "vinay";
        }
    }
}