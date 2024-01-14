using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherReminder.Models.ApiModel.IpAddressGeocodeApi
{
    public class IpAddressGeocodeResults
    {
        public string Country_name { get; set; }
        public string Region_name { get; set; }
        public string City { get; set; }
    }
}
