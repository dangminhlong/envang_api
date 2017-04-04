using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Data;

namespace WebEnvang.Models.FeatureArticleConfigGroup
{
    public class FeatureArticleConfigGroupService : BaseService
    {
        public async Task<dynamic> GetList()
        {
            DataTable dt = (await MsSqlHelper.ExecuteDataSetTask(ConnectionString, "sp_featurearticleconfiggroup_getlist")).Tables[0];
            return (from r in dt.AsEnumerable()
                    select new
                    {
                        Id = r.Field<object>("Id"),
                        Name = r.Field<object>("Name"),
                        FriendlyName = r.Field<object>("FriendlyName"),
                        Order = r.Field<object>("Order")
                    }).ToList();
        }
        public Task Save(FeatureArticleConfigGroupDTO dto, string userId, string IP)
        {
            return MsSqlHelper.ExecuteNonQueryTask(ConnectionString, "sp_featurearticleconfiggroup_save",
                new string[] { "@id", "@name", "@friendlyname", "@order", "@userid", "@ip" },
                new object[] { dto.Id, dto.Name, dto.FriendlyName, dto.Order, userId, IP });
        }

        public Task Delete(FeatureArticleConfigGroupDTO dto, string userId, string IP)
        {
            return MsSqlHelper.ExecuteNonQueryTask(ConnectionString, "sp_featurearticleconfiggroup_delete",
                new string[] { "@id", "@userid", "@ip" },
                new object[] { dto.Id, userId, IP });
        }
    }
}