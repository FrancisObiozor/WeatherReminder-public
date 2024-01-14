namespace WeatherReminder.Models.ApiModel.GeocodeApi.GoogleGeocode
{
    public class Result
    {
        public AddressComponents[] Address_components { get; set; }
        public string Formatted_address { get; set; }
        public Geometry Geometry { get; set; }
    }

}
