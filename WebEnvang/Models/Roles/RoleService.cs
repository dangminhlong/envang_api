using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace WebEnvang.Models.Roles
{
    public class RoleService : BaseService
    {
        private readonly ApplicationDbContext ctx = null;
        public RoleService()
        {
            ctx = new ApplicationDbContext();
        }
        public async Task<dynamic> GetRoles()
        {
            return await (from r in ctx.Roles
                    select new
                    {
                        Id = r.Id,
                        Name = r.Name
                    }).ToListTask();
        }

        public async Task<dynamic> GetPages(RoleDTO dto)
        {
            string roleId = dto.RoleId;
            var query = (from p in ctx.Pages
                         let hasRight = ( from pg in ctx.PageRoles.Where(pg=>pg.PageId == p.Id && pg.RoleId == roleId) select pg).Any()
                         select new
                         {
                             Id = p.Id,
                             Name = p.Name,
                             RouterPath = p.RouterPath,
                             Order = p.Order,
                             HasRight = hasRight
                         });
            return await query.ToListTask();
        }

        public Task SavePageRoles(RoleDTO dto)
        {
            var roleId = dto.RoleId;
            var dsPageRoles = (from pr in ctx.PageRoles
                               where pr.RoleId == roleId
                               select pr).ToList();
            dsPageRoles.ForEach(pr =>
            {
                if (!dto.PageIdList.Contains(pr.PageId))
                {
                    ctx.PageRoles.Remove(pr);
                }
            });
            var oldIdList = dsPageRoles.Select(x => x.PageId).ToList();
            dto.PageIdList.ForEach(id =>
            {
                if (!oldIdList.Contains(id))
                {
                    var entry = new PageRole
                    {
                        PageId = id,
                        RoleId = roleId
                    };
                    ctx.PageRoles.Add(entry);
                }
            });
            return ctx.SaveChangesAsync();
        }

        public async Task<dynamic> GetPagesByUser(string userId)
        {
            DataTable dt = (await MsSqlHelper.ExecuteDataSetTask(ConnectionString, "sp_getlist_pages_by_user",
                new string[] { "@userId" },
                new object[] { userId })).Tables[0];
            var listGroup = (from r in dt.AsEnumerable()
                             orderby r.Field<int>("GroupOrder")
                             select r.Field<string>("Group")).Distinct().ToList();
            return (from g in listGroup
                    select new
                    {
                        Group = g,
                        Routers = (from r in dt.AsEnumerable()
                                   where r.Field<string>("Group") == g
                                   orderby r.Field<int>("Order")
                                   select new
                                   {
                                       Name = r.Field<object>("Name"),
                                       RouterPath = r.Field<object>("RouterPath")
                                   }).ToList()
                    });
        }
    }
}