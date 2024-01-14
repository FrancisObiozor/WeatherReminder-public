using WeatherReminder.Models.ApiModel.GeocodeApi.GoogleGeocode;

namespace WeatherReminder.Models.ApiModel.GeocodeApi
{
    public class GeocodeModel
    {
        public string ErrorString { get; set; }
        public GeocodeResults LocationData { get; set; }
    }
}

