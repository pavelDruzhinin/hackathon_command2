using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameShop.Models
{
    public class Game
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Key { get; set; }       // Ключ от игры, выдается при покупке
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public List<OrderPosition> OrderPositions { get; set; }
    }
}