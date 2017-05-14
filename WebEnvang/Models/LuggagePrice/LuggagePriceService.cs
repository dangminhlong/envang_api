using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Data;

namespace WebEnvang.Models.LuggagePrice
{
    public class LuggagePriceService : BaseService
    {
        private readonly ApplicationDbContext ctx = null;
        public LuggagePriceService()
        {
            ctx = new ApplicationDbContext();
        }
        public async Task<dynamic> GetList(LuggagePrice dto)
        {
            var query = (from e in ctx.LuggagePrices
                         where e.AirlineId == dto.AirlineId && e.IsDeleted == false
                         orderby e.Weight
                         select e);
            return await query.ToListTask();
        }
        public async Task<dynamic> GetListByAirlineCode(string code)
        {
            var query = (from lp in ctx.LuggagePrices
                         join ar in ctx.Airlines on lp.AirlineId equals ar.Id
                         where ar.Code == code && lp.IsDeleted == false
                         orderby lp.Weight
                         select lp);
            return await query.ToListTask();
        }
        public async Task Save(LuggagePrice dto, string userId, string IP)
        {
            var entry = await (from e in ctx.LuggagePrices
                               where e.Id == dto.Id
                               select e).FirstOrDefaultTask();
            if (entry == null)
            {
                entry = new LuggagePrice();
                ctx.LuggagePrices.Add(entry);
            }
            entry.Copy(dto);
            entry.UserId = userId;
            entry.IP = IP;
            await ctx.SaveChangesAsync();
        }

        public async Task Delete(LuggagePrice dto, string userId, string IP)
        {
            var entry = await(from e in ctx.LuggagePrices
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