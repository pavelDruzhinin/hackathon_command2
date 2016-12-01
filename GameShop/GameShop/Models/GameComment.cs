using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GameShop.Models
{
    public class GameComment
    {
        public int Id { get; set; }
        [Required]
        [Range(0, 10)]
        public int Rating { get; set; }
        [Required]
        [DataType(DataType.MultilineText)]
        public string Comment { get; set; }
        public DateTime Time { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public int GameId { get; set; }
        public Game Game { get; set; }
    }
}