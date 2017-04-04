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
        public async Task<dynamic> GetList(UserDTO dto)
        {
            DataTable dt = (await MsSqlHelper.ExecuteDataSetTask(ConnectionString, "sp_user_getlist", 
                new string[] { "@username", "@roleId" }, 
                new object[] { dto.UserName, dto.RoleId })).Tables[0];
            return (from r in dt.AsEnumerable()
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
                    }).ToList();
        }
    }
}