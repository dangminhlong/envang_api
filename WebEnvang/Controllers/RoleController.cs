using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WebEnvang.Models.Roles;

namespace WebEnvang.Controllers
{
    [Authorize(Roles = "System")]
    public class RoleController : ApiController
    {
        private RoleService RoleService = null;
        public RoleController()
        {
            this.RoleService = new RoleService();
        }
        [HttpGet]
        public async Task<dynamic> Get()
        {
            return await RoleService.GetRoles();
        }

        [HttpPost]
        public async Task<dynamic> GetPages([FromBody]RoleDTO dto)
        {
            return await RoleService.GetPages(dto);
        }

        [HttpPost]
        public async Task<dynamic> SavePageRoles([FromBody]RoleDTO dto)
        {
            try
            {
                await RoleService.SavePageRoles(dto);
                return new
                {
                    success = true
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
