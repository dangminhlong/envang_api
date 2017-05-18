using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace WebEnvang.Models.EnvietFlightScrape
{
    public class EvScrapeService
    {
        private decimal adultFee;
        private decimal childFee;
        private decimal infantFee;
        public EvScrapeService(decimal adultFee, decimal childFee, decimal infantFee)
        {
            this.adultFee = adultFee;
            this.childFee = childFee;
            this.infantFee = infantFee;
        }
        public async Task<SearchFlightResult> SearchFlight(string fromCity, string toCity, string departureDate, string returnDate, int adult, int child, int infant)
        {
            string genUrl = string.Format("http://booking.vemaybaytructuyen.com/Booking/CrossDomainSelectFlight?DepartureCity={0}&ArrivalCity={1}&DepartureDate={2}&ArrivalDate={3}&AdultNo={4}&ChildNo={5}&InfantNo={6}",
                fromCity, toCity, departureDate, returnDate, adult, child, infant);
            WebRequestHandler handler = new WebRequestHandler()
            {
                CookieContainer = new System.Net.CookieContainer()
            };
            HttpClient client = new HttpClient(handler);
            var genResponse = await client.GetAsync(genUrl);
            List<SearchFlightInfo> departureList = new List<SearchFlightInfo>();
            List<SearchFlightInfo> returnList = new List<SearchFlightInfo>();
            if (genResponse.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var taskJS = SearchFlightByBrand("JETSTAR", client, adult, child, infant);
                var taskVJ = SearchFlightByBrand("VIETJET", client, adult, child, infant);
                var taskVN = SearchFlightByBrand("VNA", client, adult, child, infant);
                var resultJS = await taskJS;
                var resultVJ = await taskVJ;
                var resultVN = await taskVN;
                departureList.AddRange(resultJS.DepartureList);
                departureList.AddRange(resultVJ.DepartureList);
                departureList.AddRange(resultVN.DepartureList);
                returnList.AddRange(resultJS.ReturnList);
                returnList.AddRange(resultVJ.ReturnList);
                returnList.AddRange(resultVN.ReturnList);
            }
            return new SearchFlightResult
            {
                DepartureList = departureList,
                ReturnList = returnList
            };
        }

        public async Task<SearchFlightResult> SearchFlightByBrand(string brand, HttpClient client, int adult, int child, int infant)
        {
            IList<SearchFlightInfo> departureList = new List<SearchFlightInfo>();
            IList<SearchFlightInfo> returnList = new List<SearchFlightInfo>();
            string searchFlightUrl = "http://booking.vemaybaytructuyen.com/Booking/SearchFlightsByAirline";
            var formContent = new FormUrlEncodedContent(new KeyValuePair<string, string>[]
            {
                new KeyValuePair<string, string>("airlineId", brand)
            });
            var searchResponse = await client.PostAsync(searchFlightUrl, formContent);
            if (searchResponse.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var strResult = await searchResponse.Content.ReadAsStringAsync();
                var searchResult = JsonConvert.DeserializeObject<EvSearchFlightResult>(strResult);

                string departureDatePattern = ".DepartureDate\" type=\"hidden\" value=\"(?<v>[^\"]+)\"";
                string arrivalDatePattern = ".ArrivalDate\" type=\"hidden\" value=\"(?<v>[^\"]+)\"";
                string departureTimePattern = ".DepartureTime\" type=\"hidden\" value=\"(?<v>[^\"]+)\"";
                string arrivalTimePattern = ".ArrivalTime\" type=\"hidden\" value=\"(?<v>[^\"]+)\"";
                string flightNoPattern = ".FlightNo\" type=\"hidden\" value=\"(?<v>[^\"]+)\"";
                string seatClassPattern = ".SeatClass\" type=\"hidden\" value=\"(?<v>[^\"]+)\"";
                string adultFarePattern = ".FareInfo.AdultFare\" type=\"hidden\" value=\"(?<v>[^\"]+)\"";
                string adultPricePattern = ".FareInfo.AdultPrice\" type=\"hidden\" value=\"(?<v>[^\"]+)\"";
                string adultChargePattern = ".FareInfo.AdultCharge\" type=\"hidden\" value=\"(?<v>[^\"]+)\"";
                string adultServiceFeePattern = ".FareInfo.AdultServiceFee\" type=\"hidden\" value=\"(?<v>[^\"]+)\"";
                string childFarePattern = ".FareInfo.ChildFare\" type=\"hidden\" value=\"(?<v>[^\"]+)\"";
                string childPricePattern = ".FareInfo.ChildPrice\" type=\"hidden\" value=\"(?<v>[^\"]+)\"";
                string childChargePattern = ".FareInfo.ChildCharge\" type=\"hidden\" value=\"(?<v>[^\"]+)\"";
                string childServiceFeePattern = ".FareInfo.ChildServiceFee\" type=\"hidden\" value=\"(?<v>[^\"]+)\"";
                string infantFarePattern = ".FareInfo.InfantFare\" type=\"hidden\" value=\"(?<v>[^\"]+)\"";
                string infantPricePattern = ".FareInfo.InfantPrice\" type=\"hidden\" value=\"(?<v>[^\"]+)\"";
                string infantChargePattern = ".FareInfo.InfantCharge\" type=\"hidden\" value=\"(?<v>[^\"]+)\"";
                string infantServiceFeePattern = ".FareInfo.InfantServiceFee\" type=\"hidden\" value=\"(?<v>[^\"]+)\"";

                if (searchResult.rMessage.Result)
                {
                    searchResult.result.DepartureFlightHtmls.ForEach((r) =>
                    {
                        string strNgayBay = string.Empty, strNgayDen = string.Empty, strGioBay = string.Empty, strGioDen = string.Empty, strMaChuyenBay = string.Empty, strHangGhe = string.Empty,
                            strAdultFare = string.Empty, strAdultCharge = string.Empty, strAdultPrice = string.Empty, strAdultServiceFee = string.Empty,
                            strChildFare = string.Empty, strChildCharge = string.Empty, strChildPrice = string.Empty, strChildServiceFee = string.Empty,
                            strInfantFare = string.Empty, strInfantCharge = string.Empty, strInfantPrice = string.Empty, strInfantServiceFee = string.Empty;
                        var match = Regex.Match(r.Html, departureDatePattern);
                        if (match.Success)
                        {
                            strNgayBay = match.Groups["v"].Value;
                        }
                        match = Regex.Match(r.Html, arrivalDatePattern);
                        if (match.Success)
                        {
                            strNgayDen = match.Groups["v"].Value;
                        }
                        match = Regex.Match(r.Html, departureTimePattern);
                        if (match.Success)
                        {
                            strGioBay = match.Groups["v"].Value;
                        }
                        match = Regex.Match(r.Html, arrivalTimePattern);
                        if (match.Success)
                        {
                            strGioDen = match.Groups["v"].Value;
                        }
                        match = Regex.Match(r.Html, flightNoPattern);
                        if (match.Success)
                        {
                            strMaChuyenBay = match.Groups["v"].Value;
                        }
                        match = Regex.Match(r.Html, seatClassPattern);
                        if (match.Success)
                        {
                            strHangGhe = match.Groups["v"].Value;
                        }

                        match = Regex.Match(r.Html, adultFarePattern);
                        if (match.Success)
                        {
                            strAdultFare = match.Groups["v"].Value;
                        }
                        match = Regex.Match(r.Html, adultPricePattern);
                        if (match.Success)
                        {
                            strAdultPrice = match.Groups["v"].Value;
                        }
                        match = Regex.Match(r.Html, adultChargePattern);
                        if (match.Success)
                        {
                            strAdultCharge = match.Groups["v"].Value;
                        }
                        match = Regex.Match(r.Html, adultServiceFeePattern);
                        if (match.Success)
                        {
                            strAdultServiceFee = match.Groups["v"].Value;
                        }

                        match = Regex.Match(r.Html, childFarePattern);
                        if (match.Success)
                        {
                            strChildFare = match.Groups["v"].Value;
                        }
                        match = Regex.Match(r.Html, childPricePattern);
                        if (match.Success)
                        {
                            strChildPrice = match.Groups["v"].Value;
                        }
                        match = Regex.Match(r.Html, childChargePattern);
                        if (match.Success)
                        {
                            strChildCharge = match.Groups["v"].Value;
                        }
                        match = Regex.Match(r.Html, childServiceFeePattern);
                        if (match.Success)
                        {
                            strChildServiceFee = match.Groups["v"].Value;
                        }

                        match = Regex.Match(r.Html, infantFarePattern);
                        if (match.Success)
                        {
                            strInfantFare = match.Groups["v"].Value;
                        }
                        match = Regex.Match(r.Html, infantPricePattern);
                        if (match.Success)
                        {
                            strInfantPrice = match.Groups["v"].Value;
                        }
                        match = Regex.Match(r.Html, infantChargePattern);
                        if (match.Success)
                        {
                            strInfantCharge = match.Groups["v"].Value;
                        }
                        match = Regex.Match(r.Html, infantServiceFeePattern);
                        if (match.Success)
                        {
                            strInfantServiceFee = match.Groups["v"].Value;
                        }
                        try
                        {
                            SearchFlightInfo info = new SearchFlightInfo();
                            info.Airline = brand;
                            info.DepartureDate = strNgayBay.Split(new char[] { ' ' })[0];
                            info.ArrivalDate = strNgayDen.Split(new char[] { ' ' })[0];
                            info.DepartureTime = strGioBay;
                            info.ArrivalTime = strGioDen;
                            info.FlightNo = strMaChuyenBay;
                            info.StopNo = r.StopNo < 0 ? 0 : r.StopNo;
                            info.SeatClass = strHangGhe;
                            info.AdultFare = Convert.ToDecimal(strAdultFare);
                            info.AdultCharge = Convert.ToDecimal(strAdultCharge); ;
                            info.AdultServiceFee = adultFee;
                            info.AdultPrice = info.AdultFare + info.AdultCharge + adultFee;

                            info.ChildFare = Convert.ToDecimal(strChildFare);
                            info.ChildCharge = Convert.ToDecimal(strChildCharge);
                            info.ChildServiceFee = childFee;
                            info.ChildPrice = info.ChildFare + info.ChildCharge + childFee;

                            info.InfantFare = Convert.ToDecimal(strInfantFare);
                            info.InfantCharge = Convert.ToDecimal(strInfantCharge);
                            info.InfantServiceFee = infantFee;
                            info.InfantPrice = info.InfantFare + info.InfantCharge + infantFee;

                            info.TicketFare = info.AdultFare * adult + info.ChildFare * child + info.InfantFare * infant;
                            info.TicketPrice = info.AdultPrice * adult + info.ChildPrice * child + info.InfantPrice * infant;

                            departureList.Add(info);
                        }
                        catch
                        {
                        }
                    });

                    if (searchResult.result.ReturnFlightHtmls != null)
                    {
                        searchResult.result.ReturnFlightHtmls.ForEach((r) =>
                        {
                            string strNgayBay = string.Empty, strNgayDen = string.Empty, strGioBay = string.Empty, strGioDen = string.Empty, strMaChuyenBay = string.Empty, strHangGhe = string.Empty,
                                strAdultFare = string.Empty, strAdultCharge = string.Empty, strAdultPrice = string.Empty, strAdultServiceFee = string.Empty,
                                strChildFare = string.Empty, strChildCharge = string.Empty, strChildPrice = string.Empty, strChildServiceFee = string.Empty,
                                strInfantFare = string.Empty, strInfantCharge = string.Empty, strInfantPrice = string.Empty, strInfantServiceFee = string.Empty;
                            var match = Regex.Match(r.Html, departureDatePattern);
                            if (match.Success)
                            {
                                strNgayBay = match.Groups["v"].Value;
                            }
                            match = Regex.Match(r.Html, arrivalDatePattern);
                            if (match.Success)
                            {
                                strNgayDen = match.Groups["v"].Value;
                            }
                            match = Regex.Match(r.Html, departureTimePattern);
                            if (match.Success)
                            {
                                strGioBay = match.Groups["v"].Value;
                            }
                            match = Regex.Match(r.Html, arrivalTimePattern);
                            if (match.Success)
                            {
                                strGioDen = match.Groups["v"].Value;
                            }
                            match = Regex.Match(r.Html, flightNoPattern);
                            if (match.Success)
                            {
                                strMaChuyenBay = match.Groups["v"].Value;
                            }
                            match = Regex.Match(r.Html, seatClassPattern);
                            if (match.Success)
                            {
                                strHangGhe = match.Groups["v"].Value;
                            }

                            match = Regex.Match(r.Html, adultFarePattern);
                            if (match.Success)
                            {
                                strAdultFare = match.Groups["v"].Value;
                            }
                            match = Regex.Match(r.Html, adultPricePattern);
                            if (match.Success)
                            {
                                strAdultPrice = match.Groups["v"].Value;
                            }
                            match = Regex.Match(r.Html, adultChargePattern);
                            if (match.Success)
                            {
                                strAdultCharge = match.Groups["v"].Value;
                            }
                            match = Regex.Match(r.Html, adultServiceFeePattern);
                            if (match.Success)
                            {
                                strAdultServiceFee = match.Groups["v"].Value;
                            }

                            match = Regex.Match(r.Html, childFarePattern);
                            if (match.Success)
                            {
                                strChildFare = match.Groups["v"].Value;
                            }
                            match = Regex.Match(r.Html, childPricePattern);
                            if (match.Success)
                            {
                                strChildPrice = match.Groups["v"].Value;
                            }
                            match = Regex.Match(r.Html, childChargePattern);
                            if (match.Success)
                            {
                                strChildCharge = match.Groups["v"].Value;
                            }
                            match = Regex.Match(r.Html, childServiceFeePattern);
                            if (match.Success)
                            {
                                strChildServiceFee = match.Groups["v"].Value;
                            }

                            match = Regex.Match(r.Html, infantFarePattern);
                            if (match.Success)
                            {
                                strInfantFare = match.Groups["v"].Value;
                            }
                            match = Regex.Match(r.Html, infantPricePattern);
                            if (match.Success)
                            {
                                strInfantPrice = match.Groups["v"].Value;
                            }
                            match = Regex.Match(r.Html, infantChargePattern);
                            if (match.Success)
                            {
                                strInfantCharge = match.Groups["v"].Value;
                            }
                            match = Regex.Match(r.Html, infantServiceFeePattern);
                            if (match.Success)
                            {
                                strInfantServiceFee = match.Groups["v"].Value;
                            }
                            try
                            {
                                SearchFlightInfo info = new SearchFlightInfo();
                                info.Airline = brand;
                                info.DepartureDate = strNgayBay.Split(new char[] { ' ' })[0];
                                info.ArrivalDate = strNgayDen.Split(new char[] { ' ' })[0];
                                info.DepartureTime = strGioBay;
                                info.ArrivalTime = strGioDen;
                                info.FlightNo = strMaChuyenBay;
                                info.StopNo = r.StopNo < 0 ? 0 : r.StopNo;
                                info.SeatClass = strHangGhe;
                                info.AdultFare = Convert.ToDecimal(strAdultFare);
                                info.AdultCharge = Convert.ToDecimal(strAdultCharge);
                                info.AdultServiceFee = adultFee;
                                info.AdultPrice = info.AdultFare + info.AdultCharge + adultFee;

                                info.ChildFare = Convert.ToDecimal(strChildFare);
                                info.ChildCharge = Convert.ToDecimal(strChildCharge);
                                info.ChildServiceFee = childFee;
                                info.ChildPrice = info.ChildFare + info.ChildCharge + childFee;

                                info.InfantFare = Convert.ToDecimal(strInfantFare);
                                info.InfantCharge = Convert.ToDecimal(strInfantCharge);
                                info.InfantServiceFee = infantFee;
                                info.InfantPrice = info.InfantFare + info.InfantCharge + infantFee;

                                info.TicketFare = info.AdultFare * adult + info.ChildFare * child + info.InfantFare * infant;
                                info.TicketPrice = info.AdultPrice * adult + info.ChildPrice * child + info.InfantPrice * infant;

                                returnList.Add(info);
                            }
                            catch
                            {
                            }
                        });
                    }
                }
            }
            return new SearchFlightResult
            {
                DepartureList = departureList,
                ReturnList = returnList
            };
        }
    }
}