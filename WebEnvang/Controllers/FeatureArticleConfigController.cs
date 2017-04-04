using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WebEnvang.Models.FeatureArticleConfig;

namespace WebEnvang.Controllers
{
    [Authorize(Roles = "System,Admin")]
    public class FeatureArticleConfigController : ApiController
    {
        private FeatureArticleConfigService FeatureArticleConfigService;
        public FeatureArticleConfigController()
        {
            this.FeatureArticleConfigService = new FeatureArticleConfigService();
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<dynamic> GetList([FromBody]FeatureArticleConfigDTO dto)
        {
            return await FeatureArticleConfigService.GetList(dto);
        }
        [HttpPost]
        public async Task<dynamic> Save([FromBody]FeatureArticleConfigDTO dto)
        {
            try
            {
                string userid = User.Identity.GetUserId();
                await FeatureArticleConfigService.Save(dto, userid, Request.GetOwinContext().Request.RemoteIpAddress);
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
        public async Task<dynamic> Remove([FromBody]FeatureArticleConfigDTO dto)
        {
            try
            {
                string userid = User.Identity.GetUserId();
                await FeatureArticleConfigService.Delete(dto, userid, Request.GetOwinContext().Request.RemoteIpAddress);
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
