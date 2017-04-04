using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Data;

namespace WebEnvang.Models.FeatureArticleConfig
{
    public class FeatureArticleConfigService : BaseService
    {
        public async Task<dynamic> GetList(FeatureArticleConfigDTO dto)
        {
            DataTable dt = await MsSqlHelper.ExecuteDataTableTask(ConnectionString, "sp_FeatureArticleConfig_getlist",
                new string[] { "@groupId" },
                new object[] { dto.GroupId });
            return (from r in dt.AsEnumerable()
                    select new
                    {
                        Id = r.Field<object>("Id"),
                        Name = r.Field<object>("Name"),
                        Order = r.Field<object>("Order"),
                        ArticleId = r.Field<object>("ArticleId"),
                        GroupId = r.Field<object>("GroupId"),
                        ArticleName = r.Field<object>("ArticleName"),
                        GroupName = r.Field<object>("GroupName"),
                        Style = r.Field<object>("Style")
                    }).ToList();
        }
        public Task Save(FeatureArticleConfigDTO dto, string userId, string IP)
        {
            return MsSqlHelper.ExecuteNonQueryTask(ConnectionString, "sp_FeatureArticleConfig_save",
                new string[] { "@id", "@name", "@articleid", "@order", "@groupid", "@style", "@userid", "@ip" },
                new object[] { dto.Id, dto.Name, dto.ArticleId, dto.Order, dto.GroupId, string.IsNullOrEmpty(dto.Style) ? "" : dto.Style, userId, IP });
        }

        public Task Delete(FeatureArticleConfigDTO dto, string userId, string IP)
        {
            return MsSqlHelper.ExecuteNonQueryTask(ConnectionString, "sp_FeatureArticleConfig_delete",
                new string[] { "@id", "@userid", "@ip" },
                new object[] { dto.Id, userId, IP });
        }
    }
}