using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Data;

namespace WebEnvang.Models.Province
{
    public class ProvinceService : BaseService
    {
        public async Task<dynamic> GetList()
        {
            DataTable dt = (await MsSqlHelper.ExecuteDataSetTask(ConnectionString, "sp_province_getlist")).Tables[0];
            return (from r in dt.AsEnumerable()
                    select new
                    {
                        Id = r.Field<object>("Id"),
                        Name = r.Field<object>("Name"),
                        VietJetAirCode = r.Field<object>("VietJetAirCode"),
                        JetStarCode = r.Field<object>("JetStarCode")
                    }).ToList();
        }
        public Task Save(ProvinceDTO dto, string userId, string IP)
        {
            return MsSqlHelper.ExecuteNonQueryTask(ConnectionString, "sp_province_save",
                new string[] { "@id", "@name", "@vietjetaircode", "@jetstarcode", "@userid", "@ip" },
                new object[] { dto.Id, dto.Name, dto.VietJetAirCode, dto.JetStarCode, userId, IP });
        }

        public Task Delete(ProvinceDTO dto, string userId, string IP)
        {
            return MsSqlHelper.ExecuteNonQueryTask(ConnectionString, "sp_province_delete",
                new string[] { "@id", "@userid", "@ip" },
                new object[] { dto.Id, userId, IP });
        }
    }
}