using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace WebEnvang.Models.WebConfig
{
    public class WebConfigService : BaseService
    {
        public Task Save(WebConfigDTO dto, string userid, string ip)
        {
            return MsSqlHelper.ExecuteNonQueryTask(ConnectionString, "sp_webconfig_save",
                new string[] { "@hotlinetop", "@hotlinefull", "@userid", "@ip",
                    "@email", "@mobilenumber", "@country", "@province", "@city", "@address"},
                new object[] { dto.HotlineTop, dto.HotlineFull, userid, ip,
                    dto.Email, dto.MobileNumber, dto.Country, dto.Province, dto.City, dto.Address});
        }

        public async Task<WebConfigDTO> Get()
        {
            DataTable dt = await MsSqlHelper.ExecuteDataTableTask(ConnectionString, "sp_webconfig_get");
            var item = (from r in dt.AsEnumerable()
                    select new WebConfigDTO
                    {
                        HotlineTop = r.Field<object>("HotlineTop").ToString(),
                        HotlineFull = r.Field<object>("HotlineFull").ToString(),
                        Email = r.Field<object>("Email").ToString(),
                        MobileNumber = r.Field<object>("MobileNumber").ToString(),
                        Country = r.Field<object>("Country").ToString(),
                        Province = r.Field<object>("Province").ToString(),
                        City = r.Field<object>("City").ToString(),
                        Address = r.Field<object>("Address").ToString()
                    }).FirstOrDefault();
            return item;
        }
    }
}