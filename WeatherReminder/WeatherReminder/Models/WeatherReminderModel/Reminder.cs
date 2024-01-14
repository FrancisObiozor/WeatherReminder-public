using System;

namespace WeatherReminder.Models.WeatherReminderModel
{
    public class Reminder
    {
        public int DaysBeforeEvent { get; set; }
        public DateTime ReminderTime { get; set; }
    }
}
