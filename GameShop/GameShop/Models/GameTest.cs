using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GameShop.Models
{
    public class GameTest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public decimal Price { get; set; }
        [Range(0, 10)]
        public int Rating { get; set; }
        public string Description { get; set; }
        public string SystemRequirement { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime PubDate { get; set; }
        public string GamePosterUrl { get; set; }
        public List<OrderPosition> OrderPositions { get; set; }
        public List<GameComment> GameComments { get; set; }
    }
}