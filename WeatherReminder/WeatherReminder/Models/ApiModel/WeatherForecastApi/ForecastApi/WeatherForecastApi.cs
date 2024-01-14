using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherReminder.Models.ApiModel.WeatherForecastApi.ForecastApi
{
    public class WeatherForecastApi
    {
        public DailyForecast[] DailyForecasts { get; set; }
    }
}

