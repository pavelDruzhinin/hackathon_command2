namespace GameShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modifyGameModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Games", "GamePosterUrl", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Games", "GamePosterUrl");
        }
    }
}
