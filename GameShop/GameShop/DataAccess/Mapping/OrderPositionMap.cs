using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration;
using GameShop.Models;

namespace GameShop.DataAccess.Mapping
{
    public class OrderPositionMap: EntityTypeConfiguration<OrderPosition>
    {
        public OrderPositionMap()
        {
            ToTable("OrderPositions");
            HasKey(x => x.Id);
            HasRequired(x => x.Order);
            HasRequired(x => x.Game);
        }
    }
}