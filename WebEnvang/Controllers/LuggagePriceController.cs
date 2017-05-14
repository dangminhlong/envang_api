using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WebEnvang.Models.LuggagePrice;

namespace WebEnvang.Controllers
{
    [Authorize(Roles = "System,Admin")]
    public class LuggagePriceController : ApiController
    {
        private LuggagePriceService LuggagePriceService;
        public LuggagePriceController()
        {
            this.LuggagePriceService = new LuggagePriceService();
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<dynamic> GetList([FromBody]LuggagePrice dto)
        {
            return await LuggagePriceService.GetList(dto);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<dynamic> GetListByAirlineCode([FromBody]string Code)
        {
            return await LuggagePriceService.GetListByAirlineCode(Code);
        }

        [HttpPost]
        public async Task<dynamic> Save([FromBody]LuggagePrice dto)
        {
            try
            {
                string userid = User.Identity.GetUserId();
                await LuggagePriceService.Save(dto, userid, Request.GetOwinContext().Request.RemoteIpAddress);
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
        public async Task<dynamic> Remove([FromBody]LuggagePrice dto)
        {
            try
            {
                string userid = User.Identity.GetUserId();
                await LuggagePriceService.Delete(dto, userid, Request.GetOwinContext().Request.RemoteIpAddress);
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
