using WeatherReminder.Models.ApiModel.GeocodeApi;
using WeatherReminder.Models.PositionModel;

namespace WeatherReminder.Models.ApiModel.LocationFromBrowserApi
{
    public class LocationFromBrowser : ILocationFromBrowser
    {

        public CityStateCountry CityStateCountry { get; set; }


    }
}
