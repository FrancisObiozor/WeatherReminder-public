using WeatherReminder.Models.PositionModel;

namespace WeatherReminder.Models.ApiModel.LocationFromBrowserApi
{
    public interface ILocationFromBrowser
    {
        public CityStateCountry CityStateCountry { get; set; }
    }
}
