using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration;
using GameShop.Models;

namespace GameShop.DataAccess.Mapping
{
    public class NewsMap : EntityTypeConfiguration<News>
    {
        public NewsMap()
        {
            ToTable("News");
            HasKey(x => x.Id);
            HasRequired(x => x.NewsFeed);
        }
    }
}