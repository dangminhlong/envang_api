using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebEnvang.Models.PaymentMethod
{
    [Table("PaymentMethod")]
    public class PaymentMethod
    {
        public void Copy(PaymentMethod dto)
        {
            this.Name = dto.Name;
            this.Description = dto.Description;
            this.AllowResponseFromCustomer = dto.AllowResponseFromCustomer;
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool AllowResponseFromCustomer { get; set; }
    }
}