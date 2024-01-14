namespace WeatherReminder.Models.WeatherReminderModel
{
    public class UserLocation
    {
        public int Id { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string LocationKey { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public bool PullLocationFromIp { get; set; }
    }
}