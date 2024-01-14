using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace WeatherReminder.Models.GeocodeApi.OpencageGeocode
{
    public class OpenCageLatLongApi
    {
        public LatitudeLongitudeApiResult[] Results { get; set; }
    }

}
