using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebEnvang.Models.FlightRoute
{
    [Table("FlightRoute")]
    public class FlightRoute
    {
        [Key,Column(Order = 0)]
        public int SrcLocationId { get; set; }
        [Key, Column(Order = 1)]
        public int DestLocationId { get; set; }
    }
}