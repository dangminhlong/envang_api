﻿using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WebEnvang.Models.ArticleTypes;

namespace WebEnvang.Controllers
{
    [Authorize(Roles = "System,Admin")]
    public class ArticleTypeController : ApiController
    {
        private ArticleTypeService ArticleTypeService;
        public ArticleTypeController()
        {
            this.ArticleTypeService = new ArticleTypeService();
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<dynamic> GetList()
        {
            return await ArticleTypeService.GetList();
        }
        [HttpPost]
        public async Task<dynamic> Save([FromBody]ArticleType dto)
        {
            try
            {
                string userid = User.Identity.GetUserId();
                await ArticleTypeService.Save(dto, userid, Request.GetOwinContext().Request.RemoteIpAddress);
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
        public async Task<dynamic> Remove([FromBody]ArticleType dto)
        {
            try
            {
                string userid = User.Identity.GetUserId();
                await ArticleTypeService.Delete(dto, userid, Request.GetOwinContext().Request.RemoteIpAddress);
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
