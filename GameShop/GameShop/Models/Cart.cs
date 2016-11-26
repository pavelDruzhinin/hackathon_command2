using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameShop.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public int GameId { get; set; }
        public Game Game { get; set; }
    }
}