namespace GameShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class roman : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "Current", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "Current");
        }
    }
}
