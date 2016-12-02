namespace GameShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migr6 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.GameComments", "Comment", c => c.String(nullable: false, maxLength: 300));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.GameComments", "Comment", c => c.String(nullable: false));
        }
    }
}
