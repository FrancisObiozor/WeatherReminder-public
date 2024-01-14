using Microsoft.AspNetCore.Mvc;
using WeatherReminder.Models.ApiModel.GeocodeApi;
using WeatherReminder.Models.DataStorageModel;
using WeatherReminder.Models.PositionModel;
using Microsoft.AspNetCore.Identity;
using WeatherReminder.Models.WeatherReminderModel;
using WeatherReminder.Models.ApiModel.WeatherForecastApi.ForecastApi;
using WeatherReminder.Models.ApiModel.WeatherForecastApi.LocationApi;

namespace WeatherReminder.Models.ApiModel.LocationFromBrowserApi
{
    public class LocationFromBrowserController : Controller
    {
        private readonly IGeocodeLocation _geocodeLocation;
        private readonly ILocationFromBrowser _locationFromBrowser;
        private readonly IWeatherReminderUserRepository _weatherReminderUserRepository;
        private readonly IWeatherForecast _weatherForecast;
        private readonly ILocationKey _locationKey;

        public LocationFromBrowserController(
            IGeocodeLocation geocodeLocation,
            ILocationFromBrowser locationFromBrowser,
            IWeatherReminderUserRepository weatherReminderUserRepository,
            IWeatherForecast weatherForecast,
            ILocationKey locationKey
            )
        {
            _geocodeLocation = geocodeLocation;
            _locationFromBrowser = locationFromBrowser;
            _weatherReminderUserRepository = weatherReminderUserRepository;
            _weatherForecast = weatherForecast;
            _locationKey = locationKey;
        }

        [HttpGet]
        public IActionResult GetLocation(double latitude, double longitude)
        {
            var user = _weatherReminderUserRepository.GetUser(User);
            var currentUserCityStateCountry = _geocodeLocation.GetCityStateCountry(latitude, longitude).Result;
            _locationFromBrowser.CityStateCountry = currentUserCityStateCountry;

            return Ok();
        }

    }
}
