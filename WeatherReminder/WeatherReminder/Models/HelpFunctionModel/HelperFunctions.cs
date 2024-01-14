using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeatherReminder.Models.WeatherReminderModel;

namespace WeatherReminder.Models.HelpFunctionModel
{
    public class HelperFunctions : IHelperFunctions
    {
        public List<Reminder> StringToList(string reminders)
        {
            var stringToList = new List<Reminder>();
            if (string.IsNullOrWhiteSpace(reminders))
            {
                return stringToList;
            }

            var list = reminders.Split(Environment.NewLine);
            foreach (var item in list)
            {
                if (string.IsNullOrWhiteSpace(item))
                {
                    continue;
                }
                var entries = item.Split(", ");
                var reminder = new Reminder();

                reminder.DaysBeforeEvent = Convert.ToInt32(entries[0]);
                reminder.ReminderTime = Convert.ToDateTime(entries[1]);
                stringToList.Add(reminder);
            }

            return stringToList;
        }

        public string ListToString(List<Reminder> reminders)
        {
            if (reminders == null || !reminders.Any())
            {
                return "";
            }

            var convertedString = new StringBuilder();
            var newLine = Environment.NewLine;
            foreach (var reminder in reminders)
            {
                string formattedTime = reminder.ReminderTime.ToString("hh:mm tt"); // It will give "08:00 AM"

                convertedString.Append($"{reminder.DaysBeforeEvent}, {formattedTime}{newLine}");
            }

            var length = convertedString.Length;
            convertedString.Remove(length - newLine.Length, newLine.Length);

            return convertedString.ToString();
        }

    }
}
