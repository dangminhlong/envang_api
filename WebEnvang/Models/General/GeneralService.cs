using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace WebEnvang.Models.General
{
    public class GeneralService : BaseService
    {
        private readonly ApplicationDbContext ctx = null;
        public GeneralService()
        {
            ctx = new ApplicationDbContext();
        }
        public async Task Save(GeneralInformation data)
        {
            var entry = ctx.GeneralInformations.FirstOrDefault();
            if (entry == null)
            {
                entry = new GeneralInformation();
                entry.CopyData(data);
                ctx.GeneralInformations.Add(entry);
            }
            else
                entry.CopyData(data);
            await ctx.SaveChangesAsync();
        }
        public async Task<int> Delete(int id)
        {
            var entry = ctx.GeneralInformations.Where(e=>e.Id == id).FirstOrDefault();
            if (entry != null)
            {
                ctx.GeneralInformations.Remove(entry);
                return await ctx.SaveChangesAsync();
            }
            else
                return 0;
        }
        public async Task<GeneralInformation> Get()
        {
            return await Task<GeneralInformation>.Run(()=> {
                return ctx.GeneralInformations.FirstOrDefault();
            });
        }
        public async Task<IList<GeneralInformation>> GetList()
        {
            return await Task<IList<GeneralInformation>>.Run(() =>
            {
                return ctx.GeneralInformations.ToList();
            });
        }
    }
}