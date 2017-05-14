using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebEnvang.Models.LuggagePrice
{
    [Table("LuggagePrice")]
    public class LuggagePrice
    {
        public void Copy(LuggagePrice dto)
        {
            this.Name = dto.Name;
            this.Weight = dto.Weight;
            this.Price = dto.Price;
            this.AirlineId = dto.AirlineId;
            this.Order = dto.Order;
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public int Weight { get; set; }
        public decimal Price { get; set; }
        public int AirlineId { get; set; }
        public int Order { get; set; }
        public bool IsDeleted { get; set; }
        public string UserId { get; set; }
        public string IP { get; set; }
    }
}