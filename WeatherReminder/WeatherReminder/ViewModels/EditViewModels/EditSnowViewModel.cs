using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WeatherReminder.Models.WeatherReminderModel;

namespace WeatherReminder.ViewModels.EditViewModels
{
    public class EditSnowViewModel
    {
        [Required(ErrorMessage = "Reminder On/Off is required")]
        [Display(Name = "Snow Reminder On/Off")]
        public bool IsReminder { get; set; }

        public List<Reminder> RemindersList { get; set; }

        public string RemindersString { get; set; }
    }
}
