using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using GameShop.DataAccess.Mapping;

namespace GameShop.DataAccess
{
    public class GameShopContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public GameShopContext() : base("name=GameShopContext")
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<GameShopContext>());
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new CategoryMap());
            modelBuilder.Configurations.Add(new CustomerMap());
            modelBuilder.Configurations.Add(new GameMap());
            modelBuilder.Configurations.Add(new OrderMap());
            modelBuilder.Configurations.Add(new OrderPositionMap());
        }

        public System.Data.Entity.DbSet<GameShop.Models.Game> Games { get; set; }

        public System.Data.Entity.DbSet<GameShop.Models.Category> Categories { get; set; }

        public System.Data.Entity.DbSet<GameShop.Models.Order> Orders { get; set; }

        public System.Data.Entity.DbSet<GameShop.Models.Customer> Customers { get; set; }
    }
}
