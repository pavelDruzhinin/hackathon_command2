using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration;
using GameShop.Models;

namespace GameShop.DataAccess.Mapping
{
    public class OrderMap: EntityTypeConfiguration<Order>
    {
        public OrderMap()
        {
            ToTable("Orders");
            HasKey(x => x.Id);
            HasRequired(x => x.Customer);
            HasMany(x => x.OrderPositions);
        }
    }
}