using Microsoft.AspNetCore.Mvc;
using System;

namespace AuthService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PracticingController : Controller
    {
        public class Order
        {
            public int Id { get; set; }
            public decimal Amount { get; set; }
            public bool IsPaid { get; set; }
        }

        List<Order> orders = new List<Order>
        {
            new Order { Id = 1, Amount = 500, IsPaid = true },
            new Order { Id = 2, Amount = 2000, IsPaid = false },
            new Order { Id = 3, Amount = 1500, IsPaid = true },
            new Order { Id = 4, Amount = 300, IsPaid = true }
        };
        private bool customPredicateLogic(int m)
        {
            return true;
        }
        [HttpGet]
        public void Index()
        {
            Func<int,int, bool> func = customFuncLogic;
            Predicate<int> predicate = customPredicateLogic;
            testing(customFuncLogic);
        }

        //,Predicate<int> predicate,Action<int> action
        private void testing(Func<int,int,bool> func)
        {
            Predicate<int> predicateId = x => x == 3;
            Predicate<int> predicateAmount = amt => amt > 500;
            Predicate<bool> predicatePaid=paid=>paid == true;
            
            
            //orders.Find(o=>);

        }
        private bool customFuncLogic(int num1,int num2)
        {
            return num1 * num2 == 6;
        }
    }
}
