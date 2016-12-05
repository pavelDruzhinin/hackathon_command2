namespace GameShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addRole : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Customers", "RoleId", c => c.Int(nullable: false));
            CreateIndex("dbo.Customers", "RoleId");
            AddForeignKey("dbo.Customers", "RoleId", "dbo.Roles", "Id", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Customers", "RoleId", "dbo.Roles");
            DropIndex("dbo.Customers", new[] { "RoleId" });
            DropColumn("dbo.Customers", "RoleId");
            DropTable("dbo.Roles");
        }
    }
}
