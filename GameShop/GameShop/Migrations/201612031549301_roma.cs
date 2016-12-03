namespace GameShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class roma : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GameComments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Rating = c.Int(nullable: false),
                        Comment = c.String(nullable: false, maxLength: 300),
                        Time = c.DateTime(nullable: false),
                        CustomerId = c.Int(nullable: false),
                        GameId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.CustomerId, cascadeDelete: true)
                .ForeignKey("dbo.Games", t => t.GameId, cascadeDelete: true)
                .Index(t => t.CustomerId)
                .Index(t => t.GameId);
            
            AddColumn("dbo.Games", "Rating", c => c.Int(nullable: false));
            AddColumn("dbo.Orders", "Current", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GameComments", "GameId", "dbo.Games");
            DropForeignKey("dbo.GameComments", "CustomerId", "dbo.Customers");
            DropIndex("dbo.GameComments", new[] { "GameId" });
            DropIndex("dbo.GameComments", new[] { "CustomerId" });
            DropColumn("dbo.Orders", "Current");
            DropColumn("dbo.Games", "Rating");
            DropTable("dbo.GameComments");
        }
    }
}
