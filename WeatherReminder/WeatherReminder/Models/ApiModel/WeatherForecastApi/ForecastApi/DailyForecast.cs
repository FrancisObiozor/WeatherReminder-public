using System;

namespace WeatherReminder.Models.ApiModel.WeatherForecastApi.ForecastApi
{
    public class DailyForecast
    {
        public DateTime Date { get; set; }
        public TemperatureForecast Temperature { get; set; }
        public Day Day { get; set; }
        public Night Night { get; set; }
    }
}
