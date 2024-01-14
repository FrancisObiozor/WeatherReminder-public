namespace WeatherReminder.Models.WeatherReminderModel
{
    public class Temperature
    {
        public int Id { get; set; }
        public int TemperatureCutoff { get; set; }
        public bool IsReminder { get; set; }
    }
}