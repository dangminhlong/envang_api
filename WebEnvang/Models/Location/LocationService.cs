using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Data;

namespace WebEnvang.Models.Location
{
    public class LocationService : BaseService
    {
        private readonly ApplicationDbContext ctx = null;
        public LocationService()
        {
            ctx = new ApplicationDbContext();
        }
        public async Task<dynamic> GetList(LocationDTO dto)
        {
            var query = (from l in ctx.Locations
                         join r in ctx.Regions on l.RegionId equals r.Id
                         where l.RegionId == dto.RegionId || dto.RegionId == 0
                         select new
                         {
                             Id = l.Id,
                             Name = l.Name,
                             Code = l.Code,
                             RegionId = l.RegionId,
                             RegionName = r.Name,
                             Order = l.Order,
                             RegionColumn = r.Column,
                             RegionOrder = r.Order
                         });
            return await query.ToListTask();
        }

        public async Task<dynamic> GetListAndGroup()
        {
            var query = (from r in ctx.Regions
                         select new
                         {
                             RegionId = r.Id,
                             RegionColumn = r.Column,
                             RegionOrder = r.Order,
                             RegionName = r.Name,
                             LocationList = (from l in ctx.Locations
                                             where l.RegionId == r.Id
                                             select new
                                             {
                                                 Id = l.Id,
                                                 Name = l.Name,
                                                 Code = l.Code
                                             }).ToList()
                         });
            return await query.ToListTask();
        }

        public async Task<dynamic> GetListDestLocationAndGroup(int sourceId)
        {
            var query = (from r in ctx.Regions
                         select new
                         {
                             RegionId = r.Id,
                             RegionColumn = r.Column,
                             RegionOrder = r.Order,
                             RegionName = r.Name,
                             LocationList = (from l in ctx.Locations
                                             where l.RegionId == r.Id && l.Id != sourceId
                                             let isRouted = (from fr in ctx.FlightRoutes
                                                             where fr.SrcLocationId == sourceId && fr.DestLocationId == l.Id
                                                             select fr).Any()
                                             select new
                                             {
                                                 Id = l.Id,
                                                 Name = l.Name,
                                                 Code = l.Code,
                                                 Routed = isRouted
                                             }).ToList()
                         });
            return await query.ToListTask();
        }

        public async Task<dynamic> GetListDestLocationRoutedAndGroup(int sourceId)
        {
            var query = (from r in ctx.Regions
                         select new
                         {
                             RegionId = r.Id,
                             RegionColumn = r.Column,
                             RegionOrder = r.Order,
                             RegionName = r.Name,
                             LocationList = (from l in ctx.Locations
                                             let isRouted = (from fr in ctx.FlightRoutes
                                                             where fr.SrcLocationId == sourceId && fr.DestLocationId == l.Id && l.Id != sourceId
                                                             select fr).Any()
                                             where l.RegionId == r.Id && isRouted == true                                             
                                             select new
                                             {
                                                 Id = l.Id,
                                                 Name = l.Name,
                                                 Code = l.Code
                                             }).ToList()
                         });
            return await query.ToListTask();
        }

        public async Task Save(LocationDTO dto, string userId, string IP)
        {
            var entry = await (from e in ctx.Locations
                               where e.Id == dto.Id
                               select e).FirstOrDefaultTask();
            if (entry == null)
            {
                entry = new Location();
                ctx.Locations.Add(entry);
            }
            entry.Copy(dto);
            await ctx.SaveChangesAsync();
        }

        public async Task Delete(LocationDTO dto, string userId, string IP)
        {
            var entry = await (from e in ctx.Locations
                               where e.Id == dto.Id
                               select e).FirstOrDefaultTask();
            if (entry != null)
            {
                entry = new Location();
                ctx.Locations.Remove(entry);
                await ctx.SaveChangesAsync();
            }
        }
    }
}