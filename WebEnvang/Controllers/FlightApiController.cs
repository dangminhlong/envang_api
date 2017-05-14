using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WebEnvang.Models.BookApi;
using WebEnvang.Models.EnvietFlightScrape;

namespace WebEnvang.Controllers
{
    public class FlightApiController : ApiController
    {
        public async Task<dynamic> GetCountries()
        {
            ApiConfigService configService = new ApiConfigService();
            var config = await configService.Get();
            FlightBookService apiClient= new FlightBookService(config.Url, config.Username, config.Password);
            return await apiClient.GetCountries();
        }

        public async Task<dynamic> GetPlaces()
        {
            ApiConfigService configService = new ApiConfigService();
            var config = await configService.Get();
            FlightBookService apiClient = new FlightBookService(config.Url, config.Username, config.Password);
            return await apiClient.GetPlaces();
        }

        public async Task<dynamic> GetProvinces()
        {
            ApiConfigService configService = new ApiConfigService();
            var config = await configService.Get();
            FlightBookService apiClient = new FlightBookService(config.Url, config.Username, config.Password);
            return await apiClient.GetProvinces();
        }

        [HttpPost]
        public async Task<dynamic> FindFlightVJ([FromBody]FindFlight dto)
        {
            try
            {
                ApiConfigService configService = new ApiConfigService();
                var config = await configService.Get();
                FlightBookService apiClient = new FlightBookService(config.Url, config.Username, config.Password);
                dto.CurrencyType = "VND";
                dto.FlightType = "DirectAndContinue";
                dto.Sources = AirlineBrand.VietJetAir;
                var data = await apiClient.FindFlight(dto);
                return new {
                    success = true,
                    data = data
                };
            }
            catch (Exception e)
            {
                return new
                {
                    success = false,
                    message = e.Message
                };
            }
        }

        [HttpPost]
        public async Task<dynamic> FindFlightVN([FromBody]FindFlight dto)
        {
            try
            {
                ApiConfigService configService = new ApiConfigService();
                var config = await configService.Get();
                FlightBookService apiClient = new FlightBookService(config.Url, config.Username, config.Password);
                dto.CurrencyType = "VND";
                dto.FlightType = "DirectAndContinue";
                dto.Sources = AirlineBrand.VietnamAirlines;
                var data = await apiClient.FindFlight(dto);
                return new
                {
                    success = true,
                    data = data
                };
            }
            catch (Exception e)
            {
                return new
                {
                    success = false,
                    message = e.Message
                };
            }
        }

        [HttpPost]
        public async Task<dynamic> FindFlightJS([FromBody]FindFlight dto)
        {
            try
            {
                ApiConfigService configService = new ApiConfigService();
                var config = await configService.Get();
                FlightBookService apiClient = new FlightBookService(config.Url, config.Username, config.Password);
                dto.CurrencyType = "VND";
                dto.FlightType = "DirectAndContinue";
                dto.Sources = AirlineBrand.JetStar;
                var data = await apiClient.FindFlight(dto);
                return new
                {
                    success = true,
                    data = data
                };
            }
            catch (Exception e)
            {
                return new
                {
                    success = false,
                    message = e.Message
                };
            }
        }

        [HttpPost]
        public async Task<dynamic> BookFlight([FromBody]BookingDTO dto)
        {
            try
            {
                string userId = User.Identity.GetUserId();
                ApiConfigService configService = new ApiConfigService();
                var config = await configService.Get();
                FlightBookService apiClient = new FlightBookService(config.Url, config.Username, config.Password);
                var result = await apiClient.BookFlight(dto, userId, Request.GetOwinContext().Request.RemoteIpAddress);
                return new {
                    success=true,
                    data = result 
                };
            }
            catch(Exception e)
            {
                return new {
                    success = false,
                    message = e.Message
                };
            }
        }

        [HttpGet]
        public async Task<dynamic> ProcessCallback([FromUri]int id, [FromUri]string brand , [FromUri]string bookingCode)
        {
            try
            {
                string path = System.Web.HttpContext.Current.Server.MapPath("~/LogCallback");
                string fileName = string.Format("log.txt", DateTime.Now);
                string filePath = System.IO.Path.Combine(path, fileName);
                StreamWriter sw = new StreamWriter(filePath, true);
                await sw.WriteLineAsync(bookingCode + "@" + brand + "@" + id);
                sw.Flush();
                sw.Close();

                await FlightBookService.UpdateBookPNR(id, brand, bookingCode);

                return new { success = true };
            }
            catch (Exception e)
            {
                return new
                {
                    success = false,
                    message = e.Message
                };
            }
        }

        [HttpPost]
        public async Task<dynamic> FindFlights([FromBody]FindFlight dto)
        {
            EvScrapeService service = new EvScrapeService(50000, 50000, 0);
            string departureDate = dto.DepartDate.Split(new char[] { 'T' })[0];
            string returnDate = string.Empty;
            if (dto.RoundTrip)
            {
                returnDate = dto.ReturnDate.Split(new char[] { 'T' })[0];
            }
            return await service.SearchFlight(dto.FromPlace, dto.ToPlace, departureDate, returnDate, dto.Adult, dto.Child, dto.Infant);
        }
             
    }
}