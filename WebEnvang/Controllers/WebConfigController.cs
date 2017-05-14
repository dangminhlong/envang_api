using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WebEnvang.Models.WebConfig;

namespace WebEnvang.Controllers
{
    [Authorize]
    public class WebConfigController : ApiController
    {
        private WebConfigService WebConfigService = null;
        public WebConfigController()
        {
            this.WebConfigService = new WebConfigService();
        }
        [AllowAnonymous]
        public async Task<dynamic> Get()
        {
            return await this.WebConfigService.Get();
        }
        public async Task<dynamic> Post([FromBody]WebConfig dto)
        {
            try
            {
                string userid = User.Identity.GetUserId();
                await this.WebConfigService.Save(dto, userid, Request.GetOwinContext().Request.RemoteIpAddress);
                return new {
                    success = true
                };
            }
            catch (Exception e)
            {
                return new {
                    success = false,
                    message = e.Message
                };
            }
        }
    }
}