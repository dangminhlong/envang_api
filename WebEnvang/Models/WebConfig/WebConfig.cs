using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebEnvang.Models.WebConfig
{
    [Table("WebConfig")]
    public class WebConfig
    {
        public void Copy(WebConfig data)
        {
            this.HotlineFull = data.HotlineFull;
            this.HotlineTop = data.HotlineTop;
            this.Email = data.Email;
            this.MobileNumber = data.MobileNumber;
            this.Country = data.Country;
            this.Province = data.Province;
            this.City = data.City;
            this.Address = data.Address;
            this.AdultFee = data.AdultFee;
            this.ChildFee = data.ChildFee;
            this.InfantFee = data.InfantFee;
            this.KetQuaDatVe = data.KetQuaDatVe;
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string HotlineTop { get; set; }
        public string HotlineFull { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string Country { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public decimal AdultFee { get; set; }
        public decimal ChildFee { get; set; }
        public decimal InfantFee { get; set; }
        public string KetQuaDatVe { get; set; }
    }
}