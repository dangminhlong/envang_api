namespace WebEnvang.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PhiHang : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Airline", "PhiHang", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Airline", "PhiHang");
        }
    }
}
