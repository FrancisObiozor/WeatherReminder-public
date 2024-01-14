
using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Diagnostics;
using System.Dynamic;
using System.Text;
using System.Threading.Tasks;
using WeatherReminder.Models.ApiModel.EmailApi;
using WeatherReminder.Models.ApiModel.GeocodeApi;
using WeatherReminder.Models.ApiModel.IpAddressGeocodeApi;
using WeatherReminder.Models.ApiModel.LocationFromBrowserApi;
using WeatherReminder.Models.ApiModel.TextMessageApi;
using WeatherReminder.Models.ApiModel.WeatherForecastApi.ForecastApi;
using WeatherReminder.Models.ApiModel.WeatherForecastApi.LocationApi;
using WeatherReminder.Models.BackgroundModel;
using WeatherReminder.Models.DataStorageModel;
using WeatherReminder.Models.HelpFunctionModel;
using WeatherReminder.Models.HomepageModel;
using WeatherReminder.Models.PositionModel;
using WeatherReminder.Models.WeatherReminderModel;
using WeatherReminder.ViewModels;
using WeatherReminder.ViewModels.HomeViewModels;

namespace WeatherReminder.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWeatherReminderUserRepository _weatherReminderUserRepository;
        private readonly IGeocodeLocation _geoCodeLocation;
        private readonly IIpAddressGeocode _ipAddressGeocode;
        private readonly IHomepageStats _homepageStats;
        private readonly ILocationKey _locationKey;
        private readonly IRecurringJobManager _recurringJobManager;
        private readonly IBackgroundTasks _backgroundTasks;
        private readonly ICustomEmail _customEmail;
        private readonly ITextMessage _textMessage;
        private readonly IHelperFunctions _helperFunctions;
        private readonly IWeatherForecast _weatherForecast;
        private readonly UserManager<WeatherReminderUser> _userManager;
        private readonly ILocationFromBrowser _locationFromBrowser;

        public HomeController(IWeatherReminderUserRepository weatherReminderUserRepository,
            IGeocodeLocation geoCodeLocation,
            IIpAddressGeocode ipAddressGeocode,
            IHomepageStats homepageStats,
            ILocationKey locationKey,
            IRecurringJobManager recurringJobManager,
            IBackgroundTasks backgroundTasks,
            ICustomEmail customEmail,
            ITextMessage textMessage,
            IHelperFunctions helperFunctions,
            IWeatherForecast weatherForecast,
            UserManager<WeatherReminderUser> userManager,
            ILocationFromBrowser locationFromBrowser
            )
        {
            _weatherReminderUserRepository = weatherReminderUserRepository;
            _geoCodeLocation = geoCodeLocation;
            _ipAddressGeocode = ipAddressGeocode;
            _homepageStats = homepageStats;
            _locationKey = locationKey;
            _recurringJobManager = recurringJobManager;
            _backgroundTasks = backgroundTasks;
            _customEmail = customEmail;
            _textMessage = textMessage;
            _helperFunctions = helperFunctions;
            _weatherForecast = weatherForecast;
            _userManager = userManager;
            _locationFromBrowser = locationFromBrowser;
        }

        public IActionResult Welcome()
        {
            //_recurringJobManager.AddOrUpdate(
            //    "Send User Reminder",
            //    () => _backgroundTasks.SendUsersReminders(),
            //    "* * * * *"
            //    );

            //_recurringJobManager.AddOrUpdate(
            //    "Update Weather Forecast",
            //    () => _backgroundTasks.UpdateUsersWeatherForecast(),
            //    "0 0 * * *"
            //    );

            //_recurringJobManager.AddOrUpdate(
            //    "Delete Unconfirmed Accounts",
            //    () => _backgroundTasks.DeleteUnconfirmedAccounts(),
            //    "*/15 * * * *"
            //    );

            return RedirectToPage("/LoginUser/Login", new { area = "Identity" });

        }


        public IActionResult Index(string timeZoneDifference)
        {

            WeatherReminderUser user = _weatherReminderUserRepository.GetUser(User);


            var currentLocation = UpdateCurrentLocation(user);
            var homePage = new HomePageViewModel();

            homePage.DailyForecasts = _weatherForecast.DailyForecastToArray(user.DailyForecast);
            homePage.Reminders = _helperFunctions.StringToList(user.Weather.Reminders);
            homePage.City = currentLocation.City;
            homePage.State = currentLocation.State;
            homePage.Country = currentLocation.Country;
            homePage.PhoneNumberConfirmed = user.PhoneNumberConfirmed;
            homePage.IsUmbrellaOn = user.Weather.Umbrella.IsReminder;
            homePage.IsSnowOn = user.Weather.Snow.IsReminder;
            homePage.IsTemperatureOn = user.Weather.Temperature.IsReminder;
            homePage.TemperatureCutoff = user.Weather.Temperature.TemperatureCutoff;
            homePage.TimeZoneDifference = timeZoneDifference;
            UpdateTimeZoneDifference(timeZoneDifference, user);


            return View(homePage);
        }

        [HttpPost]
        public IActionResult Index(HomePageViewModel homePageViewModel)
        {
            var user = _weatherReminderUserRepository.GetUser(User);

            user.Weather.Umbrella.IsReminder = homePageViewModel.IsUmbrellaOn;
            user.Weather.Snow.IsReminder = homePageViewModel.IsSnowOn;
            user.Weather.Temperature.IsReminder = homePageViewModel.IsTemperatureOn;
            user.Weather.Temperature.TemperatureCutoff = homePageViewModel.TemperatureCutoff;

            _weatherReminderUserRepository.SaveUser(user);
            return View(homePageViewModel);
        }

        private void UpdateTimeZoneDifference(string timeZoneDifference, WeatherReminderUser user)
        {
            if (timeZoneDifference != null && _homepageStats.TimesVisited <= 1)
            {
                user.TimeZoneDifference = timeZoneDifference;
                _weatherReminderUserRepository.SaveUser(user);
            }
        }

        private CityStateCountry UpdateCurrentLocation(WeatherReminderUser user)
        {
            var currentlocation = _locationFromBrowser.CityStateCountry;
            bool isNewLocation = false;
            if (!(currentlocation == null || object.ReferenceEquals(currentlocation, new object())))
            {
                isNewLocation = true;
            }

            if (isNewLocation && _homepageStats.TimesVisited == 0)
            {
                if (currentlocation.City != user.Location.City ||
                   currentlocation.State != user.Location.State ||
                   currentlocation.Country != user.Location.Country)
                {
                    _homepageStats.TimesVisited++;
                    return UpdateLocation(user, currentlocation);
                }
            }

            var existingCityStateCountry = new CityStateCountry
            {
                City = user.Location.City,
                State = user.Location.State,
                Country = user.Location.Country
            };
            _homepageStats.TimesVisited++;
            return existingCityStateCountry;
        }

        private CityStateCountry UpdateLocation(WeatherReminderUser user, CityStateCountry currentLocation)
        {
            var cityStateCountry = new CityStateCountry
            {
                City = currentLocation.City,
                State = currentLocation.State,
                Country = currentLocation.Country
            };

            Coordinates latLong = _geoCodeLocation.GetLatitudeLongitude(cityStateCountry).Result;
            var locationKey = _locationKey.GetLocationKey(latLong).Result;
            var weatherForecast = _weatherForecast.GetWeatherForcast(locationKey).Result;

            user.Location.City = cityStateCountry.City;
            user.Location.State = cityStateCountry.State;
            user.Location.Country = cityStateCountry.Country;
            user.Location.Latitude = latLong.Latitude;
            user.Location.Longitude = latLong.Longitude;
            user.Location.LocationKey = locationKey;
            user.DailyForecast = _weatherForecast.DailyForecastToString(weatherForecast);

            _weatherReminderUserRepository.SaveUser(user);
            return cityStateCountry;
        }



        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        ////For testing
        //public void ResetPhone(string phone, WeatherReminderUser user)
        //{
        //    var setPhoneResult = _userManager.SetPhoneNumberAsync(user, phone).Result;
        //    user.PhoneNumberConfirmed = false;
        //    _weatherReminderUserRepository.SaveUser(user);
        //}

        ////For testing
        //public void SaveTestReminder()
        //{
        //    ClaimsPrincipal currentUser = this.User;
        //    var temperature = new Temperature();
        //    var reminder = new Reminder();
        //    TimeSpan timespan = new TimeSpan(09, 00, 00);
        //    DateTime time = DateTime.Today.Add(timespan);
        //    string displayTime = time.ToString("hh:mm tt"); // It will give "08:00 AM"

        //    temperature.TemperatureCutoff = 60;
        //    temperature.ReminderDetails = new ReminderDetails();
        //    temperature.ReminderDetails.IsReminder = false;
        //    temperature.ReminderDetails.Reminders = $"0, {displayTime}{Environment.NewLine}1, 09:00 PM";

        //    _weatherReminderUserRepository.SaveTemperature(temperature, currentUser);
        //}


        //Autocomplete Api test
        //var cityStateCountry = new CityStateCountry
        //{
        //    City = "Rockville",
        //    State = "Maryland",
        //    Country = "United States"
        //};
        //var autoComplete = _autoComplete.AutoCompleteOptions(cityStateCountry);

        //Email Api test
        //EmailModel email = new EmailModel();
        //email.Message = "{userName}, your temperature setting of {tempSetting}° will be reached today. The high for {dayofEvent} is {max}°. The low for {dayofEvent} is {min}°.";
        //email.Email = "fobiozor19@gmail.com";
        //email.Subject = "Temperature Setting Reached";
        //_email.SendEmailAsync(email);

        //Geocode Api test
        //var cityStateCountry = new CityStateCountry
        //{
        //    City = "Rockville",
        //    State = "Maryland",
        //    Country = "United States"
        //};
        //var latLong = _geoCodeLocation.GetLatitudeLongitude(cityStateCountry);
        //var verifyLocation = _geoCodeLocation.VerifyLocationMatches(cityStateCountry);

        //Text message api test
        //_textMessage.SendText("+19043384419", "{userName}, your temperature setting of {tempSetting}° will be reached today. The high for {dayofEvent} is {max}°. The low for {dayofEvent} is {min}°. {Environment.NewLine}{Environment.NewLine}Reply STOP to unsubscribe. Msg&Data Rates May Apply.");

        //Test IpAddressGeocode
        //var ipGeocode =_ipAddressGeocode.GetLocationFromIp().Result;

        //Test Location Key
        //var coordinates = new Coordinates();
        //coordinates.Latitude = "39.0839973";
        //coordinates.Longitude = "-77.1527578";
        //var locationKey = _locationKey.GetLocationKey(coordinates).Result;

        //Test weather forecast API
        //var testLocationKey = "329305";
        //var weatherForecast = _weatherForecast.GetWeatherForcast(testLocationKey).Result;

    }
}

