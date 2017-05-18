using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace WebEnvang.Models.BookApi
{
    public class BookService : BaseService
    {
        private readonly ApplicationDbContext ctx = null;
        public BookService()
        {
            ctx = new ApplicationDbContext();
        }

        public async Task<int> BookFlight(BookingDTO dto, string userId, string Ip)
        {
            BookInfo bookInfo = new BookInfo()
            {
                RoundTrip = dto.RoundTrip,
                FromPlaceCode = dto.FromPlaceCode,
                ToPlaceCode = dto.ToPlaceCode,
                FromPlaceId = dto.FromPlaceId,
                ToPlaceId = dto.ToPlaceId,
                DepartDate = dto.DepartDate,
                ReturnDate = dto.ReturnDate,
                LienHeHoTen = dto.Contact.HoTen,
                LienHeDiaChi = dto.Contact.DiaChi,
                LienHeDienThoai = dto.Contact.DienThoai,
                LienHeEmail = dto.Contact.Email,
                PaymentMethodId = dto.PaymentMethod.Id,
                PaymentMethodMessage = dto.PaymentMethod.CustomerResponse,
                TinhTrang = 0,
                NgayDat = DateTime.Now,
                NguoiDat = userId,
                IP = Ip                
            };
            

            var adults = (from a in dto.Adult
                          select new TicketPassenger
                          {
                              Type = 0,
                              FullName = a.FullName,
                              Title = a.Title,
                              Baggage = a.Baggage,
                              ReturnBaggage = a.ReturnBaggage
                          }).ToList();
            var childs = (from a in dto.Child
                          select new TicketPassenger
                          {
                              Type = 1,
                              FullName = a.FullName,
                              Title = a.Title,
                              Baggage = a.Baggage,
                              ReturnBaggage = a.ReturnBaggage
                          }).ToList();
            var infants = (from a in dto.Infant
                          select new TicketPassenger
                          {
                              Type = 2,
                              FullName = a.FullName,
                              Title = a.Title
                          }).ToList();
            if (dto.RoundTrip && dto.ChieuDi.Airline == dto.ChieuVe.Airline)
            {
                var ticket = new TicketInfo()
                {
                    Airline = dto.ChieuDi.Airline,
                    IsRoundTrip = true,
                    FlightNo = dto.ChieuDi.FlightNo,
                    TicketFare = dto.ChieuDi.TicketFare,
                    TicketPrice = dto.ChieuDi.TicketPrice,
                    TicketType = dto.ChieuDi.TicketType,
                    DepartureTime = dto.ChieuDi.DepartureTime,
                    DepartureDate = dto.ChieuDi.DepartureDate,
                    ArrivalDate = dto.ChieuDi.ArrivalDate,
                    ArrivalTime = dto.ChieuDi.ArrivalTime,
                    FromCityCode = dto.FromPlaceCode,
                    ToCityCode = dto.ToPlaceCode,
                    NgayDat = DateTime.Now,
                    NguoiDat = userId,
                    ReturnArrivalDate = dto.ChieuVe.ArrivalDate,
                    ReturnArrivalTime = dto.ChieuVe.ArrivalTime,
                    ReturnDepartureDate = dto.ChieuVe.DepartureDate,
                    ReturnDepartureTime = dto.ChieuVe.DepartureTime,
                    ReturnFlightNo = dto.ChieuVe.FlightNo,
                    ReturnTicketFare = dto.ChieuVe.TicketFare,
                    ReturnTicketPrice = dto.ChieuVe.TicketPrice,
                    ReturnTicketType = dto.ChieuVe.TicketType,
                    TinhTrang = 0
                };
                adults.ForEach(p => { ticket.Passengers.Add(p); });
                childs.ForEach(p => { ticket.Passengers.Add(p); });
                infants.ForEach(p => { ticket.Passengers.Add(p); });
                bookInfo.Tickets.Add(ticket);
            }
            else
            {
                var ticket1 = new TicketInfo()
                {
                    Airline = dto.ChieuDi.Airline,
                    IsRoundTrip = false,
                    FlightNo = dto.ChieuDi.FlightNo,
                    TicketFare = dto.ChieuDi.TicketFare,
                    TicketPrice = dto.ChieuDi.TicketPrice,
                    TicketType = dto.ChieuDi.TicketType,
                    DepartureTime = dto.ChieuDi.DepartureTime,
                    DepartureDate = dto.ChieuDi.DepartureDate,
                    ArrivalDate = dto.ChieuDi.ArrivalDate,
                    ArrivalTime = dto.ChieuDi.ArrivalTime,
                    FromCityCode = dto.FromPlaceCode,
                    ToCityCode = dto.ToPlaceCode,
                    NgayDat = DateTime.Now,
                    NguoiDat = userId,
                    TinhTrang = 0
                };

                adults.ForEach(p => { ticket1.Passengers.Add(p); });
                childs.ForEach(p => { ticket1.Passengers.Add(p); });
                infants.ForEach(p => { ticket1.Passengers.Add(p); });
                bookInfo.Tickets.Add(ticket1);
                if (dto.RoundTrip)
                {
                    var ticket2 = new TicketInfo()
                    {
                        Airline = dto.ChieuVe.Airline,
                        IsRoundTrip = false,
                        FlightNo = dto.ChieuVe.FlightNo,
                        TicketFare = dto.ChieuVe.TicketFare,
                        TicketPrice = dto.ChieuVe.TicketPrice,
                        TicketType = dto.ChieuVe.TicketType,
                        DepartureTime = dto.ChieuVe.DepartureTime,
                        DepartureDate = dto.ChieuVe.DepartureDate,
                        ArrivalDate = dto.ChieuVe.ArrivalDate,
                        ArrivalTime = dto.ChieuVe.ArrivalTime,
                        FromCityCode = dto.ToPlaceCode,
                        ToCityCode = dto.FromPlaceCode,
                        NgayDat = DateTime.Now,
                        NguoiDat = userId,
                        TinhTrang = 0
                    };
                    adults.ForEach(p => { ticket2.Passengers.Add(p); });
                    childs.ForEach(p => { ticket2.Passengers.Add(p); });
                    infants.ForEach(p => { ticket2.Passengers.Add(p); });
                    bookInfo.Tickets.Add(ticket2);
                }
            }
            ctx.BookInfos.Add(bookInfo);
            await ctx.SaveChangesAsync();
            return bookInfo.Id;
        }
    }
}