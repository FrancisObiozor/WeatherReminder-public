using System.Collections.Generic;
using WeatherReminder.Models.WeatherReminderModel;

namespace WeatherReminder.Models.HelpFunctionModel
{
    public interface IHelperFunctions
    {
        public string ListToString(List<Reminder> reminders);
        List<Reminder> StringToList(string reminders);
    }
}