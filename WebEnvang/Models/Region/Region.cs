using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebEnvang.Models.Region
{
    [Table("Region")]
    public class Region
    {
        public void Copy(Region data)
        {
            this.Name = data.Name;
            this.IsDomestic = data.IsDomestic;
            this.Column = data.Column;
            this.Order = data.Order;
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsDomestic { get; set; }
        public int Column { get; set; }
        public int Order { get; set; }
    }
}