using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WeatherReminder.ViewModels.RemindersViewModels
{
    public class ReminderViewModel
    {
        public int ListNumber { get; set; }

        [Required(ErrorMessage = "Please enter a days before event")]
        [Range(0, 4,
        ErrorMessage = "Days Before Event must be between {1} and {2}.")]
        [Display(Name = "Days Before Event")]
        public int DaysBeforeEvent { get; set; }

        [Required(ErrorMessage = "Please enter a time of day")]
        [DataType(DataType.Time)]
        [Display(Name = "Time")]
        public DateTime ReminderTime { get; set; }

        public string Reminders { get; set; }
    }
}
