using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WeatherReminder.Models.ApiModel.WeatherForecastApi.ForecastApi;
using WeatherReminder.Models.WeatherReminderModel;

namespace WeatherReminder.ViewModels.HomeViewModels
{
    public class HomePageViewModel
    {
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public bool PhoneNumberConfirmed { get; set; }

        public List<Reminder> Reminders { get; set; }
        [Required]
        public bool IsTemperatureOn { get; set; }
        [Required]
        public int TemperatureCutoff { get; set; }
        [Required]
        public bool IsUmbrellaOn { get; set; }
        [Required]
        public bool IsSnowOn { get; set; }

        public string TimeZoneDifference { get; set; }
        public DailyForecast[] DailyForecasts { get; set; }
    }
}
