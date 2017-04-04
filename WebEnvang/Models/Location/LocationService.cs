using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Data;

namespace WebEnvang.Models.Location
{
    public class LocationService : BaseService
    {
        public async Task<dynamic> GetList(LocationDTO dto)
        {
            DataTable dt = (await MsSqlHelper.ExecuteDataSetTask(ConnectionString, "sp_location_getlist", 
                new string[] { "@regionId" },
                new object[] { dto.RegionId })).Tables[0];
            return (from r in dt.AsEnumerable()
                    select new
                    {
                        Id = r.Field<object>("Id"),
                        Name = r.Field<object>("Name"),
                        Code = r.Field<object>("Code"),
                        ApiPlaceId = r.Field<object>("ApiPlaceId"),
                        RegionId = r.Field<object>("RegionId"),
                        RegionName = r.Field<object>("RegionName"),
                        Order = r.Field<object>("Order"),
                        RegionColumn = r.Field<object>("RegionColumn"),
                        RegionOrder = r.Field<object>("RegionOrder")
                    }).ToList();
        }

        public async Task<dynamic> GetListAndGroup()
        {
            DataTable dt = (await MsSqlHelper.ExecuteDataSetTask(ConnectionString, "sp_location_getlist",
                new string[] { "@regionId" },
                new object[] { 0 })).Tables[0];

            var regionList = (
                from r in dt.AsEnumerable()
                orderby r.Field<int>("RegionColumn"), r.Field<int>("RegionOrder")
                select new LocationDTO
                {
                    RegionId = r.Field<int>("RegionId"),
                    RegionColumn = r.Field<int>("RegionColumn"),
                    RegionOrder = r.Field<int>("RegionOrder"),
                    RegionName = r.Field<string>("RegionName")
                }).Distinct<LocationDTO>(new RegionComparer()).ToList();


            return (from region in regionList
                    select new
                    {
                        RegionId = region.RegionId,
                        RegionColumn = region.RegionColumn,
                        RegionOrder = region.RegionOrder,
                        RegionName = region.RegionName,
                        LocationList = (
                            from r in dt.AsEnumerable()
                            where r.Field<int>("RegionId") == region.RegionId
                            orderby r.Field<int>("Order")
                            select new
                            {
                                Id = r.Field<object>("Id"),
                                Name = r.Field<object>("Name"),
                                Code = r.Field<object>("Code"),
                                ApiPlaceId = r.Field<object>("ApiPlaceId")
                            }
                        )
                    }).ToList();
        }

        public async Task<dynamic> GetListDestLocationAndGroup(int sourceId)
        {
            DataTable dt = (await MsSqlHelper.ExecuteDataSetTask(ConnectionString, "sp_flightroute_getlist",
                new string[] { "@sourceId" },
                new object[] { sourceId })).Tables[0];

            var regionList = (
                from r in dt.AsEnumerable()
                orderby r.Field<int>("RegionColumn"), r.Field<int>("RegionOrder")
                select new LocationDTO
                {
                    RegionId = r.Field<int>("RegionId"),
                    RegionColumn = r.Field<int>("RegionColumn"),
                    RegionOrder = r.Field<int>("RegionOrder"),
                    RegionName = r.Field<string>("RegionName")
                }).Distinct<LocationDTO>(new RegionComparer()).ToList();


            return (from region in regionList
                    select new
                    {
                        RegionId = region.RegionId,
                        RegionColumn = region.RegionColumn,
                        RegionOrder = region.RegionOrder,
                        RegionName = region.RegionName,
                        LocationList = (
                            from r in dt.AsEnumerable()
                            where r.Field<int>("RegionId") == region.RegionId
                            orderby r.Field<int>("Order")
                            select new
                            {
                                Id = r.Field<object>("Id"),
                                Name = r.Field<object>("Name"),
                                Code = r.Field<object>("Code"),
                                ApiPlaceId = r.Field<object>("ApiPlaceId"),
                                Routed = r.Field<object>("Routed")
                            }
                        )
                    }).ToList();
        }

        public async Task<dynamic> GetListDestLocationRoutedAndGroup(int sourceId)
        {
            DataTable dt = (await MsSqlHelper.ExecuteDataSetTask(ConnectionString, "sp_flightroute_getlist_routed",
                new string[] { "@sourceId" },
                new object[] { sourceId })).Tables[0];

            var regionList = (
                from r in dt.AsEnumerable()
                orderby r.Field<int>("RegionColumn"), r.Field<int>("RegionOrder")
                select new LocationDTO
                {
                    RegionId = r.Field<int>("RegionId"),
                    RegionColumn = r.Field<int>("RegionColumn"),
                    RegionOrder = r.Field<int>("RegionOrder"),
                    RegionName = r.Field<string>("RegionName")
                }).Distinct<LocationDTO>(new RegionComparer()).ToList();


            return (from region in regionList
                    select new
                    {
                        RegionId = region.RegionId,
                        RegionColumn = region.RegionColumn,
                        RegionOrder = region.RegionOrder,
                        RegionName = region.RegionName,
                        LocationList = (
                            from r in dt.AsEnumerable()
                            where r.Field<int>("RegionId") == region.RegionId
                            orderby r.Field<int>("Order")
                            select new
                            {
                                Id = r.Field<object>("Id"),
                                Name = r.Field<object>("Name"),
                                Code = r.Field<object>("Code"),
                                ApiPlaceId = r.Field<object>("ApiPlaceId")
                            }
                        )
                    }).ToList();
        }

        public Task Save(LocationDTO dto, string userId, string IP)
        {
            return MsSqlHelper.ExecuteNonQueryTask(ConnectionString, "sp_location_save",
                new string[] { "@id", "@name", "@code", "@apiplaceid", "@regionId", "@order", "@userid", "@ip" },
                new object[] { dto.Id, dto.Name, dto.Code, dto.ApiPlaceId, dto.RegionId, dto.Order, userId, IP });
        }

        public Task Delete(LocationDTO dto, string userId, string IP)
        {
            return MsSqlHelper.ExecuteNonQueryTask(ConnectionString, "sp_location_delete",
                new string[] { "@id", "@userid", "@ip" },
                new object[] { dto.Id, userId, IP });
        }
    }
}