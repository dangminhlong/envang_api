using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Data;

namespace WebEnvang.Models.Airline
{
    public class AirlineService : BaseService
    {
        private readonly ApplicationDbContext ctx = null;
        public AirlineService()
        {
            ctx = new ApplicationDbContext();
        }
        public async Task<dynamic> GetList()
        {
            return await (from e in ctx.Airlines
                          where e.IsDeleted == false
                          select e).ToListTask();
        }
        public async Task Save(Airline dto, string userId, string IP)
        {
            var entry = await (from e in ctx.Airlines
                               where e.Id == dto.Id
                               select e).FirstOrDefaultTask();
            if (entry == null)
            {
                entry = new Airline();
                ctx.Airlines.Add(entry);
            }
            entry.Name = dto.Name;
            entry.Code = dto.Code;
            entry.UserId = userId;
            entry.IP = IP;
            await ctx.SaveChangesAsync();
        }

        public async Task Delete(Airline dto, string userId, string IP)
        {
            var entry = await (from e in ctx.Airlines
                               where e.Id == dto.Id
                               select e).FirstOrDefaultTask();
            if (entry != null)
            {
                entry.IsDeleted = true;
                entry.UserId = userId;
                entry.IP = IP;
                await ctx.SaveChangesAsync();
            }
        }
    }
}