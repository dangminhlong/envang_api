using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WebEnvang.Models.FeatureArticleConfigGroup;

namespace WebEnvang.Controllers
{
    [Authorize(Roles = "System,Admin")]
    public class FeatureArticleConfigGroupController : ApiController
    {
        private FeatureArticleConfigGroupService FeatureArticleConfigGroupService;
        public FeatureArticleConfigGroupController()
        {
            this.FeatureArticleConfigGroupService = new FeatureArticleConfigGroupService();
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<dynamic> GetList()
        {
            return await FeatureArticleConfigGroupService.GetList();
        }
        [HttpPost]
        public async Task<dynamic> Save([FromBody]FeatureArticleConfigGroup dto)
        {
            try
            {
                string userid = User.Identity.GetUserId();
                await FeatureArticleConfigGroupService.Save(dto, userid, Request.GetOwinContext().Request.RemoteIpAddress);
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
        public async Task<dynamic> Remove([FromBody]FeatureArticleConfigGroup dto)
        {
            try
            {
                string userid = User.Identity.GetUserId();
                await FeatureArticleConfigGroupService.Delete(dto, userid, Request.GetOwinContext().Request.RemoteIpAddress);
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
