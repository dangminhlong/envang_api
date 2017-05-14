using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Data;

namespace WebEnvang.Models.Article
{
    public class ArticleService : BaseService
    {
        private readonly ApplicationDbContext ctx = null;
        public ArticleService()
        {
            ctx = new ApplicationDbContext();
        }
        public async Task<dynamic> GetList(ArticleDTO dto)
        {
            var tong = (from e in ctx.Articles
                         where e.ArticleTypeId == dto.ArticleTypeId
                         select e).Count();
            var query = (from e in ctx.Articles
                         where e.ArticleTypeId == dto.ArticleTypeId
                         orderby e.CreatedOn descending
                         select e).Skip((dto.Page - 1) * dto.PageSize).Take(dto.PageSize);
            return new
            {
                Data = await query.ToListTask(),
                Tong = tong
            };
        }
        public async Task<dynamic> GetItem(ArticleDTO dto)
        {
            var query = (from e in ctx.Articles
                         where e.Id == dto.Id
                         select e);
            return await query.FirstOrDefaultTask();
        }

        public async Task<dynamic> GetLatest()
        {
            var query = (from e in ctx.Articles
                         orderby e.CreatedOn descending
                         select e);
            return await query.FirstOrDefaultTask();
        }

        public async Task<dynamic> GetListView(int page, int pageSize)
        {
            var tong = (from e in ctx.Articles
                        select e).Count();
            var query = (from e in ctx.Articles
                         orderby e.CreatedOn descending
                         select e).Skip((page - 1) * pageSize).Take(pageSize);
            return new
            {
                Data = await query.ToListTask(),
                TotalItems = tong
            };            
        }

        public async Task Save(ArticleDTO dto, string userId, string IP)
        {
            var entry = await (from e in ctx.Articles
                               where e.Id == dto.Id
                               select e).FirstOrDefaultTask();
            if (entry == null)
            {
                entry = new Article();
                entry.CreatedBy = userId;
                entry.CreatedOn = DateTime.Now;
                ctx.Articles.Add(entry);
            }
            entry.ModifiedBy = userId;
            entry.ModifiedOn = DateTime.Now;
            entry.Copy(dto);
            await ctx.SaveChangesAsync();
        }

        public async Task Delete(ArticleDTO dto, string userId, string IP)
        {
            var entry = await (from e in ctx.Articles
                               where e.Id == dto.Id
                               select e).FirstOrDefaultTask();
            if (entry != null)
            {
                ctx.Articles.Remove(entry);
                await ctx.SaveChangesAsync();
            }
        }
    }
}