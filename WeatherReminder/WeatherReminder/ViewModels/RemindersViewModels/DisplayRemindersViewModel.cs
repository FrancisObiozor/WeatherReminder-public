using System.Collections.Generic;
using WeatherReminder.Models.WeatherReminderModel;

namespace WeatherReminder.ViewModels.RemindersViewModels
{
    public class DisplayRemindersViewModel
    {
        public List<Reminder> Reminders { get; set; } = new List<Reminder>();
    }
}
