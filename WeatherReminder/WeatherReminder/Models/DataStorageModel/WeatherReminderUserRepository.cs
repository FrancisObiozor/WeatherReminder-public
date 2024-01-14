using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Xml.Serialization;
using WeatherReminder.Models.ApiModel.WeatherForecastApi.ForecastApi;
using WeatherReminder.Models.WeatherReminderModel;

namespace WeatherReminder.Models.DataStorageModel
{
    public class WeatherReminderUserRepository : IWeatherReminderUserRepository
    {
        private readonly UserManager<WeatherReminderUser> _userManager;
        private readonly SignInManager<WeatherReminderUser> _signInManager;
        private readonly WeatherReminderDbContext _weatherReminderDbContext;

        public WeatherReminderUserRepository(UserManager<WeatherReminderUser> userManager,
                                             SignInManager<WeatherReminderUser> signInManager,
                                             WeatherReminderDbContext weatherReminderDbContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _weatherReminderDbContext = weatherReminderDbContext;
        }

        public WeatherReminderUser GetUser(ClaimsPrincipal currentUser)
        {
            var user = new WeatherReminderUser();
            var userId = _userManager.GetUserId(currentUser);

            if (userId == null)
            {
                return user;
            }
            else
            {
                var getUser = _userManager.Users.Where(d => d.Id == userId).
                Include(l => l.Location).
                Include(t => t.Weather.Temperature).
                Include(s => s.Weather.Snow).
                Include(u => u.Weather.Umbrella).
                ToList();

                user = getUser[0];

                return user;
            }
        }

        public WeatherReminderUser GetUserByEmail(string email)
        {
            var user = _userManager.FindByEmailAsync(email).Result;
            var userId = user.Id;
            if (user == null)
            {
                return user;
            }
            else
            {
                var getUser = _userManager.Users.Where(d => d.Id == userId).
                Include(l => l.Location).
                Include(t => t.Weather.Temperature).
                Include(s => s.Weather.Snow).
                Include(u => u.Weather.Umbrella).
                ToList();

                user = getUser[0];

                return user;
            }
        }

        public List<WeatherReminderUser> GetAllUsers()
        {
            if(_userManager.Users == null)
            {
                return null;
            }
            else if (_userManager.Users.Count() > 0)
            {
                return _userManager.Users.
                Include(l => l.Location).
                Include(t => t.Weather.Temperature).
                Include(s => s.Weather.Snow).
                Include(u => u.Weather.Umbrella).
                ToList();
            }
            return null;
        }

        public Temperature GetTemperature(ClaimsPrincipal currentUser)
        {
            var user = GetUser(currentUser);
            return user.Weather.Temperature;
        }

        public UserLocation GetLocation(ClaimsPrincipal currentUser)
        {
            var user = GetUser(currentUser);
            return user.Location;
        }

        public void SaveLocation(UserLocation location, ClaimsPrincipal currentUser)
        {
            var user = GetUser(currentUser);

            user.Location.City = location.City;
            user.Location.State = location.State;
            user.Location.Country = location.Country;
            user.Location.PullLocationFromIp = location.PullLocationFromIp;

            _userManager.UpdateAsync(user).Wait();
        }

        public void SaveTemperature(Temperature temperature, ClaimsPrincipal currentUser)
        {
            var user = GetUser(currentUser);

            user.Weather.Temperature.TemperatureCutoff = temperature.TemperatureCutoff;
            user.Weather.Temperature.IsReminder = temperature.IsReminder;

            _userManager.UpdateAsync(user).Wait();
        }

        public void SaveUser(WeatherReminderUser user)
        {
            _userManager.UpdateAsync(user).Wait();
        }

        public void SaveWeatherForecast(DailyForecast[] weatherForecasts, ClaimsPrincipal currentUser)
        {
            StringWriter writer = new StringWriter();
            XmlSerializer serializer = new XmlSerializer(typeof(DailyForecast[]));
            serializer.Serialize(writer, weatherForecasts);
            var dailyForecast = writer.ToString();

            var user = GetUser(currentUser);
            user.DailyForecast = new string(dailyForecast);

            _userManager.UpdateAsync(user).Wait();
        }


        public void DeleteUserSettings(WeatherReminderUser user)
        {
            var locationId = user.Location.Id;
            var weatherId = user.Weather.Id;
            var tempId = user.Weather.Temperature.Id;
            var snowId = user.Weather.Snow.Id;
            var umbrellaId = user.Weather.Umbrella.Id;
            //var tempReminderId = user.Weather.Temperature.ReminderDetails.Id;
            //var snowReminderId = user.Weather.Snow.ReminderDetails.Id;
            //var umbrellaReminderId = user.Weather.Umbrella.ReminderDetails.Id;

            var location = _weatherReminderDbContext.UserLocation.Find(locationId);
            _weatherReminderDbContext.UserLocation.Remove(location);

            var weather = _weatherReminderDbContext.WeatherSettings.Find(weatherId);
            _weatherReminderDbContext.WeatherSettings.Remove(weather);



            var temp = _weatherReminderDbContext.Temperature.Find(tempId);
            _weatherReminderDbContext.Temperature.Remove(temp);

            var umbrella = _weatherReminderDbContext.Umbrella.Find(umbrellaId);
            _weatherReminderDbContext.Umbrella.Remove(umbrella);

            var snow = _weatherReminderDbContext.Snow.Find(snowId);
            _weatherReminderDbContext.Snow.Remove(snow);



            //var tempReminders = _weatherReminderDbContext.ReminderDetails.Find(tempReminderId);
            //_weatherReminderDbContext.ReminderDetails.Remove(tempReminders);

            //var snowReminders = _weatherReminderDbContext.ReminderDetails.Find(snowReminderId);
            //_weatherReminderDbContext.ReminderDetails.Remove(snowReminders);

            //var umbrellaReminders = _weatherReminderDbContext.ReminderDetails.Find(umbrellaReminderId);
            //_weatherReminderDbContext.ReminderDetails.Remove(umbrellaReminders);
            _weatherReminderDbContext.SaveChanges();
        }

        public void DeleteUser(WeatherReminderUser user)
        {
            DeleteUserSettings(user);
            _userManager.DeleteAsync(user).Wait();
        }

        //For testing
        public void DeleteAllUsers()
        {
            var users = GetAllUsers();
            foreach (var user in users)
            {
                DeleteUser(user);
            }
        }

        //For testing
        public void DeleteUserByEmail(string email)
        {
            var user = GetUserByEmail(email);
            DeleteUser(user);
        }


    }
}
