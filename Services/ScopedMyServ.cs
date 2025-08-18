using Microsoft.AspNetCore.Mvc;
using XYZ.DataProvider.DbContexts;
using XYZ.DataProvider.Entities;
using XYZ.DataProvider.Repositories;

namespace AuthService.Services
{
    public class ScopedMyServ : IMyServScoped
    {
        private readonly IMyServSingle _myServSingle;
        private readonly  IMyServTrans  _myServTrans;
        private IProductsRepo _productsRepo;

        public ScopedMyServ(IMyServSingle myServSingle, IProductsRepo productsRepo,  IMyServTrans  myServTrans)
        {
            _myServSingle = myServSingle; 
            _productsRepo = productsRepo;
            _myServTrans = myServTrans;
        }
        public List<Order> DoSomething()
        {
            var mm = _productsRepo.GetALlProducts().Take(10).ToList();
            return mm;

            //return "ScopedMyServ";
        }
    }
}
