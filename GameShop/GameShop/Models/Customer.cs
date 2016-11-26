using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameShop.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthday { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    }
}