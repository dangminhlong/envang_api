using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace WebEnvang.Models.BookApi
{
    public class ApiConfigService : BaseService
    {
        public async Task<ApiConfig> Get()
        {
            DataTable dt = (await MsSqlHelper.ExecuteDataSetTask(ConnectionString, "sp_api_config_get")).Tables[0];
            return new ApiConfig
            {
                Url = dt.Rows[0]["ApiUrl"].ToString(),
                Username = dt.Rows[0]["ApiUsername"].ToString(),
                Password = dt.Rows[0]["ApiPassword"].ToString()
            };
        }
    }
}