using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WebEnvang.Models.QlDonHang;

namespace WebEnvang.Controllers
{
    [Authorize(Roles ="System,Admin,Booker")]
    public class QlDonHangController : ApiController
    {
        QlDonHangService service;
        public QlDonHangController()
        {
            service = new QlDonHangService();
        }
        [HttpPost]
        public async Task<dynamic> LayDsDonHang([FromBody]QlDonHangDTO dto)
        {
            return await service.LayDsDonHang(dto.tinhtrang, dto.tungay.Date, dto.denngay.Date.AddDays(1), dto.nguoidat, dto.soDtNguoiGT);
        }
        [HttpPost]
        public async Task<dynamic> LayThongTinChiTiet([FromBody]QlDonHangDTO dto)
        {
            return await service.LayChiTietDonHang(dto.bookId);
        }
        [HttpPost]
        public async Task<dynamic> NhanXuLy([FromBody]QlDonHangDTO dto)
        {
            try
            {
                string userId = User.Identity.GetUserId();
                await service.NhanXuLy(dto.bookId, userId);
                return new
                {
                    success = true
                };
            }
            catch
            {
                return new
                {
                    success = false
                };
            }
        }
        [HttpPost]
        public async Task<dynamic> CapNhatVe([FromBody]QlDonHangDTO dto)
        {
            try
            {
                string userId = User.Identity.GetUserId();
                await service.CapNhatVe(dto.ticketId, dto.tinhtrang, dto.pnrCode, dto.ngayDat, dto.ngayXuat, dto.ngayThanhToan, userId);
                return new
                {
                    success = true
                };
            }
            catch
            {
                return new
                {
                    success = false
                };
            }
        }

        [HttpPost]
        public async Task<dynamic> HuyDonHang([FromBody]QlDonHangDTO dto)
        {
            try
            {
                string userId = User.Identity.GetUserId();
                await service.HuyDonHang(dto.bookId, userId);
                return new
                {
                    success = true
                };
            }
            catch
            {
                return new
                {
                    success = false
                };
            }
        }
    }
}