using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebEnvang.Models.BookApi
{
    public class Passenger
    {
        public int PassengerType { get; set; }
        public string Gender { get; set; }
        public string Title { get; set; }
        public string FullName { get; set; }
        public decimal Baggage { get; set; }
        public decimal ReturnBaggage { get; set; }
    }
}