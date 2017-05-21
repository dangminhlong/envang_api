using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace WebEnvang.Models.Users
{
    public class UserService : BaseService
    {
        private readonly ApplicationDbContext ctx = null;
        public UserService()
        {
            ctx = new ApplicationDbContext();
        }
        public async Task<dynamic> GetList(UserDTO dto)
        {

            DataSet ds = (await MsSqlHelper.ExecuteDataSetTask(ConnectionString, "sp_user_getlist",
                new string[] { "@username", "@roleId" },
                new object[] { dto.UserName, dto.RoleId }));
            var dt = ds.Tables[0];
            var tong = Convert.ToInt32(ds.Tables[1].Rows[0][0]);

            return new
            {
                Data = (from r in dt.AsEnumerable()
                        select new
                        {
                            Id = r.Field<object>("Id"),
                            UserName = r.Field<object>("UserName"),
                            Email = r.Field<object>("Email"),
                            PhoneNumber = r.Field<object>("PhoneNumber"),
                            RoleId = r.Field<object>("RoleId"),
                            RoleName = r.Field<object>("RoleName"),
                            FullName = r.Field<object>("FullName"),
                            Address = r.Field<object>("Address")
                        }).ToList(),
                Tong = tong
            };
        }
        public async Task<dynamic> CheckPageByUser(string routerPath, string userId)
        {
            DataTable dt = await MsSqlHelper.ExecuteDataTableTask(ConnectionString, "sp_check_user_page",
                new string[] { "@userId", "@routerPath" },
                new object[] { userId, routerPath });
            return dt.Rows.Count > 0;
        }
    }
}