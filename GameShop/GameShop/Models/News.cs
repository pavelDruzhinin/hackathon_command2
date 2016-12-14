using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GameShop.Models
{
    public class News
    {
        public int Id { get; set; }

        [Required]
        public string NewsHeader { get; set; }      // заголовок новости

        [Required]
        [DataType(DataType.MultilineText)]
        [StringLength(300)]
        public string NewsBody { get; set; }        // текст новости, 300 символов, многострочный

        public DateTime CreateDate { get; set; }    // дата создания новости
        public DateTime ModifyDate { get; set; }    // дата модификации новости
        public int NewsFeedId { get; set; }         // связь с определенной лентой новостей
        public NewsFeed NewsFeed { get; set; }
    }
}