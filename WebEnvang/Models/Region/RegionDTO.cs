using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebEnvang.Models.Region
{
    public class RegionDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsDomestic { get; set; }
        public int Column { get; set; }
        public int Order { get; set; }
    }
}