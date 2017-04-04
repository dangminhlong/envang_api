using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Data;

namespace WebEnvang.Models.LuggagePrice
{
    public class LuggagePriceService : BaseService
    {
        public async Task<dynamic> GetList(LuggagePriceDTO dto)
        {
            DataTable dt = (await MsSqlHelper.ExecuteDataSetTask(ConnectionString, "sp_luggageprice_getlist",
                new string[] { "@airlineId" },
                new object[] { dto.AirlineId })).Tables[0];
            return (from r in dt.AsEnumerable()
                    select new
                    {
                        Id = r.Field<object>("Id"),
                        Name = r.Field<object>("Name"),
                        Weight = r.Field<object>("Weight"),
                        Price = r.Field<object>("Price"),
                        AirlineId = r.Field<object>("AirlineId"),
                        Order = r.Field<object>("Order")
                    }).ToList();
        }
        public async Task<dynamic> GetListByAirlineCode(LuggagePriceDTO dto)
        {
            DataTable dt = (await MsSqlHelper.ExecuteDataSetTask(ConnectionString, "sp_luggageprice_getlist_by_airline_code",
                new string[] { "@airlineCode" },
                new object[] { dto.AirlineCode })).Tables[0];
            return (from r in dt.AsEnumerable()
                    select new
                    {
                        Id = r.Field<object>("Id"),
                        Name = r.Field<object>("Name"),
                        Weight = r.Field<object>("Weight"),
                        Price = r.Field<object>("Price"),
                        AirlineId = r.Field<object>("AirlineId"),
                        Order = r.Field<object>("Order")
                    }).ToList();
        }
        public Task Save(LuggagePriceDTO dto, string userId, string IP)
        {
            return MsSqlHelper.ExecuteNonQueryTask(ConnectionString, "sp_luggageprice_save",
                new string[] { "@id", "@name", "@weight", "@price", "@airlineid", "@order", "@userid", "@ip" },
                new object[] { dto.Id, dto.Name, dto.Weight, dto.Price, dto.AirlineId, dto.Order, userId, IP });
        }

        public Task Delete(LuggagePriceDTO dto, string userId, string IP)
        {
            return MsSqlHelper.ExecuteNonQueryTask(ConnectionString, "sp_luggageprice_delete",
                new string[] { "@id", "@userid", "@ip" },
                new object[] { dto.Id, userId, IP });
        }
    }
}