namespace WebEnvang.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NgayTT : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TicketInfo", "NguoiThanhToan", c => c.String());
            AddColumn("dbo.TicketInfo", "NgayThanhToan", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TicketInfo", "NgayThanhToan");
            DropColumn("dbo.TicketInfo", "NguoiThanhToan");
        }
    }
}
