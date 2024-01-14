using System.Collections.Generic;
using WeatherReminder.Models.WeatherReminderSupportModels;

namespace WeatherReminder.Models
{
    public interface ITemperatureReminder
    {
        public int TemperatureCutoff { get; set; }
        public bool IsReminder { get; set; }
        public List<Reminder> Reminders { get; set; }
        public int ListNumber { get; set; }
    }
}
