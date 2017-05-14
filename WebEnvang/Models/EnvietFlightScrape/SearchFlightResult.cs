using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebEnvang.Models.EnvietFlightScrape
{
    public class SearchFlightResult
    {
        public IList<SearchFlightInfo> DepartureList { get; set; }
        public IList<SearchFlightInfo> ReturnList { get; set; }
    }
}