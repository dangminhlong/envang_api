namespace WebEnvang.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PhiXuatVe : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WebConfig", "AdultFee", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.WebConfig", "ChildFee", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.WebConfig", "InfantFee", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.WebConfig", "InfantFee");
            DropColumn("dbo.WebConfig", "ChildFee");
            DropColumn("dbo.WebConfig", "AdultFee");
        }
    }
}
