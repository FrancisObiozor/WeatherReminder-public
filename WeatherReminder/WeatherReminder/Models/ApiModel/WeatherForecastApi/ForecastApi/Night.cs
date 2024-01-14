namespace WeatherReminder.Models.ApiModel.WeatherForecastApi.ForecastApi
{
    public class Night
    {
        public string IconPhrase { get; set; }
        public bool HasPrecipitation { get; set; }
        public string PrecipitationType { get; set; }
        public string PrecipitationIntensity { get; set; }
    }

}
