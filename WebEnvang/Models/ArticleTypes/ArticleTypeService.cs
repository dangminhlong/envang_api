using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Data;

namespace WebEnvang.Models.ArticleTypes
{
    public class ArticleTypeService : BaseService
    {
        private readonly ApplicationDbContext ctx = null;
        public ArticleTypeService()
        {
            ctx = new ApplicationDbContext();
        }
        public async Task<dynamic> GetList()
        {
            var query = (from e in ctx.ArticleTypes
                         select e);
            return await query.ToListTask();
        }
        public async Task Save(ArticleType dto, string userId, string IP)
        {
            var entry = await (from e in ctx.ArticleTypes
                               where e.Id == dto.Id
                               select e).FirstOrDefaultTask();
            if (entry == null)
            {
                entry = new ArticleType();
                ctx.ArticleTypes.Add(entry);
            }
            entry.Name = dto.Name;
            await ctx.SaveChangesAsync();
        }

        public async Task Delete(ArticleType dto, string userId, string IP)
        {
            var entry = await (from e in ctx.ArticleTypes
                               where e.Id == dto.Id
                               select e).FirstOrDefaultTask();
            if (entry != null)
            {
                ctx.ArticleTypes.Remove(entry);
                await ctx.SaveChangesAsync();
            }
        }
    }
}