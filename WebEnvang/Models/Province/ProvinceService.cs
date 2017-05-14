using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Data;

namespace WebEnvang.Models.Province
{
    public class ProvinceService : BaseService
    {
        private readonly ApplicationDbContext ctx = null;
        public ProvinceService()
        {
            ctx = new ApplicationDbContext();
        }
        public async Task<dynamic> GetList()
        {
            var query = (from e in ctx.Provinces
                         where e.IsDeleted == false
                         select e);
            return await query.ToListTask();
        }
        public async Task Save(Province dto, string userId, string IP)
        {
            var entry = await (from e in ctx.Provinces
                               where e.Id == dto.Id
                               select e).FirstOrDefaultTask();
            if (entry == null)
            {
                entry = new Province();
                ctx.Provinces.Add(entry);
            }
            entry.Name = dto.Name;
            entry.UserId = userId;
            entry.IP = IP;
            await ctx.SaveChangesAsync();
        }

        public async Task Delete(Province dto, string userId, string IP)
        {
            var entry = await (from e in ctx.Provinces
                               where e.Id == dto.Id
                               select e).FirstOrDefaultTask();
            if (entry != null)
            {
                entry.UserId = userId;
                entry.IP = IP;
                entry.IsDeleted = true;
                await ctx.SaveChangesAsync();
            }
        }
    }
}