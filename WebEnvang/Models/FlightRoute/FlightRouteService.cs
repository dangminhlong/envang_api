using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace WebEnvang.Models.FlightRoute
{
    public class FlightRouteService : BaseService
    {
        public Task SaveRouteInfo(FlightRouteDTO dto, string userId, string IP)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Value", typeof(int));
            dto.DestIdList.ForEach((id) =>
            {
                var row = dt.NewRow();
                row["Value"] = id;
                dt.Rows.Add(row);
            });
            return MsSqlHelper.ExecuteNonQueryTask(ConnectionString, "sp_flightroute_save",
                new string[] { "@sourceId", "@destTable", "@userid", "@ip" },
                new object[] { dto.SourceId, dt, userId, IP });
        }
    }
}