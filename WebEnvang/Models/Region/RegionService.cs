using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Data;

namespace WebEnvang.Models.Region
{
    public class RegionService : BaseService
    {
        private readonly ApplicationDbContext ctx = null;
        public RegionService()
        {
            ctx = new ApplicationDbContext();
        }
        public async Task<dynamic> GetList()
        {
            var query = (from e in ctx.Regions
                         select e);
            return await query.ToListTask();
        }
        public async Task Save(Region dto, string userId, string IP)
        {
            var entry = await (from e in ctx.Regions
                         where e.Id == dto.Id
                         select e).FirstOrDefaultTask();
            if (entry == null)
            {
                entry = new Region();
                ctx.Regions.Add(entry);
            }
            entry.Copy(dto);
            await ctx.SaveChangesAsync();
        }

        public async Task Delete(Region dto, string userId, string IP)
        {
            var entry = await (from e in ctx.Regions
                               where e.Id == dto.Id
                               select e).FirstOrDefaultTask();
            if (entry != null)
            {
                ctx.Regions.Remove(entry);
                await ctx.SaveChangesAsync();
            }
        }
    }
}