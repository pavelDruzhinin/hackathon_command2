using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration;
using GameShop.Models;

namespace GameShop.DataAccess.Mapping
{
    public class CategoryMap: EntityTypeConfiguration<Category>
    {
        public CategoryMap()
        {
            ToTable("Categories");
            HasKey(x => x.Id);
            HasMany(x => x.Games);
        }
    }
}