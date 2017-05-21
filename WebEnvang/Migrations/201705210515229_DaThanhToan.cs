namespace WebEnvang.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DaThanhToan : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TicketInfo", "DaThanhToan", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TicketInfo", "DaThanhToan");
        }
    }
}
