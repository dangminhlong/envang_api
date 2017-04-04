using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebEnvang.Models.BookApi
{
    public class FindFlight
    {
        public bool RoundTrip { get; set; }

        public string FromPlace { get; set; }

        public string ToPlace { get; set; }

        public string DepartDate { get; set; }

        public string ReturnDate { get; set; }

        public string CurrencyType { get; set; }

        public string FlightType { get; set; }

        public int Adult { get; set; }

        public int Child { get; set; }

        public int Infant { get; set; }

        public string Sources { get; set; }
    }
}