using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using GameShop.DataAccess;

namespace GameShop.DataAccess
{
    public class GameShopInitializer
    {
        public class SchoolDBInitializer : CreateDatabaseIfNotExists<GameShopContext>
        {
            protected override void Seed(GameShopContext context)
            {
                base.Seed(context);
            }
        }
    }
}