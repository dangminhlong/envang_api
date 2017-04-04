using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebEnvang.Models.Location
{
    public class LocationDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string ApiPlaceId { get; set; }
        public int RegionId { get; set; }
        public int Order { get; set; }
        public int RegionOrder { get; set; }
        public int RegionColumn { get; set; }
        public string RegionName { get; set; }
    }

    public class RegionComparer : IEqualityComparer<LocationDTO>
    {
        public bool Equals(LocationDTO x, LocationDTO y)
        {
            return x.RegionId == y.RegionId;
        }

        public int GetHashCode(LocationDTO obj)
        {
            return obj.RegionId;
        }
    }
}