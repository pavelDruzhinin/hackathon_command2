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
        public int CategoryId { get; set; }
        public decimal Price { get; set; }
        public Category Category { get; set; }
    }
}