using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebEnvang.Models.LuggagePrice
{
    public class LuggagePriceDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Weight { get; set; }
        public decimal Price { get; set; }
        public int AirlineId { get; set; }
        public int Order { get; set; }
        public string AirlineCode { get; set; }
    }
}