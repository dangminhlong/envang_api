using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WebEnvang.Models.Article;

namespace WebEnvang.Controllers
{
    [Authorize(Roles = "System,Admin")]
    public class ArticleController : ApiController
    {
        private ArticleService ArticleService;
        public ArticleController()
        {
            this.ArticleService = new ArticleService();
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<dynamic> GetList([FromBody]ArticleDTO dto)
        {
            return await ArticleService.GetList(dto);
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<dynamic> GetItem([FromBody]ArticleDTO dto)
        {
            return await ArticleService.GetItem(dto);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<dynamic> GetLatest()
        {
            return await ArticleService.GetLatest();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<dynamic> GetListView([FromBody]ArticleDTO dto)
        {
            return await ArticleService.GetListView(dto.Page, dto.PageSize);
        }

        [HttpPost]
        public async Task<dynamic> Save([FromBody]ArticleDTO dto)
        {
            try
            {
                string userid = User.Identity.GetUserId();
                await ArticleService.Save(dto, userid, Request.GetOwinContext().Request.RemoteIpAddress);
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
        public async Task<dynamic> Remove([FromBody]ArticleDTO dto)
        {
            try
            {
                string userid = User.Identity.GetUserId();
                await ArticleService.Delete(dto, userid, Request.GetOwinContext().Request.RemoteIpAddress);
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
