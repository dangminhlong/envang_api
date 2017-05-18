using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Http;
using WebEnvang.Models;
using WebEnvang.Models.AbayFlightScrape;
using WebEnvang.Models.Airline;
using WebEnvang.Models.BookApi;
using WebEnvang.Models.EnvietFlightScrape;
using WebEnvang.Models.WebConfig;

namespace WebEnvang.Controllers
{
    public class FlightApiController : ApiController
    {
        

        [HttpPost]
        public async Task<dynamic> BookFlight([FromBody]BookingDTO dto)
        {
            try
            {
                string userId = User.Identity.GetUserId();
                string ip = Request.GetOwinContext().Request.RemoteIpAddress;

                var bookService = new BookService();
                var bookInfoId = await bookService.BookFlight(dto, userId, ip);

                //send email thong bao thanh cong
                string template =
@"<h2>1. Th&ocirc;ng tin đơn hàng</h2>
<table style='width: 100%;'>
<tbody>
<tr>
<td style='width: 130px;'>Mã s&ocirc;́ hóa đơn</td>
<td>{{MSHD}}</td>
</tr>
<tr>
<td>Người đặt</td>
<td>{{NGUOI_DAT}}</td>
</tr>
<tr>
<td>Địa chỉ</td>
<td>{{DIA_CHI}}</td>
</tr>
<tr>
<td>Đi&ecirc;̣n thoại</td>
<td>{{DIEN_THOAI}}</td>
</tr>
<tr>
<td>Email</td>
<td>{{EMAIL}}</td>
</tr>
</tbody>
</table>
<h2>2. Th&ocirc;ng tin khách hàng</h2>
<table style='width: 100%;'>
<thead><tr style='background:#e7e7e9'><th>Loại khách</th><th>Tên khách</th><th>Hành lý chiều đi (kg)</th><th>Hành lý chiều về (kg)</th></tr></thead>
<tbody>
{{THONG_TIN_KHACH_HANG}}
</tbody>
</table>

<h2>3. Thông tin chuyến bay</h2>
<table style='width: 100%;'>
<thead>
<tr style='background:#e7e7e9'><td>Chuyến bay</td><td>Ngày</td><td>Loại vé</td><td>Khởi hành</td><td>Đến</td></tr>
</thead>
<tbody>
{{THONG_TIN_CHUYEN_BAY}}
</tbody>
</table>
<h2>4. Giá vé: {{GIA_VE}}</h2>

";
                string email = System.Configuration.ConfigurationManager.AppSettings["email"];
                string pass = System.Configuration.ConfigurationManager.AppSettings["pass"];
                bool sendEmailStatus = true;
                try
                {
                    StringBuilder sbPass = new StringBuilder();
                    for (int i=0; i < dto.Adult.Count; i++)
                    {
                        sbPass.Append(string.Format("<tr><td>Người lớn</td><td>{0}</td><td>{1}</td><td>{2}</td></tr>", dto.Adult[i].FullName, dto.Adult[i].Baggage, dto.Adult[i].ReturnBaggage));
                    }
                    for (int i = 0; i < dto.Child.Count; i++)
                    {
                        sbPass.Append(string.Format("<tr><td>Trẻ em</td><td>{0}</td><td>{1}</td><td>{2}</td></tr></tr>", dto.Child[i].FullName, dto.Child[i].Baggage, dto.Child[i].ReturnBaggage));
                    }
                    for (int i = 0; i < dto.Infant.Count; i++)
                    {
                        sbPass.Append(string.Format("<tr><td>Em bé</td><td>{0}</td><td></td><td></td></tr>", dto.Infant[i].FullName));
                    }
                    StringBuilder sbChuyenBay = new StringBuilder();
                    sbChuyenBay.Append(string.Format("<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{5} - {7} ({3})</td><td>{6} - {8} ({4})</td></tr>",
                        dto.ChieuDi.FlightNo, 
                        dto.ChieuDi.DepartureDate, 
                        dto.ChieuDi.TicketType,
                        dto.ChieuDi.FromCityCode, 
                        dto.ChieuDi.ToCityCode, 
                        dto.ChieuDi.DepartureTime.Substring(0,2) + ":" + dto.ChieuDi.DepartureTime.Substring(2), 
                        dto.ChieuDi.ArrivalTime.Substring(0,2) + ":" + dto.ChieuDi.ArrivalTime.Substring(2),
                        dto.ChieuDi.FromCity,
                        dto.ChieuDi.ToCity));
                    if (dto.ChieuVe != null)
                    {
                        sbChuyenBay.Append(string.Format("<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{5} - {7} ({3})</td><td>{6} - {8} ({4}</td>)</tr>",
                            dto.ChieuVe.FlightNo, 
                            dto.ChieuVe.DepartureDate, 
                            dto.ChieuVe.TicketType,
                            dto.ChieuVe.FromCityCode, 
                            dto.ChieuVe.ToCityCode, 
                            dto.ChieuVe.DepartureTime.Substring(0, 2) + ":" + dto.ChieuVe.DepartureTime.Substring(2), 
                            dto.ChieuVe.ArrivalTime.Substring(0, 2) + ":" + dto.ChieuVe.ArrivalTime.Substring(2),
                            dto.ChieuVe.FromCity,
                            dto.ChieuVe.ToCity));
                    }
                    string body = Regex.Replace(template, "{{MSHD}}", bookInfoId.ToString());
                    body = Regex.Replace(body, "{{NGUOI_DAT}}", dto.Contact.HoTen);
                    body = Regex.Replace(body, "{{DIA_CHI}}", dto.Contact.DiaChi);
                    body = Regex.Replace(body, "{{DIEN_THOAI}}", dto.Contact.DienThoai);
                    body = Regex.Replace(body, "{{EMAIL}}", dto.Contact.Email);
                    body = Regex.Replace(body, "{{THONG_TIN_KHACH_HANG}}", sbPass.ToString());
                    body = Regex.Replace(body, "{{THONG_TIN_CHUYEN_BAY}}", sbChuyenBay.ToString());
                    body = Regex.Replace(body, "{{GIA_VE}}", string.Format("{0:#,#}", dto.TotalPrice));
                    await TienIch.SendGMail(email, pass, dto.Contact.Email, "Thông tin đặt vé tại Én Vàng", body);
                }
                catch (Exception e)
                {
                    sendEmailStatus = false;
                }
                return new {
                    success=true,
                    sendEmailStatus = sendEmailStatus,
                    data = bookInfoId
                };
            }
            catch(Exception e)
            {
                return new {
                    success = false,
                    message = e.Message
                };
            }
        }
        
        [HttpPost]
        public async Task<dynamic> FindFlights([FromBody]FindFlight dto)
        {
            //EvScrapeService service = new EvScrapeService(50000, 50000, 0);
            var airlineService = new AirlineService();
            var webConfigService = new WebConfigService();
            var config = await webConfigService.Get();
            var airlineList = await airlineService.GetList();
            AbayScrapeService service = new AbayScrapeService(config.AdultFee, config.ChildFee, config.InfantFee, airlineList);
            string departureDate = dto.DepartDate.Split(new char[] { 'T' })[0];
            string returnDate = string.Empty;
            if (dto.RoundTrip)
            {
                returnDate = dto.ReturnDate.Split(new char[] { 'T' })[0];
            }
            return await service.SearchFlight(dto.RoundTrip, dto.FromPlace, dto.ToPlace, departureDate, returnDate, dto.Adult, dto.Child, dto.Infant);
        }
             
    }
}