using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebEnvang.Models.BookApi
{
    public class BookingDTO
    {
        public bool RoundTrip { get; set; }
        public string FromPlaceCode { get; set; }
        public string ToPlaceCode { get; set; }
        public int FromPlaceId { get; set; }
        public int ToPlaceId { get; set; }
        public string DepartDate { get; set; }
        public string ReturnDate { get; set; }
        public IList<Passenger> Adult { get; set; }
        public IList<Passenger> Child { get; set; }
        public IList<Passenger> Infant { get; set; }
        public BookingFlightInfo ChieuDi { get; set; }
        public BookingFlightInfo ChieuVe { get; set; }
        public BookPaymentMethod PaymentMethod { get; set; }
        public Contact Contact { get; set; }
        public decimal TotalPrice { get; set; }
        public string CouponCode { get; set; }
        public string CouponDiscount { get; set; }

    }
}