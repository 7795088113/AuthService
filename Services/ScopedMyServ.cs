using Microsoft.AspNetCore.Mvc;
 

namespace AuthService.Services
{
    public class ScopedMyServ : IMyServScoped
    {
        private readonly IMyServSingle _myServSingle;
        private readonly  IMyServTrans  _myServTrans;
        //private IProductsRepo _productsRepo;

        public ScopedMyServ(IMyServSingle myServSingle,    IMyServTrans  myServTrans)
        {
            _myServSingle = myServSingle; 
           
            _myServTrans = myServTrans;
        }
        public string DoSomething()
        {
             

            return "ScopedMyServ";
        }
    }
}
