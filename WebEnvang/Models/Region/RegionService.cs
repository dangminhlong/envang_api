using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Data;

namespace WebEnvang.Models.Region
{
    public class RegionService : BaseService
    {
        public async Task<dynamic> GetList()
        {
            DataTable dt = (await MsSqlHelper.ExecuteDataSetTask(ConnectionString, "sp_region_getlist")).Tables[0];
            return (from r in dt.AsEnumerable()
                    select new
                    {
                        Id = r.Field<object>("Id"),
                        Name = r.Field<object>("Name"),
                        IsDomestic = r.Field<object>("IsDomestic"),
                        Column = r.Field<object>("Column"),
                        Order = r.Field<object>("Order")
                    }).ToList();
        }
        public Task Save(RegionDTO dto, string userId, string IP)
        {
            return MsSqlHelper.ExecuteNonQueryTask(ConnectionString, "sp_region_save",
                new string[] { "@id", "@name", "@isdomestic", "@column", "@order", "@userid", "@ip" },
                new object[] { dto.Id, dto.Name, dto.IsDomestic, dto.Column, dto.Order, userId, IP });
        }

        public Task Delete(RegionDTO dto, string userId, string IP)
        {
            return MsSqlHelper.ExecuteNonQueryTask(ConnectionString, "sp_region_delete",
                new string[] { "@id", "@userid", "@ip" },
                new object[] { dto.Id, userId, IP });
        }
    }
}