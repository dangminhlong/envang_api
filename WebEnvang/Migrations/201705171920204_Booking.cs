namespace WebEnvang.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Booking : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BookInfo",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RoundTrip = c.Boolean(nullable: false),
                        FromPlaceCode = c.String(),
                        ToPlaceCode = c.String(),
                        FromPlaceId = c.Int(nullable: false),
                        ToPlaceId = c.Int(nullable: false),
                        DepartDate = c.String(),
                        ReturnDate = c.String(),
                        LienHeHoTen = c.String(),
                        LienHeDienThoai = c.String(),
                        LienHeEmail = c.String(),
                        LienHeDiaChi = c.String(),
                        NgayDat = c.DateTime(nullable: false),
                        NguoiDat = c.String(),
                        IP = c.String(),
                        TinhTrang = c.Int(nullable: false),
                        NguoiXuLy = c.String(),
                        NgayXuLy = c.DateTime(),
                        NguoiHuy = c.String(),
                        NgayHuy = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TicketInfo",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BookInfoId = c.Int(nullable: false),
                        IsRoundTrip = c.Boolean(nullable: false),
                        Airline = c.String(),
                        FlightNo = c.String(),
                        TicketPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TicketFare = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TicketType = c.String(),
                        FromCityCode = c.String(),
                        ToCityCode = c.String(),
                        DepartureDate = c.String(),
                        DepartureTime = c.String(),
                        ArrivalDate = c.String(),
                        ArrivalTime = c.String(),
                        ReturnFlightNo = c.String(),
                        ReturnTicketPrice = c.Decimal(precision: 18, scale: 2),
                        ReturnTicketFare = c.Decimal(precision: 18, scale: 2),
                        ReturnTicketType = c.String(),
                        ReturnDepartureDate = c.String(),
                        ReturnDepartureTime = c.String(),
                        ReturnArrivalDate = c.String(),
                        ReturnArrivalTime = c.String(),
                        GhiChu = c.String(),
                        PNRCode = c.String(),
                        TinhTrang = c.Int(nullable: false),
                        NguoiDat = c.String(),
                        NgayDat = c.DateTime(nullable: false),
                        NguoiXuat = c.String(),
                        NgayXuat = c.DateTime(),
                        NguoiHuy = c.String(),
                        NgayHuy = c.DateTime(),
                        NguoiHoan = c.String(),
                        NgayHoan = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BookInfo", t => t.BookInfoId, cascadeDelete: true)
                .Index(t => t.BookInfoId);
            
            CreateTable(
                "dbo.TicketPassenger",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TicketInfoId = c.Int(nullable: false),
                        Type = c.Int(nullable: false),
                        Title = c.String(),
                        FullName = c.String(),
                        Baggage = c.Decimal(precision: 18, scale: 2),
                        ReturnBaggage = c.Decimal(precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TicketInfo", t => t.TicketInfoId, cascadeDelete: true)
                .Index(t => t.TicketInfoId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TicketPassenger", "TicketInfoId", "dbo.TicketInfo");
            DropForeignKey("dbo.TicketInfo", "BookInfoId", "dbo.BookInfo");
            DropIndex("dbo.TicketPassenger", new[] { "TicketInfoId" });
            DropIndex("dbo.TicketInfo", new[] { "BookInfoId" });
            DropTable("dbo.TicketPassenger");
            DropTable("dbo.TicketInfo");
            DropTable("dbo.BookInfo");
        }
    }
}
