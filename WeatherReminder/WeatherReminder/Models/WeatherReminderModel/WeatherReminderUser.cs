using Microsoft.AspNetCore.Identity;
using System;

namespace WeatherReminder.Models.WeatherReminderModel
{
    // Add profile data for application users by adding properties to the WeatherReminderUser class
    public class WeatherReminderUser : IdentityUser
    {
        [PersonalData]
        public UserLocation Location { get; set; }

        [PersonalData]
        public WeatherSettings Weather { get; set; }

        [PersonalData]
        public string DailyForecast { get; set; }

        [PersonalData]
        public string CountryCode { get; set; }

        [PersonalData]
        public string TimeZoneDifference { get; set; }
    }
}

