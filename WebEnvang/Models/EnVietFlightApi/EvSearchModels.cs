using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebEnvang.Models.EnVietFlightApi
{
    public class EvResultMessage
    {
        public bool Result { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class EvFlightHtml
    {
        public int AdultFare { get; set; }
        public string FlightAirline { get; set; }
        public int IsCheapAirline { get; set; }
        public int StopNo { get; set; }
        public int DepartureDuration { get; set; }
        public int ReturnDuration { get; set; }
        public string DepartureTime { get; set; }
        public string ArrivalTime { get; set; }
        public string ReturnDepartureTime { get; set; }
        public string ReturnArrivalTime { get; set; }
        public string Html { get; set; }
    }
    public class EvFlightResult
    {
        public List<EvFlightHtml> DepartureFlightHtmls { get; set; }
        public List<EvFlightHtml> ReturnFlightHtmls { get; set; }
    }

    public class EvSearchFlightResult
    {
        public EvResultMessage rMessage { get; set; }
        public EvFlightResult result { get; set; }
    }
}