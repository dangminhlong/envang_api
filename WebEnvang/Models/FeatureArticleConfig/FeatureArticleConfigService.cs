using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Data;

namespace WebEnvang.Models.FeatureArticleConfig
{
    public class FeatureArticleConfigService : BaseService
    {
        private readonly ApplicationDbContext ctx = null;
        public FeatureArticleConfigService()
        {
            ctx = new ApplicationDbContext();
        }
        public async Task<dynamic> GetList(FeatureArticleConfig dto)
        {
            var query = (from e in ctx.FeatureArticleConfigs
                         where e.GroupId == dto.GroupId
                         select e);
            return await query.ToListTask();
        }
        public async Task Save(FeatureArticleConfig dto, string userId, string IP)
        {
            var entry = await (from e in ctx.FeatureArticleConfigs
                               where e.Id == dto.Id
                               select e).FirstOrDefaultTask();
            if (entry == null)
            {
                entry = new FeatureArticleConfig();
                ctx.FeatureArticleConfigs.Add(entry);
            }
            entry.Copy(dto);
            await ctx.SaveChangesAsync();
        }

        public async Task Delete(FeatureArticleConfig dto, string userId, string IP)
        {
            var entry = await (from e in ctx.FeatureArticleConfigs
                               where e.Id == dto.Id
                               select e).FirstOrDefaultTask();
            if (entry != null)
            {
                ctx.FeatureArticleConfigs.Remove(entry);
                await ctx.SaveChangesAsync();
            }
        }
    }
}