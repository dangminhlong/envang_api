using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using WebEnvang.Models.WebConfig;

namespace WebEnvang.Models.BookApi
{
    public class FlightBookService : BaseService
    {
        private readonly HttpClient client;
        private readonly string username;
        private readonly string password;
        private readonly string rootUri;
        public FlightBookService(string uri, string username, string password)
        {
            client = new HttpClient();
            this.rootUri = uri;
            this.username = username;
            this.password = password;
        }

        void SetRequestAuthentication(HttpRequestMessage message)
        {
            var byteArray = Encoding.ASCII.GetBytes(string.Format("{0}:{1}", username, password));
            message.Headers.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
        }

        public async Task<dynamic> BookFlight(BookingDTO dto, string userId, string IP)
        {
            WebConfigService wcService = new WebConfigService();
            var wc = await wcService.Get();
            if (dto.RoundTrip)
            {

            }
            else
            {
                int bookId = Convert.ToInt32(await MsSqlHelper.ExecuteScalarTask(ConnectionString, "sp_book_info_gen_id"));
                
                var bookingPassengers = dto.Adult.Union(dto.Child).Union(dto.Infant).ToArray();
                foreach(var p in bookingPassengers)
                {
                    p.Gender = (p.Title == "Ông" || p.Title == "Em trai" || p.Title == "Bé trai") ? "1" : "2";
                }
                bookingPassengers[0].MobileNumber = wc.MobileNumber;
                bookingPassengers[0].Email = wc.Email;
                bookingPassengers[0].City = wc.City;
                bookingPassengers[0].Country = wc.Country;
                bookingPassengers[0].Province = wc.Province;
                bookingPassengers[0].Address = wc.Address;
                
                var bookingInfo = new
                {
                    Brand = dto.ChieuDi.Brand,
                    Adult = dto.Adult.Count,
                    Child = dto.Child.Count,
                    Infant = dto.Infant.Count,
                    RoundTrip = false,
                    FromPlaceId = dto.ChieuDi.FromPlaceId,
                    ToPlaceId = dto.ChieuDi.ToPlaceId,
                    FlightNumber = dto.ChieuDi.FlightNumber,
                    TicketPrice = dto.ChieuDi.TicketPrice,
                    TicketType = dto.ChieuDi.TicketType,
                    DepartDate = dto.DepartDate,
                    ReturnDate = dto.ReturnDate,
                    FromPlaceCode = dto.FromPlaceCode,
                    ToPlaceCode = dto.ToPlaceCode,                    
                    FareBasis = dto.ChieuDi.FareBasis,
                    CurrencyType = "VND",
                    CallBackUrl = System.Configuration.ConfigurationManager.AppSettings["publicApiUrl"] + "/api/flightApi/ProcessCallback/" + bookId + "/?brand=" + dto.ChieuDi.Brand,
                    BookingPassengers = bookingPassengers
                };
                var strJsonContent = JsonConvert.SerializeObject(bookingInfo);
                var content = new StringContent(strJsonContent, Encoding.UTF8, "application/json");
                var requestMessage = new HttpRequestMessage()
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(this.rootUri + "/oapi/airline/Bookings"),
                    Content = content
                };
                this.SetRequestAuthentication(requestMessage);
                var response = await this.client.SendAsync(requestMessage);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var apiBookResult = JsonConvert.DeserializeObject<BookResult>(result);

                    DataTable tblTicket = CreateTableTicket();
                    var rowTicket = tblTicket.NewRow();
                    rowTicket["ApiBookingId"] = apiBookResult.Id;
                    rowTicket["RoundTrip"] = dto.RoundTrip;
                    rowTicket["Adult"] = dto.Adult.Count;
                    rowTicket["Child"] = dto.Child.Count;
                    rowTicket["Infant"] = dto.Infant.Count;
                    rowTicket["Brand"] = dto.ChieuDi.Brand;
                    rowTicket["DepartTime"] = dto.ChieuDi.DepartTime;
                    rowTicket["LandingTime"] = dto.ChieuDi.LandingTime;
                    rowTicket["FlightNumber"] = dto.ChieuDi.FlightNumber;
                    rowTicket["TicketType"] = dto.ChieuDi.TicketType;
                    rowTicket["TicketPrice"] = dto.ChieuDi.TicketPrice;
                    rowTicket["TotalPrice"] = dto.ChieuDi.TotalPrice;
                    rowTicket["FareBasis"] = dto.ChieuDi.FareBasis;
                    tblTicket.Rows.Add(rowTicket);
//rowTicket["ReturnTicketType"] = 
//rowTicket["ReturnTicketPrice"]
//rowTicket["ReturnTotalPrice"]
//rowTicket["ReturnDepartTime"]
//rowTicket["ReturnLandingTime"]
//rowTicket["ReturnFlightNumber"]
//rowTicket["ReturnFareBasis"]


                    DataTable tblPassenger = CreateTablePassenger();
                    bookingPassengers.ToList().ForEach(p =>
                    {
                        var row = tblPassenger.NewRow();
                        row["FirstName"] = p.FirstName;
                        row["LastName"] = p.LastName;
                        row["Middle"] = p.MiddleName;
                        row["PassengerType"] = p.PassengerType;
                        row["Gender"] = p.Gender;
                        row["Title"] = p.Title;
                        row["PhoneNumber"] = p.MobileNumber;
                        row["Email"] = p.Email;
                        row["Address"] = p.Address;
                        row["City"] = p.City;
                        row["Province"] = p.Province;
                        row["Country"] = p.Country;
                        row["Baggage"] = p.Baggage;
                        row["ReturnBaggage"] = p.ReturnBaggage;
                        tblPassenger.Rows.Add(row);
                    });

                    MsSqlHelper.ExecuteNoneQuery(ConnectionString, "sp_save_book_info",
                        new string[] {
                            "@id",
                            "@fromplaceId",
                            "@toplaceId",
                            "@departDate",
                            "@returnDate",
                            "@roundtrip",
                            "@contactfullname",
                            "@contactphonenumber",
                            "@contactemail",
                            "@contactaddress",
                            "@totalprice",
                            "@bookby",
                            "@bookip",
                            "@couponcode",
                            "@coupondiscount",
                            "@tbl_ticket",
                            "@tbl_passenger"
                        },
                        new object[]
                        {
                            bookId,
                            dto.FromPlaceId,
                            dto.ToPlaceId,
                            dto.DepartDate,
                            dto.ReturnDate,
                            dto.RoundTrip,
                            dto.Contact.HoTen,
                            dto.Contact.DienThoai,
                            dto.Contact.Email,
                            dto.Contact.DiaChi,
                            dto.TotalPrice,
                            userId,
                            IP,
                            string.IsNullOrEmpty(dto.CouponCode) ? DBNull.Value : (object)dto.CouponCode,
                            string.IsNullOrEmpty(dto.CouponDiscount) ? DBNull.Value : (object)dto.CouponDiscount,
                            tblTicket,
                            tblPassenger
                        });

                    return JsonConvert.DeserializeObject(result);
                }
                else
                {
                    var result = await response.Content.ReadAsStringAsync();
                    throw new InvalidOperationException(result);
                }

            }
            return 0;
            
        }

        public async Task<dynamic> FindFlight(FindFlight findFlight)
        {
            //var content = new ObjectContent(typeof(FindFlight), findFlight, new JsonMediaTypeFormatter());
            var strJsonContent = JsonConvert.SerializeObject(findFlight);
            var content = new StringContent(strJsonContent, Encoding.UTF8, "application/json");
            var requestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(this.rootUri + "/oapi/airline/Flights/Find?$expand=TicketPriceDetails"),
                Content = content
            };
            this.SetRequestAuthentication(requestMessage);
            var response = await this.client.SendAsync(requestMessage);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject(result);
            }
            else
            {
                var result = await response.Content.ReadAsStringAsync();
                throw new InvalidOperationException(result);
            }
        }

        public async Task<dynamic> GetPlaces()
        {
            var requestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(this.rootUri + "/oapi/airline/Places")
            };
            this.SetRequestAuthentication(requestMessage);
            var response = await this.client.SendAsync(requestMessage);
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject(result);
        }

        public async Task<dynamic> GetCountries()
        {
            var requestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(this.rootUri + "/oapi/airline/Countries")
            };
            this.SetRequestAuthentication(requestMessage);
            var response = await this.client.SendAsync(requestMessage);
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject(result);
        }

        public async Task<dynamic> GetProvinces()
        {
            var requestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(this.rootUri + "/oapi/airline/provinces")
            };
            this.SetRequestAuthentication(requestMessage);
            var response = await this.client.SendAsync(requestMessage);
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject(result);
        }

        DataTable CreateTableTicket()
        {
            DataTable tbl = new DataTable();
            tbl.Columns.Add("ApiBookingId", typeof(int));
            tbl.Columns.Add("RoundTrip", typeof(bool));
            tbl.Columns.Add("Adult", typeof(int));
            tbl.Columns.Add("Child", typeof(int));
            tbl.Columns.Add("Infant", typeof(int));
            tbl.Columns.Add("Brand");
            tbl.Columns.Add("DepartTime", typeof(DateTime));
            tbl.Columns.Add("LandingTime", typeof(DateTime));
            tbl.Columns.Add("FlightNumber");
            tbl.Columns.Add("TicketType");
            tbl.Columns.Add("TicketPrice", typeof(decimal));
            tbl.Columns.Add("TotalPrice", typeof(decimal));
            tbl.Columns.Add("ReturnTicketType");
            tbl.Columns.Add("ReturnTicketPrice", typeof(decimal));
            tbl.Columns.Add("ReturnTotalPrice", typeof(decimal));
            tbl.Columns.Add("ReturnDepartTime", typeof(DateTime));
            tbl.Columns.Add("ReturnLandingTime", typeof(DateTime));
            tbl.Columns.Add("ReturnFlightNumber");
            tbl.Columns.Add("FareBasis");
            tbl.Columns.Add("ReturnFareBasis");

            return tbl;
        }

        DataTable CreateTablePassenger()
        {
            DataTable tbl = new DataTable();
            tbl.Columns.Add("FirstName");
            tbl.Columns.Add("LastName");
            tbl.Columns.Add("Middle");
            tbl.Columns.Add("PassengerType", typeof(int));
            tbl.Columns.Add("Gender");
            tbl.Columns.Add("Title");
            tbl.Columns.Add("PhoneNumber");
            tbl.Columns.Add("Email");
            tbl.Columns.Add("Address");
            tbl.Columns.Add("City");
            tbl.Columns.Add("Province");
            tbl.Columns.Add("Country");
            tbl.Columns.Add("Baggage", typeof(int));
            tbl.Columns.Add("ReturnBaggage", typeof(int));
            return tbl;
        }

        public static Task UpdateBookPNR(int bookId, string brand, string pnr)
        {
            return MsSqlHelper.ExecuteNonQueryTask(StaticConnectionString, "sp_book_update_pnr",
                new string[] { "@bookId", "@brand", "@pnr" },
                new object[] { bookId, brand, pnr });
        }
    }
}