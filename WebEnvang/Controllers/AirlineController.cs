using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WebEnvang.Models.Airline;

namespace WebEnvang.Controllers
{
    [Authorize(Roles = "System,Admin")]
    public class AirlineController : ApiController
    {
        private AirlineService AirlineService;
        public AirlineController()
        {
            this.AirlineService = new AirlineService();
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<dynamic> GetList()
        {
            return await AirlineService.GetList();
        }
        [HttpPost]
        public async Task<dynamic> Save([FromBody]AirlineDTO dto)
        {
            try
            {
                string userid = User.Identity.GetUserId();
                await AirlineService.Save(dto, userid, Request.GetOwinContext().Request.RemoteIpAddress);
                return new
                {
                    success = true,
                    message = "Lưu thành công"
                };
            }
            catch(Exception e)
            {
                return new
                {
                    success = false,
                    message = e.Message
                };
            }
        }
        [HttpPost]
        public async Task<dynamic> Remove([FromBody]AirlineDTO dto)
        {
            try
            {
                string userid = User.Identity.GetUserId();
                await AirlineService.Delete(dto, userid, Request.GetOwinContext().Request.RemoteIpAddress);
                return new
                {
                    success = true,
                    message = "Xóa thành công"
                };
            }
            catch (Exception e)
            {
                return new
                {
                    success = false,
                    message = e.Message
                };
            }
        }
    }
}
