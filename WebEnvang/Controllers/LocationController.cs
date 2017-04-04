using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WebEnvang.Models.Location;

namespace WebEnvang.Controllers
{
    [Authorize(Roles="System,Admin")]
    public class LocationController : ApiController
    {
        private LocationService LocationService;
        public LocationController()
        {
            this.LocationService = new LocationService();
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<dynamic> GetList([FromBody]LocationDTO dto)
        {
            return await LocationService.GetList(dto);
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<dynamic> GetListAndGroup()
        {
            return await LocationService.GetListAndGroup();
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<dynamic> GetListDestLocationAndGroup([FromBody]LocationDTO dto)
        {
            return await LocationService.GetListDestLocationAndGroup(dto.Id);
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<dynamic> GetListDestLocationRoutedAndGroup([FromBody]LocationDTO dto)
        {
            return await LocationService.GetListDestLocationRoutedAndGroup(dto.Id);
        }
        [HttpPost]
        public async Task<dynamic> Save([FromBody]LocationDTO dto)
        {
            try
            {
                string userid = User.Identity.GetUserId();
                await LocationService.Save(dto, userid, Request.GetOwinContext().Request.RemoteIpAddress);
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
        public async Task<dynamic> Remove([FromBody]LocationDTO dto)
        {
            try
            {
                string userid = User.Identity.GetUserId();
                await LocationService.Delete(dto, userid, Request.GetOwinContext().Request.RemoteIpAddress);
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
