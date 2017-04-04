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
        
        public async Task<dynamic> GetRoles()
        {
            DataTable dt = (await MsSqlHelper.ExecuteDataSetTask(ConnectionString, "sp_role_getlist")).Tables[0];
            return (from r in dt.AsEnumerable()
                    select new
                    {
                        Id = r.Field<object>("Id"),
                        Name = r.Field<object>("Name")
                    }).ToList();
        }

        public async Task<dynamic> GetPages(RoleDTO dto)
        {
            DataTable dt = (await MsSqlHelper.ExecuteDataSetTask(ConnectionString, "sp_getlist_pages",
                new string[] { "@roleId" },
                new object[] { dto.RoleId })).Tables[0];
            return (from r in dt.AsEnumerable()
                    select new
                    {
                        Id = r.Field<object>("Id"),
                        Name = r.Field<object>("Name"),
                        RouterPath = r.Field<object>("RouterPath"),
                        Order = r.Field<object>("Order"),
                        HasRight = r.Field<object>("HasRight")
                    }).ToList();
        }

        public Task SavePageRoles(RoleDTO dto)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Value", typeof(int));
            for (int i=0; i < dto.PageIdList.Count; i++)
            {
                var row = dt.NewRow();
                row["Value"] = dto.PageIdList[i];
                dt.Rows.Add(row);
            }
            return MsSqlHelper.ExecuteNonQueryTask(ConnectionString, "sp_role_save_page",
                new string[] { "@roleId", "@table" },
                new object[] { dto.RoleId, dt });
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