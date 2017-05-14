using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace WebEnvang.Models.FlightRoute
{
    public class FlightRouteService : BaseService
    {
        private readonly ApplicationDbContext ctx = null;
        public FlightRouteService()
        {
            ctx = new ApplicationDbContext();
        }
        public async Task SaveRouteInfo(FlightRouteDTO dto, string userId, string IP)
        {
            var frList = (from e in ctx.FlightRoutes
                          where e.SrcLocationId == dto.SourceId || e.DestLocationId == dto.SourceId
                          select e).ToList();
            frList.ForEach(fr =>
            {
                bool bRoute = false;
                foreach (int id in dto.DestIdList)
                {
                    if (id == fr.SrcLocationId || id == fr.DestLocationId)
                    {
                        bRoute = true;
                        break;
                    }
                }
                if (!bRoute)
                {
                    ctx.FlightRoutes.Remove(fr);
                }
            });

            dto.DestIdList.ForEach(id =>
            {
                var entry = frList.Where(fr => fr.SrcLocationId == dto.SourceId && fr.DestLocationId == id).FirstOrDefault();
                if (entry == null)
                {
                    var entry1 = new FlightRoute
                    {
                        SrcLocationId = dto.SourceId,
                        DestLocationId = id
                    };
                    var entry2 = new FlightRoute
                    {
                        SrcLocationId = id,
                        DestLocationId = dto.SourceId
                    };
                    ctx.FlightRoutes.Add(entry1);
                    ctx.FlightRoutes.Add(entry2);
                }
            });
            await ctx.SaveChangesAsync();
        }
    }
}