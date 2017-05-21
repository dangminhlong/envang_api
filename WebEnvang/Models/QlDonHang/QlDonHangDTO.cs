using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebEnvang.Models.QlDonHang
{
    public class QlDonHangDTO
    {
        public int bookId { get; set; }
        public int tinhtrang { get; set; }
        public DateTime tungay { get; set; }
        public DateTime denngay { get; set; }
        public string nguoidat { get; set; }
        public string soDtNguoiGT { get; set; }
        public int ticketId { get; set; }
        public string pnrCode { get; set; }
        public DateTime? ngayDat { get; set; }
        public DateTime? ngayXuat { get; set; }
        public DateTime? ngayThanhToan { get; set; }
    }
}