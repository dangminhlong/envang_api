using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebEnvang.Models.BookApi
{
    [Table("TicketInfo")]
    public class TicketInfo
    {
        public TicketInfo()
        {
            Passengers = new Collection<TicketPassenger>();
        }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        public int BookInfoId { get; set; }
        public bool IsRoundTrip { get; set; }
        public string Airline { get; set; }
        public string FlightNo { get; set; }
        public decimal TicketPrice { get; set; }
        public decimal TicketFare { get; set; }
        public string TicketType { get; set; }
        public string FromCityCode { get; set; }
        public string ToCityCode { get; set; }
        public string DepartureDate { get; set; }
        public string DepartureTime { get; set; }
        public string ArrivalDate { get; set; }
        public string ArrivalTime { get; set; }
        public string ReturnFlightNo { get; set; }
        public decimal? ReturnTicketPrice { get; set; }
        public decimal? ReturnTicketFare { get; set; }
        public string ReturnTicketType { get; set; }
        public string ReturnDepartureDate { get; set; }
        public string ReturnDepartureTime { get; set; }
        public string ReturnArrivalDate { get; set; }
        public string ReturnArrivalTime { get; set; }
        public string GhiChu { get; set; }
        public string PNRCode { get; set; }
        public int TinhTrang { get; set; } //0:Chua Dat,1:Da Dat,2:Da Xuat, 3:Huy
        public bool DaThanhToan { get; set; }
        public string NguoiDat { get; set; }
        public DateTime? NgayDat { get; set; }
        public string NguoiXuat { get; set; }
        public DateTime? NgayXuat { get; set; }
        public string NguoiHuy { get; set; }
        public DateTime? NgayHuy { get; set; }
        public string NguoiHoan { get; set; }
        public DateTime? NgayHoan { get; set; }
        public string NguoiThanhToan { get; set; }
        public DateTime? NgayThanhToan { get; set; }

        public virtual ICollection<TicketPassenger> Passengers { get; set; }
        public virtual BookInfo BookInfo { get; set; }
    }
}