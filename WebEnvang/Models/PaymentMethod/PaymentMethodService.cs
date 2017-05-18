using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace WebEnvang.Models.PaymentMethod
{
    public class PaymentMethodService
    {
        private readonly ApplicationDbContext ctx = null;
        public PaymentMethodService()
        {
            ctx = new ApplicationDbContext();
        }
        public async Task<dynamic> GetList()
        {
            return await (from e in ctx.PaymentMethods
                          select e).ToListTask();
        }
        public async Task Save(PaymentMethod dto)
        {
            var entry = await (from e in ctx.PaymentMethods
                               where e.Id == dto.Id
                               select e).FirstOrDefaultTask();
            if (entry == null)
            {
                entry = new PaymentMethod();
                ctx.PaymentMethods.Add(entry);
            }
            entry.Copy(dto);
            await ctx.SaveChangesAsync();
        }
        public async Task Delete(PaymentMethod dto)
        {
            var entry = await (from e in ctx.PaymentMethods
                               where e.Id == dto.Id
                               select e).FirstOrDefaultTask();
            if (entry != null)
            {
                ctx.PaymentMethods.Remove(entry);
                await ctx.SaveChangesAsync();
            }
        }

    }
}