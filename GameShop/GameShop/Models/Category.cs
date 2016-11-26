using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameShop.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Game> Games { get; set; }
    }
}