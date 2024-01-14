using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeatherReminder.Models.WeatherReminderModel;

namespace WeatherReminder.ViewModels.RemindersViewModels
{
    public class EditRemindersViewModel
    {
        public List<Reminder> RemindersList { get; set; }
        public string RemindersString { get; set; }
    }
}
