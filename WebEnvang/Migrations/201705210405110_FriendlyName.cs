namespace WebEnvang.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FriendlyName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Article", "FriendlyName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Article", "FriendlyName");
        }
    }
}
