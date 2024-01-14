using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherReminder.Models.ApiModel.IpAddressGeocodeApi
{
    public class IpAddressGeocodeModel
    {
        public IpAddressGeocodeResults Location { get; set; }
        public string Error { get; set; }
    }
}
