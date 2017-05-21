namespace WebEnvang.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ThongTinKhac : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BookInfo", "XuatHoaDon", c => c.Boolean(nullable: false));
            AddColumn("dbo.BookInfo", "ThongTinXuatHoaDon", c => c.String());
            AddColumn("dbo.BookInfo", "SoDienThoaiNguoiGioiThieu", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.BookInfo", "SoDienThoaiNguoiGioiThieu");
            DropColumn("dbo.BookInfo", "ThongTinXuatHoaDon");
            DropColumn("dbo.BookInfo", "XuatHoaDon");
        }
    }
}
