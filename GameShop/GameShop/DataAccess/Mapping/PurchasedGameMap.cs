﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration;
using GameShop.Models;

namespace GameShop.DataAccess.Mapping
{
    public class PurchasedGameMap : EntityTypeConfiguration<PurchasedGame>
    {
        public PurchasedGameMap()
        {
            ToTable("PurchasedGames");
            HasKey(x => x.Id);
            HasRequired(x => x.Customer);
        }
    }
}