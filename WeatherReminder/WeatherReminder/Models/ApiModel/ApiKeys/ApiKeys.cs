namespace WeatherReminder.Models.ApiModel.ApiKeys
{
    public class ApiKeys
    {
        public string SendGridUser { get; set; }
        public string SendGridKey { get; set; }
        public string SendGridEmail { get; set; }
        public string ConnectionStringsWeatherReminderDbContextConnection { get; set; }
        public TwilioText Twilio { get; set; }
        public string AutoCompleteKey { get; set; }
        public string GeocodeKey { get; set; }
        public string IpAddressGeocodeKey { get; set; }
        public string WeatherKey { get; set; }
        public string LocationKey { get; set; }
    }
}
