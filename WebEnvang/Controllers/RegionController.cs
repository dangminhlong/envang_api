using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WebEnvang.Models.Region;

namespace WebEnvang.Controllers
{
    [Authorize(Roles = "System,Admin")]
    public class RegionController : ApiController
    {
        private RegionService RegionService;
        public RegionController()
        {
            this.RegionService = new RegionService();
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<dynamic> GetList()
        {
            return await RegionService.GetList();
        }
        [HttpPost]
        public async Task<dynamic> Save([FromBody]RegionDTO dto)
        {
            try
            {
                string userid = User.Identity.GetUserId();
                await RegionService.Save(dto, userid, Request.GetOwinContext().Request.RemoteIpAddress);
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
        public async Task<dynamic> Remove([FromBody]RegionDTO dto)
        {
            try
            {
                string userid = User.Identity.GetUserId();
                await RegionService.Delete(dto, userid, Request.GetOwinContext().Request.RemoteIpAddress);
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
