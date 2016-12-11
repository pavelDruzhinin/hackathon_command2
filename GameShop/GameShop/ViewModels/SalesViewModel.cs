using GameShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameShop.ViewModels
{
    public class SalesViewModel
    {
        public string Name { get; set; }
        public int Count { get; set; }
        public int GameId { get; set; }
        public DateTime Time { get; set; }
    }
}