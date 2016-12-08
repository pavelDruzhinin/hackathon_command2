using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameShop.Models
{
    public class PurchasedGame
    {
        public int Id { get; set; }
        public int GameId { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
        public Decimal Price { get; set; }
        public DateTime Time { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
    }
}