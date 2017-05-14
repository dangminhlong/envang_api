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
        private readonly ApplicationDbContext ctx = null;
        public WebConfigService()
        {
            ctx = new ApplicationDbContext();
        }
        public Task Save(WebConfig dto, string userid, string ip)
        {
            var entry = (from e in ctx.WebConfigs
                         select e).FirstOrDefault();
            if (entry == null)
            {
                entry = new WebConfig();
                entry.Copy(dto);
                ctx.WebConfigs.Add(entry);
            }
            else
            {
                entry.Copy(dto);
            }
            return ctx.SaveChangesAsync();
        }

        public async Task<WebConfig> Get()
        {
            return await (from r in ctx.WebConfigs
                          select r).FirstOrDefaultTask();
        }
    }
}