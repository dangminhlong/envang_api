using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WebEnvang.Models.FlightRoute;

namespace WebEnvang.Controllers
{
    [Authorize(Roles ="System")]
    public class FlightRouteController : ApiController
    {
        private FlightRouteService FlightRouteService;
        public FlightRouteController()
        {
            this.FlightRouteService = new FlightRouteService();
        }

        public async Task<dynamic> Save([FromBody]FlightRouteDTO dto)
        {
            try
            {
                string userId = User.Identity.GetUserId();
                await this.FlightRouteService.SaveRouteInfo(dto, userId, Request.GetOwinContext().Request.RemoteIpAddress);
                return new
                {
                    success = true,
                    message = "Lưu thành công"
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
