using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebEnvang.Models.Location
{
    [Table("Location")]
    public class Location
    {
        public void Copy(LocationDTO dto)
        {
            this.Name = dto.Name;
            this.Code = dto.Code;
            this.RegionId = dto.RegionId;
            this.Order = dto.Order;
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int RegionId { get; set; }
        public int Order { get; set; }
    }
}