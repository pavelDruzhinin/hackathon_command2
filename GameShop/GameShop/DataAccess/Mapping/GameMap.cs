using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration;
using GameShop.Models;

namespace GameShop.DataAccess.Mapping
{
    public class GameMap: EntityTypeConfiguration<Game>
    {
        public GameMap()
        {
            ToTable("Games");
            HasKey(x => x.Id);
            HasRequired(x => x.Category);
            HasMany(x => x.OrderPositions);
            HasMany(x => x.GameComments);
        }            
    }
}