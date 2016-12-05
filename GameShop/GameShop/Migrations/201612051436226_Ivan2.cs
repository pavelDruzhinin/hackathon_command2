namespace GameShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Ivan2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PurchasedGames",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GameId = c.Int(nullable: false),
                        Key = c.String(),
                        Name = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Time = c.DateTime(nullable: false),
                        CustomerId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.CustomerId, cascadeDelete: true)
                .Index(t => t.CustomerId);
            
            DropColumn("dbo.Games", "Key");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Games", "Key", c => c.String());
            DropForeignKey("dbo.PurchasedGames", "CustomerId", "dbo.Customers");
            DropIndex("dbo.PurchasedGames", new[] { "CustomerId" });
            DropTable("dbo.PurchasedGames");
        }
    }
}
