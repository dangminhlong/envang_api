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
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public int Baggage { get; set; }
        public int ReturnBaggage { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Address { get; set; }
        public string Province { get; set; }
        public string Nationality { get; set; }
        public string PassportNumber { get; set; }
        public string PassportExpired { get; set; }
    }
}