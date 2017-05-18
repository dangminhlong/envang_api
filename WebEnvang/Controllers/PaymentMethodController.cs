using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WebEnvang.Models.PaymentMethod;

namespace WebEnvang.Controllers
{
    [Authorize(Roles = "System,Admin")]
    public class PaymentMethodController : ApiController
    {
        private PaymentMethodService PaymentMethodService;
        public PaymentMethodController()
        {
            this.PaymentMethodService = new PaymentMethodService();
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<dynamic> GetList()
        {
            return await PaymentMethodService.GetList();
        }
        [HttpPost]
        public async Task<dynamic> Save([FromBody]PaymentMethod dto)
        {
            try
            {
                await PaymentMethodService.Save(dto);
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
        public async Task<dynamic> Remove([FromBody]PaymentMethod dto)
        {
            try
            {
                await PaymentMethodService.Delete(dto);
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
