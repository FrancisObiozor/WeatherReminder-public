namespace WeatherReminder.Models.WeatherReminderModel
{
    public class WeatherSettings
    {
        public int Id { get; set; }
        public Temperature Temperature { get; set; }
        public Umbrella Umbrella { get; set; }
        public Snow Snow { get; set; }
        public bool IsEmailNotification { get; set; }
        public bool IsSmsNotification { get; set; }
        public bool IsCellVerified { get; set; }
        public string Reminders { get; set; }
    }
}