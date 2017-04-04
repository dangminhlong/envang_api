using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Data;

namespace WebEnvang.Models.Airline
{
    public class AirlineService : BaseService
    {
        public async Task<dynamic> GetList()
        {
            DataTable dt = (await MsSqlHelper.ExecuteDataSetTask(ConnectionString, "sp_airline_getlist")).Tables[0];
            return (from r in dt.AsEnumerable()
                    select new
                    {
                        Id = r.Field<object>("Id"),
                        Name = r.Field<object>("Name"),
                        Code = r.Field<object>("Code")
                    }).ToList();
        }
        public Task Save(AirlineDTO dto, string userId, string IP)
        {
            return MsSqlHelper.ExecuteNonQueryTask(ConnectionString, "sp_airline_save",
                new string[] { "@id", "@name", "code", "@userid", "@ip" },
                new object[] { dto.Id, dto.Name, dto.Code, userId, IP });
        }

        public Task Delete(AirlineDTO dto, string userId, string IP)
        {
            return MsSqlHelper.ExecuteNonQueryTask(ConnectionString, "sp_airline_delete",
                new string[] { "@id", "@userid", "@ip" },
                new object[] { dto.Id, userId, IP });
        }
    }
}