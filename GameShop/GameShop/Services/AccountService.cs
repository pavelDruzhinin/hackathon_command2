using GameShop.DataAccess;
using System.Linq;

namespace GameShop.Services
{
    public class AccountService
    {
        public bool Login(string login, string password)
        {
            using (var db = new GameShopContext())
            {
                var customer = db.Customers.FirstOrDefault(x => x.Login == login && x.Password == password);

                return customer != null;
            }
        }
    }
}