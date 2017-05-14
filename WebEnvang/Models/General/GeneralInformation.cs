using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebEnvang.Models.General
{
    [Table("GeneralInformation")]
    public class GeneralInformation
    {
        public void CopyData(GeneralInformation info)
        {
            this.Slogan = info.Slogan;
            this.Logo = info.Logo;
            this.BackgroundImage = info.BackgroundImage;
            this.Email = info.Email;
            this.Hotline = info.Hotline;
            this.Address = info.Address;
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Slogan { get; set; }
        public string Logo { get; set; }
        public string BackgroundImage { get; set; }
        public string Email { get; set; }
        public string Hotline { get; set; }
        public string Address { get; set; }
    }
}