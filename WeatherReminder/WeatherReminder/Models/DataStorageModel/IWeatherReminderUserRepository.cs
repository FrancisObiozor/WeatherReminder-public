using System.Collections.Generic;
using System.Security.Claims;
using WeatherReminder.Models.ApiModel.WeatherForecastApi.ForecastApi;
using WeatherReminder.Models.WeatherReminderModel;

namespace WeatherReminder.Models.DataStorageModel
{
    public interface IWeatherReminderUserRepository
    {
        WeatherReminderUser GetUser(ClaimsPrincipal currentUser);
        Temperature GetTemperature(ClaimsPrincipal currentUser);
        UserLocation GetLocation(ClaimsPrincipal currentUser);
        void SaveLocation(UserLocation location, ClaimsPrincipal currentUser);
        void SaveTemperature(Temperature temperature, ClaimsPrincipal currentUser);
        void SaveUser(WeatherReminderUser user);
        void SaveWeatherForecast(DailyForecast[] weatherForecasts, ClaimsPrincipal currentUser);
        List<WeatherReminderUser> GetAllUsers();
        void DeleteUserSettings(WeatherReminderUser user);
        void DeleteAllUsers();
        void DeleteUser(WeatherReminderUser user);
        void DeleteUserByEmail(string email);
        public WeatherReminderUser GetUserByEmail(string email);

    }
}
