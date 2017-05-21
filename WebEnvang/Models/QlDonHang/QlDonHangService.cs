using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace WebEnvang.Models.QlDonHang
{
    public class QlDonHangService
    {
        private readonly ApplicationDbContext ctx = null;
        public QlDonHangService()
        {
            ctx = new ApplicationDbContext();
        }
        public async Task<dynamic> LayDsDonHang(int tinhtrang, DateTime? tungay, DateTime? denngay, string nguoidat, string soDtNguoiGT)
        {
            var query = (from x in ctx.BookInfos
                         join pm in ctx.PaymentMethods on x.PaymentMethodId equals pm.Id
                         where (x.TinhTrang == tinhtrang || (tinhtrang == -1 && x.TinhTrang != 3))
                            && (x.NgayDat >= tungay && x.NgayDat < denngay)
                            && (string.IsNullOrEmpty(nguoidat) || x.LienHeHoTen.Contains(nguoidat))
                            && (string.IsNullOrEmpty(soDtNguoiGT)||(x.SoDienThoaiNguoiGioiThieu.Contains(soDtNguoiGT)))
                         select new
                         {
                             Id = x.Id,
                             RoundTrip = x.RoundTrip,
                             NgayDat = x.NgayDat,
                             NguoiDat = x.LienHeHoTen,
                             DienThoai = x.LienHeDienThoai,
                             Email = x.LienHeEmail,
                             DiaChi = x.LienHeDiaChi,
                             FromPlaceCode = x.FromPlaceCode,
                             ToPlaceCode = x.ToPlaceCode,
                             DepartureDate = x.DepartDate,
                             ReturnDate = x.ReturnDate,
                             HinhThucThanhToan = pm.Name,
                             ThongTinThanhToan = x.PaymentMethodMessage,
                             TinhTrang = x.TinhTrang,
                             TotalPrice = x.TotalPrice,
                             XuatHoaDon = x.XuatHoaDon,
                             ThongTinXuatHoaDon = x.ThongTinXuatHoaDon,
                             SoDienThoaiNguoiGioiThieu = x.SoDienThoaiNguoiGioiThieu
                         });
            return await query.ToListTask();
        }

        public async Task<dynamic> LayChiTietDonHang(int bookId)
        {
            var queryTicket = (from x in ctx.TicketInfos
                               where x.BookInfoId == bookId
                               select new
                               {
                                   Ticket = new
                                   {
                                       Id = x.Id,
                                       BookInfoId = x.BookInfoId,
                                       IsRoundTrip = x.IsRoundTrip,
                                       Airline = x.Airline,
                                       FlightNo = x.FlightNo,
                                       TicketPrice = x.TicketPrice,
                                       TicketFare = x.TicketFare,
                                       TicketType = x.TicketType,
                                       FromCityCode = x.FromCityCode,
                                       ToCityCode = x.ToCityCode,
                                       DepartureDate = x.DepartureDate,
                                       DepartureTime = x.DepartureTime,
                                       ArrivalDate = x.ArrivalDate,
                                       ArrivalTime = x.ArrivalTime,
                                       ReturnFlightNo = x.ReturnFlightNo,
                                       ReturnTicketPrice = x.ReturnTicketPrice,
                                       ReturnTicketFare = x.ReturnTicketFare,
                                       ReturnTicketType = x.ReturnTicketType,
                                       ReturnDepartureDate = x.ReturnDepartureDate,
                                       ReturnDepartureTime = x.ReturnDepartureTime,
                                       ReturnArrivalDate = x.ReturnArrivalDate,
                                       ReturnArrivalTime = x.ReturnArrivalTime,
                                       GhiChu = x.GhiChu,
                                       PNRCode = x.PNRCode,
                                       TinhTrang = x.TinhTrang,
                                       NguoiDat = x.NguoiDat,
                                       NgayDat = x.NgayDat,
                                       NguoiXuat = x.NguoiXuat,
                                       NgayXuat = x.NgayXuat,
                                       NguoiHuy = x.NguoiHuy,
                                       NgayHuy = x.NgayHuy,
                                       NguoiHoan = x.NguoiHoan,
                                       NgayHoan = x.NgayHoan,
                                       NgayThanhToan = x.NgayThanhToan,
                                       NguoiThanhToan = x.NguoiThanhToan
                                   },
                                   Passengers = (from y in x.Passengers
                                                select new
                                                {
                                                    y.Id,
                                                    y.Type,
                                                    y.Title,
                                                    y.FullName,
                                                    y.Baggage,
                                                    y.ReturnBaggage
                                                }).ToList()
                               });
            return await queryTicket.ToListTask();
        }

        public async Task NhanXuLy(int bookId, string userId)
        {
            var book = await (from x in ctx.BookInfos
                              where x.Id == bookId
                              select x).FirstOrDefaultTask();
            if (book != null)
            {
                book.TinhTrang = 1;
                book.NgayXuLy = DateTime.Now;
                book.NguoiXuLy = userId;
                await ctx.SaveChangesAsync();
            }
        }

        public async Task CapNhatVe(int ticketId, int tinhTrang, string pnrCode, DateTime? ngayDat, DateTime? ngayXuat, DateTime? ngayThanhToan, string userId)
        {
            var ticket = await (from x in ctx.TicketInfos
                          where x.Id == ticketId
                          select x).FirstOrDefaultTask();
            if (ticket != null)
            {
                ticket.TinhTrang = tinhTrang;
                ticket.PNRCode = pnrCode;
                if (ticket.NgayDat != ngayDat && ngayDat != null)
                {
                    ticket.NgayDat = ngayDat;
                    ticket.NguoiDat = userId;
                }
                if (ticket.NgayXuat != ngayXuat && ngayXuat != null)
                {
                    ticket.NgayXuat = ngayXuat;
                    ticket.NguoiXuat = userId;
                }
                if (ticket.NgayThanhToan != ngayThanhToan && ngayThanhToan != null)
                {
                    ticket.DaThanhToan = true;
                    ticket.NgayThanhToan = ngayThanhToan;
                    ticket.NguoiThanhToan = userId;
                }
                await ctx.SaveChangesAsync();
            }
        }

        public async Task HuyDonHang(int bookId, string userId)
        {
            var book = await (from b in ctx.BookInfos
                              where b.Id == bookId
                              select b).FirstOrDefaultTask();
            if (book != null)
            {
                book.NguoiHuy = userId;
                book.NgayHuy = DateTime.Now;
                book.TinhTrang = 3;
                await ctx.SaveChangesAsync();
            }
        }
    }
}