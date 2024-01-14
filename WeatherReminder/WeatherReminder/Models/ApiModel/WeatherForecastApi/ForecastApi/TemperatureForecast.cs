namespace WeatherReminder.Models.ApiModel.WeatherForecastApi.ForecastApi
{
    public class TemperatureForecast
    {
        public Minimum Minimum { get; set; }
        public Maximum Maximum { get; set; }
    }

    public class Minimum
    {
        public float Value { get; set; }
        public string Unit { get; set; }
        public int UnitType { get; set; }
    }

    public class Maximum
    {
        public float Value { get; set; }
        public string Unit { get; set; }
        public int UnitType { get; set; }
    }
}
