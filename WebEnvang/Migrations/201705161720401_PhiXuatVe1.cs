namespace WebEnvang.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PhiXuatVe1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Airline", "PhiHangTreEm", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Airline", "PhiHangEmBe", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Airline", "PhiHangEmBe");
            DropColumn("dbo.Airline", "PhiHangTreEm");
        }
    }
}
