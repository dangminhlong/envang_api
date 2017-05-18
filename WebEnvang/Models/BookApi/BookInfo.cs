using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebEnvang.Models.BookApi
{
    [Table("BookInfo")]
    public class BookInfo
    {
        public BookInfo()
        {
            Tickets = new Collection<TicketInfo>();
        }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        public bool RoundTrip { get; set; }
        public string FromPlaceCode { get; set; }
        public string ToPlaceCode { get; set; }
        public int FromPlaceId { get; set; }
        public int ToPlaceId { get; set; }
        public string DepartDate { get; set; }
        public string ReturnDate { get; set; }
        public string LienHeHoTen { get; set; }
        public string LienHeDienThoai { get; set; }
        public string LienHeEmail { get; set; }
        public string LienHeDiaChi { get; set; }
        public int PaymentMethodId { get; set; }
        public string PaymentMethodMessage { get; set; }
        public DateTime NgayDat { get; set; }
        public string NguoiDat { get; set; }
        public string IP { get; set; }
        public int TinhTrang { get; set; } //0: Dat, 1:XuLy, 2:Huy
        public string NguoiXuLy { get; set; }
        public DateTime? NgayXuLy { get; set; }
        public string NguoiHuy { get; set; }
        public DateTime? NgayHuy { get; set; }

        public virtual ICollection<TicketInfo> Tickets { get; set; }
    }
}