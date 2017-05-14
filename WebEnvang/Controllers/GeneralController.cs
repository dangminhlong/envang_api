using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WebEnvang.Models.General;

namespace WebEnvang.Controllers
{
    public class GeneralController : ApiController
    {
        private GeneralService service;
        public GeneralController()
        {
            this.service = new GeneralService();
        }
        public async Task<dynamic> Get()
        {
            return await service.Get();
        }
        public async Task<dynamic> Post([FromBody]GeneralInformation dto)
        {
            try
            {
                await service.Save(dto);
                return new
                {
                    success = true
                };
            }
            catch
            {
                return new
                {
                    success = false
                };
            }
        }
    }
}