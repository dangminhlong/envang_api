using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Data;

namespace WebEnvang.Models.ArticleTypes
{
    public class ArticleTypeService : BaseService
    {
        public async Task<dynamic> GetList()
        {
            DataTable dt = await MsSqlHelper.ExecuteDataTableTask(ConnectionString, "sp_articletype_getlist");
            return (from r in dt.AsEnumerable()
                    select new
                    {
                        Id = r.Field<object>("Id"),
                        Name = r.Field<object>("Name")
                    }).ToList();
        }
        public Task Save(ArticleTypeDTO dto, string userId, string IP)
        {
            return MsSqlHelper.ExecuteNonQueryTask(ConnectionString, "sp_articletype_save",
                new string[] { "@id", "@name", "@userid", "@ip" },
                new object[] { dto.Id, dto.Name, userId, IP });
        }

        public Task Delete(ArticleTypeDTO dto, string userId, string IP)
        {
            return MsSqlHelper.ExecuteNonQueryTask(ConnectionString, "sp_articletype_delete",
                new string[] { "@id", "@userid", "@ip" },
                new object[] { dto.Id, userId, IP });
        }
    }
}