using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherReminder.Models.ApiModel.WeatherForecastApi.ForecastApi
{
    public class WeatherForecastModel
    {
        public WeatherForecastApi WeatherForecast { get; set; }
        public string Error { get; set; }
    }
}
