using System.Collections.Generic;

namespace WeatherReminder.Models.WeatherReminderSupportModels
{
    public class TemperatureReminder : ITemperatureReminder
    {
        public int TemperatureCutoff { get; set; }
        public bool IsReminder { get; set; }
        public List<Reminder> Reminders { get; set; }
        public int ListNumber { get; set; }
    }
}
