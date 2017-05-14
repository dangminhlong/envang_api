using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Data;

namespace WebEnvang.Models.FeatureArticleConfigGroup
{
    public class FeatureArticleConfigGroupService : BaseService
    {
        private readonly ApplicationDbContext ctx = null;
        public FeatureArticleConfigGroupService()
        {
            ctx = new ApplicationDbContext();
        }
        public async Task<dynamic> GetList()
        {

            return await (from r in ctx.FeatureArticleConfigGroups
                    select r).ToListTask();
        }
        public Task Save(FeatureArticleConfigGroup dto, string userId, string IP)
        {
            var entry = (from r in ctx.FeatureArticleConfigGroups
                         where r.Id == dto.Id
                         select r).FirstOrDefault();
            if (entry == null)
            {
                entry = new FeatureArticleConfigGroup();
                entry.Copy(dto);
                ctx.FeatureArticleConfigGroups.Add(entry);
            }
            else
            {
                entry.Copy(dto);
            }
            return ctx.SaveChangesAsync();
        }

        public async Task Delete(FeatureArticleConfigGroup dto, string userId, string IP)
        {
            var entry = (from r in ctx.FeatureArticleConfigGroups
                         where r.Id == dto.Id
                         select r).FirstOrDefault();
            if (entry != null)
            {
                ctx.FeatureArticleConfigGroups.Remove(entry);
                await ctx.SaveChangesAsync();
            }
        }
    }
}