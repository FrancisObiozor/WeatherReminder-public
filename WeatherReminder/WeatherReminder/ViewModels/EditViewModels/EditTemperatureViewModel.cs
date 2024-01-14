using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WeatherReminder.Models.WeatherReminderModel;

namespace WeatherReminder.ViewModels.EditViewModels
{
    public class EditTemperatureViewModel
    {
        [Required(ErrorMessage = "Reminder On/Off is required")]
        [Display(Name = "Temperature Reminder On/Off")]
        public bool IsReminder { get; set; }

        [Required(ErrorMessage = "Temperature Cut Off is required")]
        [Display(Name = "Temperature Cut Off")]
        [Range(0, 150)]
        public int TemperatureCutoff { get; set; }

        public List<Reminder> RemindersList { get; set; }

        public string RemindersString { get; set; }

    }
}
