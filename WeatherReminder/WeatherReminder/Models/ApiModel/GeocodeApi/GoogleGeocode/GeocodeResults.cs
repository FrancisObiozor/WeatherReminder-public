using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherReminder.Models.ApiModel.GeocodeApi.GoogleGeocode
{
    public class GeocodeResults
    {
        public Result[] Results { get; set; }
        public string Status { get; set; }
    }
}
