using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration;
using GameShop.Models;

namespace GameShop.DataAccess.Mapping
{
    public class GameCommentMap : EntityTypeConfiguration<GameComment>
    {
        public GameCommentMap()
        {
            ToTable("GameComments");
            HasKey(x => x.Id);
            HasRequired(x => x.Customer);
            HasRequired(x => x.Game);
        }
    }
}