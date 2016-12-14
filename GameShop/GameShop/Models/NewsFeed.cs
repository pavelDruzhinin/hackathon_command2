using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameShop.Models
{
    public class NewsFeed
    {
        public int Id { get; set; }
        public string Name { get; set; }            // наименование ленты новостей
        public List<News> ManyNews { get; set; }    // связь с новостями в данной ленте
    }
}