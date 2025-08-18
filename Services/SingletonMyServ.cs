namespace AuthService.Services
{
    public class SingletonMyServ : IMyServSingle
    {
        //private readonly Lazy<IMyServScoped> _myServScoped;
        //public SingletonMyServ(Lazy<IMyServScoped> myServScoped)
        //{

        //    _myServScoped = myServScoped;
        //}
        public string DoSomething()
        {
            return "SingletonMyServ";
        }
    }
}
