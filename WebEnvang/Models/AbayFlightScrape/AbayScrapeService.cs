using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace WebEnvang.Models.AbayFlightScrape
{
    public class AbayScrapeService
    {
        private decimal adultFee;
        private decimal childFee;
        private decimal infantFee;
        private IList<Airline.Airline> listAirline;
        public AbayScrapeService(decimal adultFee, decimal childFee, decimal infantFee, IList<Airline.Airline> listAirline)
        {
            this.adultFee = adultFee;
            this.childFee = childFee;
            this.infantFee = infantFee;
            this.listAirline = listAirline;
        }
        public async Task<AbaySearchFlightResult> SearchFlight(bool roundTrip, string fromCity, string toCity, string fromDate, string toDate, int adult, int child, int infant)
        {
            string requestIdUrl = "https://www.abay.vn/intro/?";
            string genUrl = "https://www.abay.vn/Abay/";
            WebRequestHandler handler = new WebRequestHandler()
            {
                CookieContainer = new System.Net.CookieContainer()
            };
            string certPath = HttpContext.Current.Server.MapPath("~/SSL/abay.cer");
            X509Certificate cert = X509Certificate.CreateFromCertFile(certPath);
            handler.ClientCertificates.Add(cert);
            HttpClient client = new HttpClient(handler);            
            client.DefaultRequestHeaders.Host = "www.abay.vn";
            //client.DefaultRequestHeaders.Add("Connection", "keep-alive");
            //client.DefaultRequestHeaders.Add("Cache-Control", "max-age=0");            
            client.DefaultRequestHeaders.Add("Upgrade-Insecure-Requests", "1");
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36");
            //client.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
            //client.DefaultRequestHeaders.Add("Referer", "https://www.abay.vn/Abay/");
            //client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, sdch, br");
            //client.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.8");

            //send first requeset
            var genResponse = await client.GetAsync(genUrl);
            var genResult = await genResponse.Content.ReadAsStringAsync();

            //wait for 220 ms
            await Task.Delay(220);

            //get requestId to create cookie
            var requestIdResponse = await client.GetAsync(requestIdUrl);
            string requestId = await requestIdResponse.Content.ReadAsStringAsync();
            client.DefaultRequestHeaders.Add("Cookie", "RequestId=" + requestId);

            //resend request with cookie
            genResponse = await client.GetAsync(genUrl);
            genResult = await genResponse.Content.ReadAsStringAsync();

            string vsgPattern = "<input type=\"hidden\" name=\"__VIEWSTATEGENERATOR\" id=\"__VIEWSTATEGENERATOR\" value=\"(?<v>[^\"]+)\" />";
            string vsPattern = "<input type=\"hidden\" name=\"__VIEWSTATE\" id=\"__VIEWSTATE\" value=\"(?<v>[^\"]+)\" />";
            string vsg = string.Empty, vs = string.Empty;
            var match = Regex.Match(genResult, vsgPattern);
            if (match.Success)
            {
                vsg = match.Groups["v"].Value;
            }
            match = Regex.Match(genResult, vsPattern);
            if (match.Success)
            {
                vs = match.Groups["v"].Value;
            }
            var formContentOneWay = new FormUrlEncodedContent(new KeyValuePair<string, string>[]{
                new KeyValuePair<string, string>("ctl00$ContentPlaceHolderMainColumn$UsrSearchForm1$flightwaytype","radioOneWay"),
                new KeyValuePair<string, string>("ctl00$ContentPlaceHolderMainColumn$UsrSearchForm1$txtFrom",string.Format("{0} ({0})", fromCity)),
                new KeyValuePair<string, string>("tl00$ContentPlaceHolderMainColumn$UsrSearchForm1$txtTo",string.Format("{0} ({0})", toCity)),
                new KeyValuePair<string, string>("ctl00$ContentPlaceHolderMainColumn$UsrSearchForm1$txtDepDate",fromDate),
                new KeyValuePair<string, string>("ctl00$ContentPlaceHolderMainColumn$UsrSearchForm1$cboAdult",adult.ToString()),
                new KeyValuePair<string, string>("ctl00$ContentPlaceHolderMainColumn$UsrSearchForm1$cboChild",child.ToString()),
                new KeyValuePair<string, string>("ctl00$ContentPlaceHolderMainColumn$UsrSearchForm1$cboInfant",infant.ToString()),
                new KeyValuePair<string, string>("ctl00$ContentPlaceHolderMainColumn$UsrSearchForm1$btnSearch","Đang xử lý"),
                new KeyValuePair<string, string>("__VIEWSTATE", vs),
                new KeyValuePair<string, string>("__VIEWSTATEGENERATOR", vsg)
            });

            var formContentRountrip = new FormUrlEncodedContent(new KeyValuePair<string, string>[]{
                new KeyValuePair<string, string>("ctl00$ContentPlaceHolderMainColumn$UsrSearchForm1$flightwaytype","radioRoundTrip"),
                 new KeyValuePair<string, string>("ctl00$ContentPlaceHolderMainColumn$UsrSearchForm1$txtFrom",string.Format("{0} ({0})", fromCity)),
                new KeyValuePair<string, string>("tl00$ContentPlaceHolderMainColumn$UsrSearchForm1$txtTo",string.Format("{0} ({0})", toCity)),
                new KeyValuePair<string, string>("ctl00$ContentPlaceHolderMainColumn$UsrSearchForm1$txtDepDate",fromDate),
                new KeyValuePair<string, string>("ctl00$ContentPlaceHolderMainColumn$UsrSearchForm1$txtRetDate", toDate),
                new KeyValuePair<string, string>("ctl00$ContentPlaceHolderMainColumn$UsrSearchForm1$cboAdult",adult.ToString()),
                new KeyValuePair<string, string>("ctl00$ContentPlaceHolderMainColumn$UsrSearchForm1$cboChild",child.ToString()),
                new KeyValuePair<string, string>("ctl00$ContentPlaceHolderMainColumn$UsrSearchForm1$cboInfant",infant.ToString()),
                new KeyValuePair<string, string>("ctl00$ContentPlaceHolderMainColumn$UsrSearchForm1$btnSearch","Đang xử lý"),
                new KeyValuePair<string, string>("__VIEWSTATE", vs),
                new KeyValuePair<string, string>("__VIEWSTATEGENERATOR", vsg)
            });

            var formContent = formContentOneWay;
            if (roundTrip)
                formContent = formContentRountrip;

            var searchResponse = await client.PostAsync(genUrl, formContent);
            var searchResult = await searchResponse.Content.ReadAsStringAsync();

            var strReturnUri = searchResponse.RequestMessage.RequestUri.AbsoluteUri;
            var returnUriPattern = "sessionId=(?<v>.+)";
            match = Regex.Match(strReturnUri, returnUriPattern);
            string sessionId = string.Empty;
            if (match.Success)
            {
                sessionId = match.Groups["v"].Value;
            }
            client.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");
            client.DefaultRequestHeaders.Add("Referer", strReturnUri);
            var linkSearchResult = await SearchLinks("VN,BL,VJ", sessionId, client);
            
            string[] airlineList1 = (from inf in linkSearchResult
                                     select inf.AirlineCode).Distinct().ToArray();

            //CHECK JETSTAR
            if (!airlineList1.Contains("BL"))
            {
                await Task.Delay(2000);
                var linkSearchResult2 = await SearchLinks("BL,VJ", sessionId, client);
                linkSearchResult2.ForEach(r =>
                {
                    var test = linkSearchResult.Where(x => x.Link == r.Link).FirstOrDefault();
                    if (test == null)
                        linkSearchResult.Add(r);
                });
            }

            //CHECK VIETJET
            if (!airlineList1.Contains("VJ"))
            {
                await Task.Delay(2000);
                var linkSearchResult2 = await SearchLinks("VJ,BL", sessionId, client);
                linkSearchResult2.ForEach(r =>
                {
                    var test = linkSearchResult.Where(x => x.Link == r.Link).FirstOrDefault();
                    if (test == null)
                        linkSearchResult.Add(r);
                });
            }

            var result1 = await SearchDetails(linkSearchResult, client);
            return result1;
        }

        public async Task<List<AbaySearchLink>> SearchLinks(string airline, string sessionId, HttpClient client)
        {
            string searchAirlineUri = string.Format("https://www.abay.vn/Abay/Domestic/AjaxGetResultDomestic.aspx?sessionId={0}&airlines={1}", sessionId, airline);
            var searchAirlineResponse = await client.GetAsync(searchAirlineUri);
            var searchAirlineResult = await searchAirlineResponse.Content.ReadAsStringAsync();
            searchAirlineResult = Regex.Replace(searchAirlineResult, "[\n\r\t]+", "");
            searchAirlineResult = Regex.Replace(searchAirlineResult, "[ ]+", " ");
            searchAirlineResult = Regex.Replace(searchAirlineResult, "\"", "\'");
            string searchDetailPattern = "<td class='begin'><img class='airlogo' alt='(?<hang>[A-Z]+)' /></td> <td>([^<]+)</td> <td class='f-time'>([^<]+)</td> <td style='display:none'>([^<]+)</td> <td class='f-price'>([^<]+)</td> <td class='v-detail' style='padding-left:30px;display:'><a giatri='(?<link>[^']+)' class='linkViewFlightDetail'>";
            Match match = Regex.Match(searchAirlineResult, searchDetailPattern);
            List<AbaySearchLink> list = new List<AbaySearchLink>();
            while (match.Success)
            {
                string hang = match.Groups["hang"].Value;
                string link = match.Groups["link"].Value;
                list.Add(new AbaySearchLink
                {
                    AirlineCode = hang,
                    Link = link
                });
                match = match.NextMatch();
            }
            return list;
        }

        public async Task<AbaySearchFlightResult> SearchDetails(List<AbaySearchLink> list, HttpClient client)
        {
            List<SearchItemResultInfo> departureList = new List<SearchItemResultInfo>();
            List<SearchItemResultInfo> returnList = new List<SearchItemResultInfo>();
            
            foreach (var linkItem in list)
            {
                string fullLink = "https://www.abay.vn" + linkItem.Link;
                var searchDetailResponse = await client.GetAsync(fullLink);
                var searchDetailResult = await searchDetailResponse.Content.ReadAsStringAsync();
                searchDetailResult = Regex.Replace(searchDetailResult, "[\n\r\t]+", "");
                searchDetailResult = Regex.Replace(searchDetailResult, "[ ]+", " ");
                searchDetailResult = Regex.Replace(searchDetailResult, "\"", "\'");

                SearchItemResultInfo info = new SearchItemResultInfo();

                string pattern = "<tbody class='view-detail-flight'> <tr> <td valign='top'> <p> Từ <b>(?<tutinh>[^<]+)</b>, (?<tunuoc>[^<]+)</p> <p> Sân bay : <b>(?<tusanbay>[^<]+)</b>\\((?<tucode>[A-Z]+)\\)<br> </p> <p> <!----> <b>(?<tugio>[^<]+)</b>, (?<tungay>[^<]+)</p> </td> <td valign='top'> <p> tới <b>(?<dentinh>[^<]+)</b>, (?<dennuoc>[^<]+)</p> <p> Sân Bay : <b>(?<densanbay>[^<]+)</b> \\((?<dencode>[A-Z]+)\\) </p> <p> <!----> <b>(?<dengio>[^<]+)</b>, (?<denngay>[^<]+)</p> </td> <td style=''> <table width='100%' cellpadding='0' cellspacing='0'> <!-- <tr> <td colspan='2' style=' text-align:center;'> Tổng thời gian : <b>\\{Duration\\}</b> </td> </tr>--> <tr> <td style='text-align: right;'> <img align='absmiddle' src='(?<hinh>[^']+)'> </td> <td style='line-height: 18px; padding: 0;'> (?<tenhang>[^<]+)<br/>\\(<b>(?<chuyenbay>[^<]+)</b>\\) <br/> <span style='display: \\{TicketClassDisplay\\}'> Loại vé : <b>(?<loaive>[^<]+)</b> </span> </td> </tr>";
                Match match1 = Regex.Match(searchDetailResult, pattern);
                Airline.Airline hang = null;
                if (match1.Success)
                {
                    string tutinh = match1.Groups["tutinh"].Value;
                    string tunuoc = match1.Groups["tunuoc"].Value;
                    string tusanbay = match1.Groups["tusanbay"].Value;
                    string tucode = match1.Groups["tucode"].Value;
                    string tugio = match1.Groups["tugio"].Value;
                    string tungay = match1.Groups["tungay"].Value;

                    string dentinh = match1.Groups["dentinh"].Value;
                    string dennuoc = match1.Groups["dennuoc"].Value;
                    string densanbay = match1.Groups["densanbay"].Value;
                    string dencode = match1.Groups["dencode"].Value;
                    string dengio = match1.Groups["dengio"].Value;
                    string denngay = match1.Groups["denngay"].Value;

                    string tenhang = match1.Groups["tenhang"].Value;
                    string chuyenbay = match1.Groups["chuyenbay"].Value;
                    string loaive = match1.Groups["loaive"].Value;
                    hang = this.listAirline.Where(x => x.Name == tenhang.Trim()).FirstOrDefault();
                    if (hang == null) continue;
                    info.AirlineName = tenhang.Trim();
                    info.TicketType = loaive.Trim();
                    info.FlightNo = chuyenbay.Trim();
                    info.FromCity = tutinh.Trim();
                    info.FromCountry = tunuoc.Trim();
                    info.FromAirPort = tusanbay.Trim();
                    info.FromCityCode = tucode.Trim();
                    info.ToCity = dentinh.Trim();
                    info.ToCountry = dennuoc.Trim();
                    info.ToAirPort = densanbay.Trim();
                    info.ToCityCode = dencode.Trim();
                    info.DepartureDate = tungay;
                    info.DepartureTime = tugio.Replace(":", "");
                    info.ArrivalDate = denngay;
                    info.ArrivalTime = dengio.Replace(":", "");
                }

                string patternNL = "<tr> <td align='center' class='pax'> Người lớn </td> <td align='center' class='pax'> (?<sl>\\d+) </td> <td align='center' class='pax pb-price'> (?<giafare>[,0-9]+) VNĐ </td> <td align='center' class='pax pb-price'> (?<phi>[,0-9]+) VNĐ </td> <td align='center' class='pax pb-price' style='font-size: 14px;color:#FB7201; (display:none;)?'> (?<giam>[,0-9]+) VNĐ </td> <td align='center' class='pax pb-price'> (?<tonggia>[,0-9]+) VNĐ </td></tr>";
                string patternTE = "<tr> <td align='center' class='pax'> Trẻ em </td> <td align='center' class='pax'> (?<sl>\\d+) </td> <td align='center' class='pax pb-price'> (?<giafare>[,0-9]+) VNĐ </td> <td align='center' class='pax pb-price'> (?<phi>[,0-9]+) VNĐ </td> <td align='center' class='pax pb-price' style='font-size: 14px;color:#FB7201; (display:none;)?'> (?<giam>[,0-9]+) VNĐ </td> <td align='center' class='pax pb-price'> (?<tonggia>[,0-9]+) VNĐ </td></tr>";
                string patternEB = "<tr> <td align='center' class='pax'> Trẻ sơ sinh </td> <td align='center' class='pax'> (?<sl>\\d+) </td> <td align='center' class='pax pb-price'> (?<giafare>[,0-9]+) VNĐ </td> <td align='center' class='pax pb-price'> (?<phi>[,0-9]+) VNĐ </td> <td align='center' class='pax pb-price' style='font-size: 14px;color:#FB7201; (display:none;)?'> (?<giam>[,0-9]+) VNĐ </td> <td align='center' class='pax pb-price'> (?<tonggia>[,0-9]+) VNĐ </td></tr>";

                Match matchNL = Regex.Match(searchDetailResult, patternNL);
                if (matchNL.Success)
                {
                    string sl = matchNL.Groups["sl"].Value;
                    string giafare = matchNL.Groups["giafare"].Value;
                    string phi = matchNL.Groups["phi"].Value;
                    string giam = matchNL.Groups["giam"].Value;
                    string tonggia = matchNL.Groups["tonggia"].Value;

                    info.Adult = Convert.ToInt32(sl);
                    info.AdultFare = Convert.ToDecimal(Regex.Replace(giafare, "[^0-9]+", ""));
                    info.AdultCharge = info.AdultFare * 10/100 + hang.PhiHang + adultFee;
                    info.AdultPrice = info.Adult * (info.AdultFare + info.AdultCharge);

                    info.TicketFare = info.Adult * info.AdultFare;
                    info.TicketPrice = info.AdultPrice;
                }

                Match matchTE = Regex.Match(searchDetailResult, patternTE);
                if (matchTE.Success)
                {
                    string sl = matchTE.Groups["sl"].Value;
                    string giafare = matchTE.Groups["giafare"].Value;
                    string phi = matchTE.Groups["phi"].Value;
                    string giam = matchTE.Groups["giam"].Value;
                    string tonggia = matchTE.Groups["tonggia"].Value;

                    info.Child = Convert.ToInt32(sl);
                    info.ChildFare = Convert.ToDecimal(Regex.Replace(giafare, "[^0-9]+", ""));
                    info.ChildCharge = info.ChildFare * 10 / 100 + hang.PhiHangTreEm + childFee;
                    info.ChildPrice = info.Child * (info.ChildFare + info.ChildCharge);

                    info.TicketFare += info.Child * info.ChildFare;
                    info.TicketPrice += info.ChildPrice;
                }

                Match matchEB = Regex.Match(searchDetailResult, patternEB);
                if (matchEB.Success)
                {
                    string sl = matchEB.Groups["sl"].Value;
                    string giafare = matchEB.Groups["giafare"].Value;
                    string phi = matchEB.Groups["phi"].Value;
                    string giam = matchEB.Groups["giam"].Value;
                    string tonggia = matchEB.Groups["tonggia"].Value;

                    info.Infant = Convert.ToInt32(sl);
                    info.InfantFare = Convert.ToDecimal(Regex.Replace(giafare, "[^0-9]+", ""));
                    info.InfantCharge = info.InfantFare * 10 / 100 + hang.PhiHangEmBe + infantFee;
                    info.InfantPrice = info.Infant * (info.InfantFare + info.InfantCharge);

                    info.TicketFare += info.Infant * info.InfantFare;
                    info.TicketPrice += info.Infant * info.InfantPrice;
                }
                if (fullLink.Contains("WayType=0"))
                    departureList.Add(info);
                else
                    returnList.Add(info);
            }
            return new AbaySearchFlightResult
            {
                DepartureList = departureList,
                ReturnList = returnList
            };
        }
    }
}