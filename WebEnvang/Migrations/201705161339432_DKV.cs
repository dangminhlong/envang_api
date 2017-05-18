namespace WebEnvang.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DKV : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Airline", "DieuKienHanhLy", c => c.String());
            AddColumn("dbo.Airline", "DieuKienVe", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Airline", "DieuKienVe");
            DropColumn("dbo.Airline", "DieuKienHanhLy");
        }
    }
}
