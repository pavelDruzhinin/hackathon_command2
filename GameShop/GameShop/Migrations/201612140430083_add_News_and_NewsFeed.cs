namespace GameShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_News_and_NewsFeed : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.News",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NewsHeader = c.String(nullable: false),
                        NewsBody = c.String(nullable: false, maxLength: 300),
                        CreateDate = c.DateTime(nullable: false),
                        ModifyDate = c.DateTime(nullable: false),
                        NewsFeedId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NewsFeed", t => t.NewsFeedId, cascadeDelete: true)
                .Index(t => t.NewsFeedId);
            
            CreateTable(
                "dbo.NewsFeed",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.News", "NewsFeedId", "dbo.NewsFeed");
            DropIndex("dbo.News", new[] { "NewsFeedId" });
            DropTable("dbo.NewsFeed");
            DropTable("dbo.News");
        }
    }
}
