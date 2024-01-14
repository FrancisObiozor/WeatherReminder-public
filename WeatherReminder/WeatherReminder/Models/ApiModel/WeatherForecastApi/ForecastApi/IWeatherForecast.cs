using System.Threading.Tasks;

namespace WeatherReminder.Models.ApiModel.WeatherForecastApi.ForecastApi
{
    public interface IWeatherForecast
    {
        Task<DailyForecast[]> GetWeatherForcast(string locationKey);
        DailyForecast[] DailyForecastToArray(string dailyForecasts);
        string DailyForecastToString(DailyForecast[] dailyForecasts);
    }
}