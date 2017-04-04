using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebEnvang.Models.FlightRoute
{
    public class FlightRouteDTO
    {
        public int SourceId { get; set; }
        public List<int> DestIdList { get; set; }
    }
}