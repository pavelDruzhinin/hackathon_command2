namespace GameShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addDiscr : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Games", "SystemRequirement", c => c.String());
            AddColumn("dbo.Games", "PubDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Games", "PubDate");
            DropColumn("dbo.Games", "SystemRequirement");
        }
    }
}
