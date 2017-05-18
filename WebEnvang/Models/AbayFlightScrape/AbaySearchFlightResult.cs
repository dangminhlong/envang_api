using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebEnvang.Models.AbayFlightScrape
{
    public class AbaySearchFlightResult
    {
        public List<SearchItemResultInfo> DepartureList { get; set; }
        public List<SearchItemResultInfo> ReturnList { get; set; }
    }
}