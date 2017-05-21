namespace WebEnvang.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BookTotalPrice : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BookInfo", "TotalPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BookInfo", "TotalPrice");
        }
    }
}
