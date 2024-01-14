using Microsoft.AspNetCore.Identity;
using System;
using System.Dynamic;
using System.Text;
using WeatherReminder.Models.ApiModel.EmailApi;
using WeatherReminder.Models.ApiModel.TextMessageApi;
using WeatherReminder.Models.ApiModel.WeatherForecastApi.ForecastApi;
using WeatherReminder.Models.DataStorageModel;
using WeatherReminder.Models.HelpFunctionModel;
using WeatherReminder.Models.UnconfirmedUsers;
using WeatherReminder.Models.WeatherReminderModel;

namespace WeatherReminder.Models.BackgroundModel
{
    public class BackgroundTasks : IBackgroundTasks
    {
        private readonly IWeatherReminderUserRepository _weatherReminderUserRepository;
        private readonly IHelperFunctions _helperFunctions;
        private readonly IWeatherForecast _weatherForecast;
        private readonly IEmail _email;
        private readonly ITextMessage _textMessage;
        private readonly UserManager<WeatherReminderUser> _userManager;
        private readonly ICustomEmail _customEmail;
        private readonly IUnconfirmedUserRepository _unconfirmedUserRepository;

        public BackgroundTasks(IWeatherReminderUserRepository weatherReminderUserRepository,
                               IHelperFunctions helperFunctions,
                               IWeatherForecast weatherForecast,
                               IEmail email,
                               ITextMessage textMessage,
                               UserManager<WeatherReminderUser> userManager,
                               ICustomEmail customEmail,
                               IUnconfirmedUserRepository unconfirmedUserRepository)
        {
            _weatherReminderUserRepository = weatherReminderUserRepository;
            _helperFunctions = helperFunctions;
            _weatherForecast = weatherForecast;
            _email = email;
            _textMessage = textMessage;
            _userManager = userManager;
            _customEmail = customEmail;
            _unconfirmedUserRepository = unconfirmedUserRepository;
        }

        public void SendUsersReminders()
        {
            var users = _weatherReminderUserRepository.GetAllUsers();

            if (users != null)
            {
                foreach (var user in users)
                {
                    var temperatureReminderOn = user.Weather.Temperature.IsReminder;
                    var snowReminderOn = user.Weather.Snow.IsReminder;
                    var umbrellaReminderOn = user.Weather.Umbrella.IsReminder;
                    if (!temperatureReminderOn && !snowReminderOn && !umbrellaReminderOn)
                    {
                        continue;
                    }

                    var reminders = user.Weather.Reminders;

                    if (string.IsNullOrWhiteSpace(reminders))
                    {
                        continue;
                    }

                    var dailyForecast = _weatherForecast.DailyForecastToArray(user.DailyForecast);
                    if (dailyForecast == null)
                    {
                        dailyForecast = UpdateSingleUserWeatherForecast(user);
                    }

                    //Tried to update forecast but could not so go to next user
                    if (dailyForecast == null)
                    {
                        continue;
                    }

                    SendReminders(user, dailyForecast);
                    //SendTemperatureReminders(temperatureReminders, user, dailyForecast);
                    //SendUmbrellaReminders(umbrellaReminders, user, dailyForecast);
                    //SendSnowReminders(snowReminders, user, dailyForecast);
                }
            }

        }

        private void SendReminders(WeatherReminderUser user, DailyForecast[] dailyForecast)
        {
            var reminders = _helperFunctions.StringToList(user.Weather.Reminders);
            //foreach (var reminder in reminders)
            for (int i = 0; i < 1; i++)
            {
                var reminder = reminders[i];
                var userSetTime = reminder.ReminderTime.ToString("hh:mm tt");

                var targetDate = DateTime.Now.AddDays(reminder.DaysBeforeEvent);
                var targetForecast = Array.Find(dailyForecast, d => d.Date.Day == targetDate.Day);

                //var currentTime = DateTime.Now.Add(TimeSpan.Parse(user.TimeZoneDifference)).ToString("hh:mm tt");
                //if (userSetTime == currentTime)
                //{
                //    SendReminder(user, reminder, targetForecast);
                //}
            }
        }

        private void SendReminder(WeatherReminderUser user, Reminder reminder, DailyForecast targetForecast)
        {
            StringBuilder textMessage = new StringBuilder();
            var messages = new string[3];

            var dayForecast = targetForecast.Day.PrecipitationType;
            var nightForecast = targetForecast.Night.PrecipitationType;
            var max = targetForecast.Temperature.Maximum.Value;
            var min = targetForecast.Temperature.Minimum.Value;
            var tempSetting = user.Weather.Temperature.TemperatureCutoff;
            var temperatureInRange = tempSetting <= max && tempSetting >= min;
            var userName = user.Email.Substring(0, user.Email.IndexOf('@'));

            var temperatureReminderOn = user.Weather.Temperature.IsReminder;
            var umbrellaReminderOn = user.Weather.Umbrella.IsReminder;
            var snowReminderOn = user.Weather.Snow.IsReminder;
            var canSendMessage = false;

            textMessage.Append($"{Environment.NewLine}{Environment.NewLine}---WeatherReminder---{Environment.NewLine}{Environment.NewLine}Hi {userName},");

            if (temperatureReminderOn && temperatureInRange)
            {
                messages[0] = ConstructTemperatureMessage(reminder, textMessage, user, targetForecast);
                canSendMessage = true;
            }

            if (umbrellaReminderOn && (targetForecast.Day.PrecipitationType == "Rain" || targetForecast.Night.PrecipitationType == "Rain"))
            {
                messages[1] = ConstructUmbrellaMessage(reminder, textMessage);
                canSendMessage = true;
            }

            if (snowReminderOn && (dayForecast == "Snow" || dayForecast == "Ice" || nightForecast == "Snow" || nightForecast == "Ice"))
            {
                messages[2] = ConstructSnowMessage(reminder, textMessage);
                canSendMessage = true;
            }

            if (snowReminderOn && (dayForecast == "Mixed" || nightForecast == "Mixed"))
            {
                messages[2] = ConstructMixedMessage(reminder, textMessage);
                canSendMessage = true;
            }

            textMessage.Append($"{Environment.NewLine}{Environment.NewLine}To stop receiving notifications, log in to weatherreminder.franobi.com and update your settings.Or reply STOP to unsubscribe.");

            if (user.Weather.IsEmailNotification && user.EmailConfirmed && canSendMessage)
            {
                SendEmail(user, messages, targetForecast);
            }

            if (user.Weather.IsSmsNotification && user.PhoneNumberConfirmed && canSendMessage)
            {
                _textMessage.SendText(user.CountryCode, user.PhoneNumber, textMessage.ToString());
            }
        }

        private void SendEmail(WeatherReminderUser user, string[] message, DailyForecast targetForecast)
        {
            CustomEmailModel customEmailModel = new CustomEmailModel();
            customEmailModel.TemplateId = "d-4cf634f5869946ea8826492bafbdd2e1";
            customEmailModel.Email = user.Email;
            var max = targetForecast.Temperature.Maximum.Value;
            var min = targetForecast.Temperature.Minimum.Value;
            var temperatureMessage = message[0];
            var umbrellaMessage = message[1];
            var snowMessage = message[2];
            var userName = user.Email.Substring(0, user.Email.IndexOf('@'));

            dynamic templateData = new ExpandoObject();
            templateData.Username = userName;

            templateData.UmbrellaMessage = umbrellaMessage;
            if (!string.IsNullOrWhiteSpace(umbrellaMessage))
            {
                templateData.UmbrellaMargin = "margin-top:20px";
            }

            templateData.SnowMessage = snowMessage;
            if (!string.IsNullOrWhiteSpace(snowMessage))
            {
                templateData.SnowMargin = "margin-top:20px";
            }

            templateData.TemperatureMessage = temperatureMessage;
            if (!string.IsNullOrWhiteSpace(temperatureMessage))
            {
                templateData.High = $"High: {max.ToString()}°";
                templateData.Low = $"Low: {min.ToString()}°";
                templateData.TemperatureTopMargin = "margin-top:20px";
                templateData.TemperatureBottomMargin = "margin-top:10px";
            }

            customEmailModel.TemplateData = templateData;
            _customEmail.SendEmailAsync(customEmailModel);
        }

        private static string ConstructTemperatureMessage(Reminder reminder, StringBuilder textMessage, WeatherReminderUser user, DailyForecast targetForecast)
        {
            var userName = user.Email.Substring(0, user.Email.IndexOf('@'));
            var tempSetting = user.Weather.Temperature.TemperatureCutoff;
            var max = targetForecast.Temperature.Maximum.Value;
            var min = targetForecast.Temperature.Minimum.Value;
            string temperatureMessage;
            var dayofEvent = DateTime.Now.AddDays(reminder.DaysBeforeEvent).DayOfWeek;


            if (reminder.DaysBeforeEvent == 0)
            {
                textMessage.Append($"{Environment.NewLine}{Environment.NewLine}Your temperature setting of {tempSetting}° will be reached today.");
                temperatureMessage = $"Your temperature setting of {tempSetting}° will be reached today.";
            }
            else if (reminder.DaysBeforeEvent == 1)
            {
                textMessage.Append($"{Environment.NewLine}{Environment.NewLine}Your temperature setting of {tempSetting}° will be reached tommorrow.");
                temperatureMessage = $"Your temperature setting of {tempSetting}° will be reached tommorrow.";
            }
            else
            {
                textMessage.Append($"{Environment.NewLine}{Environment.NewLine}Your temperature setting of {tempSetting}° will be reached on {dayofEvent} in {reminder.DaysBeforeEvent} days.");
                temperatureMessage = $"Your temperature setting of {tempSetting}° will be reached on {dayofEvent} in {reminder.DaysBeforeEvent} days.";
            }
            textMessage.Append($"{Environment.NewLine}High: {max}°{Environment.NewLine}Low: {min}°");
            return temperatureMessage;
        }

        private static string ConstructMixedMessage(Reminder reminder, StringBuilder textMessage)
        {
            string snowMessage;
            var dayofEvent = DateTime.Now.AddDays(reminder.DaysBeforeEvent).DayOfWeek;
            if (reminder.DaysBeforeEvent == 0)
            {
                textMessage.Append($"{Environment.NewLine}{Environment.NewLine}It may rain and snow today. Remember to pack your umbrella and snow gear.");
                snowMessage = $"It may rain and snow today. Remember to pack your umbrella and snow gear.";
            }
            else if (reminder.DaysBeforeEvent == 1)
            {
                textMessage.Append($"{Environment.NewLine}{Environment.NewLine}It may rain and snow tomorrow. Remember to pack your umbrella and snow gear.");
                snowMessage = $"It may rain and snow tomorrow. Remember to pack your umbrella and snow gear.";
            }
            else
            {
                textMessage.Append($"{Environment.NewLine}{Environment.NewLine}It may rain and snow on {dayofEvent}. Remember to pack your umbrella and snow gear.");
                snowMessage = $"It may rain and snow on {dayofEvent}. Remember to pack your umbrella and snow gear.";
            }

            return snowMessage;
        }

        private static string ConstructSnowMessage(Reminder reminder, StringBuilder textMessage)
        {
            string snowMessage;
            var dayofEvent = DateTime.Now.AddDays(reminder.DaysBeforeEvent).DayOfWeek;
            if (reminder.DaysBeforeEvent == 0)
            {
                textMessage.Append($"{Environment.NewLine}{Environment.NewLine}It may snow today. Remember to pack your snow gear.");
                snowMessage = $"It may snow today. Remember to pack your snow gear.";
            }
            else if (reminder.DaysBeforeEvent == 1)
            {
                textMessage.Append($"{Environment.NewLine}{Environment.NewLine}It may snow tomorrow. Remember to pack your snow gear.");
                snowMessage = $"It may snow tomorrow. Remember to pack your snow gear.";
            }
            else
            {
                textMessage.Append($"{Environment.NewLine}{Environment.NewLine}It may snow on {dayofEvent}. Remember to pack your snow gear.");
                snowMessage = $"It may snow on {dayofEvent}. Remember to pack your snow gear.";
            }

            return snowMessage;
        }

        private static string ConstructUmbrellaMessage(Reminder reminder, StringBuilder textMessage)
        {
            string umbrellaMessage;
            var dayofEvent = DateTime.Now.AddDays(reminder.DaysBeforeEvent).DayOfWeek;
            if (reminder.DaysBeforeEvent == 0)
            {
                textMessage.Append($"{Environment.NewLine}{Environment.NewLine}It may rain today. Remember to pack your umbrella.");
                umbrellaMessage = $"It may rain today. Remember to pack your umbrella.";
            }
            else if (reminder.DaysBeforeEvent == 1)
            {
                textMessage.Append($"{Environment.NewLine}{Environment.NewLine}It may rain tomorrow. Remember to pack your umbrella.");
                umbrellaMessage = $"It may rain tomorrow. Remember to pack your umbrella.";
            }
            else
            {
                textMessage.Append($"{Environment.NewLine}{Environment.NewLine}It may rain on {dayofEvent}. Remember to pack your umbrella.");
                umbrellaMessage = $"It may rain on {dayofEvent}. Remember to pack your umbrella.";
            }

            return umbrellaMessage;
        }

        public void DeleteUnconfirmedAccounts()
        {
            var unconfirmedUsers = _unconfirmedUserRepository.GetUsers();
            var hour = new TimeSpan(1, 0, 0);
            if (unconfirmedUsers.Count > 0)
            {
                foreach (var user in unconfirmedUsers)
                {
                    var unconfirmedUser = _weatherReminderUserRepository.GetUserByEmail(user.Email);
                    var timeActive = DateTime.Now - user.AccountCreated;
                    if (TimeSpan.Compare(timeActive, hour) >= 0 && !unconfirmedUser.EmailConfirmed)
                    {
                        _weatherReminderUserRepository.DeleteUser(unconfirmedUser);
                        _unconfirmedUserRepository.RemoveUser(user);
                    }
                }
            }
        }

        public void UpdateUsersWeatherForecast()
        {
            var users = _weatherReminderUserRepository.GetAllUsers();
            if (users != null)
            {
                foreach (var user in users)
                {
                    if (user.EmailConfirmed)
                    {
                        var userName = user.Email.Substring(0, user.Email.IndexOf('@'));
                        var forecast = _weatherForecast.GetWeatherForcast(user.Location.LocationKey).Result;
                        user.DailyForecast = new string(_weatherForecast.DailyForecastToString(forecast));
                        _userManager.UpdateAsync(user).Wait();
                    }
                }
            }
        }

        private DailyForecast[] UpdateSingleUserWeatherForecast(WeatherReminderUser user)
        {
            DailyForecast[] dailyForecast = _weatherForecast.GetWeatherForcast(user.Location.LocationKey).Result;
            user.DailyForecast = new string(_weatherForecast.DailyForecastToString(dailyForecast));
            _userManager.UpdateAsync(user).Wait();
            return dailyForecast;
        }

        //public void SendTemperatureReminders(string temperatureReminders, WeatherReminderUser user, DailyForecast[] dailyForecast)
        //{
        //    var reminders = _helperFunctions.StringToList(temperatureReminders);

        //    foreach (var reminder in reminders)
        //    {
        //        var userSetTime = reminder.ReminderTime.ToString("hh:mm tt");

        //        var targetDate = DateTime.Now.AddDays(reminder.DaysBeforeEvent);
        //        var targetForecast = Array.Find(dailyForecast, d => d.Date.Day == targetDate.Day);
        //        if (targetForecast == null)
        //        {
        //            dailyForecast = UpdateSingleUserWeatherForecast(user);
        //            targetForecast = Array.Find(dailyForecast, d => d.Date.Day == targetDate.Day);
        //        }

        //        var currentTime = DateTime.Now.Add(TimeSpan.Parse(user.TimeZoneDifference)).ToString("hh:mm tt");
        //        if (userSetTime == currentTime)
        //        {
        //            var max = targetForecast.Temperature.Maximum.Value;
        //            var min = targetForecast.Temperature.Minimum.Value;
        //            var tempSetting = user.Weather.Temperature.TemperatureCutoff;
        //            var temperatureInRange = tempSetting <= max && tempSetting >= min;
        //            var userName = user.Email.Substring(0, user.Email.IndexOf('@'));
        //            var temperatureReminderOn = user.Weather.Temperature.ReminderDetails.IsReminder;

        //            if (temperatureReminderOn && temperatureInRange)
        //            {
        //                var dayofEvent = DateTime.Now.AddDays(reminder.DaysBeforeEvent).DayOfWeek;
        //                StringBuilder message = new StringBuilder();
        //                if (reminder.DaysBeforeEvent == 0)
        //                {
        //                    message.Append($"Your temperature setting of {tempSetting}° will be reached today.");
        //                }
        //                else if (reminder.DaysBeforeEvent == 1)
        //                {
        //                    message.Append($"Your temperature setting of {tempSetting}° will be reached tommorrow.");
        //                }
        //                else
        //                {
        //                    message.Append($"Your temperature setting of {tempSetting}° will be reached on {dayofEvent} in {reminder.DaysBeforeEvent} days.");
        //                }

        //                if (user.Weather.IsEmailNotification)
        //                {
        //                    CustomEmailModel customEmailModel = new CustomEmailModel();
        //                    customEmailModel.TemplateId = "d-ef50db03fd9146e386b05e528cf1a7d5";
        //                    customEmailModel.Email = user.Email;

        //                    dynamic templateData = new ExpandoObject();
        //                    templateData.Username = userName;
        //                    templateData.Message = message.ToString();
        //                    templateData.High = max.ToString();
        //                    templateData.Low = min.ToString();
        //                    customEmailModel.TemplateData = templateData;
        //                    _customEmail.SendEmailAsync(customEmailModel);
        //                }

        //                message.Insert(0, $"{Environment.NewLine}{Environment.NewLine}---WeatherReminder---{Environment.NewLine}{Environment.NewLine}Hi {userName}, {Environment.NewLine}{Environment.NewLine}");
        //                message.Append($"{Environment.NewLine}{Environment.NewLine}High temp: {max}.{Environment.NewLine}Low temp: {min}.{Environment.NewLine}{Environment.NewLine}To stop receiving notifications, log in to weatherreminder.franobi.com and update your settings. Or reply STOP to unsubscribe.");

        //                if (user.Weather.IsSmsNotification && user.PhoneNumberConfirmed)
        //                {
        //                    _textMessage.SendText(user.CountryCode, user.PhoneNumber, message.ToString());
        //                }
        //            }
        //        }
        //    }
        //}



        //public void SendUmbrellaReminders(string umbrellaReminders, WeatherReminderUser user, DailyForecast[] dailyForecast)
        //{
        //    var reminders = _helperFunctions.StringToList(umbrellaReminders);

        //    foreach (var reminder in reminders)
        //    {
        //        var userSetTime = reminder.ReminderTime.ToString("hh:mm tt");

        //        var targetDate = DateTime.Now.AddDays(reminder.DaysBeforeEvent);
        //        var targetForecast = Array.Find(dailyForecast, d => d.Date.Day == targetDate.Day);
        //        if (targetForecast == null)
        //        {
        //            dailyForecast = UpdateSingleUserWeatherForecast(user);
        //            targetForecast = Array.Find(dailyForecast, d => d.Date.Day == targetDate.Day);
        //        }

        //        var currentTime = DateTime.Now.Add(TimeSpan.Parse(user.TimeZoneDifference)).ToString("hh:mm tt");
        //        if (userSetTime == currentTime)
        //        {
        //            var userName = user.Email.Substring(0, user.Email.IndexOf('@'));
        //            var umbrellaReminderOn = user.Weather.Umbrella.ReminderDetails.IsReminder;

        //            if (umbrellaReminderOn && (targetForecast.Day.PrecipitationType == "Rain" || targetForecast.Night.PrecipitationType == "Rain"))
        //            {
        //                var dayofEvent = DateTime.Now.AddDays(reminder.DaysBeforeEvent).DayOfWeek;
        //                StringBuilder message = new StringBuilder();
        //                StringBuilder emailMessage = new StringBuilder();
        //                if (reminder.DaysBeforeEvent == 0)
        //                {
        //                    message.Append($"It may rain today. Remember to pack your umbrella.");
        //                }
        //                else if (reminder.DaysBeforeEvent == 1)
        //                {
        //                    message.Append($"It may rain tomorrow. Remember to pack your umbrella.");
        //                }
        //                else
        //                {
        //                    message.Append($"It may rain on {dayofEvent}. Remember to pack your umbrella.");
        //                }

        //                if (user.Weather.IsEmailNotification)
        //                {
        //                    CustomEmailModel customEmailModel = new CustomEmailModel();
        //                    customEmailModel.TemplateId = "d-d1e0f0b5734347ddacf7b2832f0204fb";
        //                    customEmailModel.Email = user.Email;

        //                    dynamic templateData = new ExpandoObject();
        //                    templateData.Username = userName;
        //                    templateData.Message = message.ToString();
        //                    customEmailModel.TemplateData = templateData;
        //                    _customEmail.SendEmailAsync(customEmailModel);
        //                }

        //                message.Insert(0, $"{Environment.NewLine}{Environment.NewLine}---WeatherReminder---{Environment.NewLine}{Environment.NewLine}Hi {userName}, {Environment.NewLine}{Environment.NewLine}");
        //                message.Append($"{Environment.NewLine}{Environment.NewLine}To stop receiving notifications, log in to weatherreminder.franobi.com and update your settings. Or reply STOP to unsubscribe. Msg&Data Rates May Apply.");

        //                if (user.Weather.IsSmsNotification && user.PhoneNumberConfirmed)
        //                {
        //                    _textMessage.SendText(user.CountryCode, user.PhoneNumber, message.ToString());
        //                }
        //            }
        //        }
        //    }
        //}



        //public void SendSnowReminders(string snowReminders, WeatherReminderUser user, DailyForecast[] dailyForecast)
        //{
        //    var reminders = _helperFunctions.StringToList(snowReminders);

        //    foreach (var reminder in reminders)
        //    {
        //        var userSetTime = reminder.ReminderTime.ToString("hh:mm tt");

        //        var targetDate = DateTime.Now.AddDays(reminder.DaysBeforeEvent);
        //        var targetForecast = Array.Find(dailyForecast, d => d.Date.Day == targetDate.Day);
        //        if (targetForecast == null)
        //        {
        //            dailyForecast = UpdateSingleUserWeatherForecast(user);
        //            targetForecast = Array.Find(dailyForecast, d => d.Date.Day == targetDate.Day);
        //        }

        //        var currentTime = DateTime.Now.Add(TimeSpan.Parse(user.TimeZoneDifference)).ToString("hh:mm tt");
        //        if (userSetTime == currentTime)
        //        {
        //            var userName = user.Email.Substring(0, user.Email.IndexOf('@'));
        //            var snowReminderOn = user.Weather.Snow.ReminderDetails.IsReminder;
        //            var dayForecast = targetForecast.Day.PrecipitationType;
        //            var nightForecast = targetForecast.Night.PrecipitationType;

        //            If precipitation is snow or ice
        //            if (snowReminderOn && (dayForecast == "Snow" || dayForecast == "Ice" || nightForecast == "Snow" || nightForecast == "Ice"))
        //            {
        //                var dayofEvent = DateTime.Now.AddDays(reminder.DaysBeforeEvent).DayOfWeek;
        //                StringBuilder message = new StringBuilder();
        //                if (reminder.DaysBeforeEvent == 0)
        //                {
        //                    message.Append("It may snow today. Remember to pack your snow gear.");
        //                }
        //                else if (reminder.DaysBeforeEvent == 1)
        //                {
        //                    message.Append("It may snow tomorrow. Remember to pack your snow gear.");
        //                }
        //                else
        //                {
        //                    message.Append($"It may snow on {dayofEvent}. Remember to pack your snow gear.");
        //                }

        //                if (user.Weather.IsEmailNotification)
        //                {
        //                    CustomEmailModel customEmailModel = new CustomEmailModel();
        //                    customEmailModel.TemplateId = "d-85d82716351f4eb2b9a3a1fbaa972336";
        //                    customEmailModel.Email = user.Email;

        //                    dynamic templateData = new ExpandoObject();
        //                    templateData.Username = userName;
        //                    templateData.Message = message.ToString();
        //                    customEmailModel.TemplateData = templateData;
        //                    _customEmail.SendEmailAsync(customEmailModel);
        //                }

        //                message.Insert(0, $"{Environment.NewLine}{Environment.NewLine}---WeatherReminder---{Environment.NewLine}{Environment.NewLine}Hi {userName}, {Environment.NewLine}{Environment.NewLine}");
        //                message.Append($"{Environment.NewLine}{Environment.NewLine}To stop receiving notifications, log in to weatherreminder.franobi.com and update your settings. Or reply STOP to unsubscribe. Msg&Data Rates May Apply.");
        //                if (user.Weather.IsSmsNotification && user.PhoneNumberConfirmed)
        //                {
        //                    _textMessage.SendText(user.CountryCode, user.PhoneNumber, message.ToString());
        //                }
        //            }


        //            If precipication is mixed
        //            if (snowReminderOn && (dayForecast == "Mixed" || nightForecast == "Mixed"))
        //            {
        //                var dayofEvent = DateTime.Now.AddDays(reminder.DaysBeforeEvent).DayOfWeek;
        //                StringBuilder message = new StringBuilder();
        //                if (reminder.DaysBeforeEvent == 0)
        //                {
        //                    message.Append($"It may rain and snow today. Remember to pack your umbrella and snow gear.");
        //                }
        //                else if (reminder.DaysBeforeEvent == 1)
        //                {
        //                    message.Append($"It may rain and snow tomorrow. Remember to pack your umbrella and snow gear.");
        //                }
        //                else
        //                {
        //                    message.Append($"It may rain and snow on {dayofEvent}. Remember to pack your umbrella and snow gear.");
        //                }

        //                if (user.Weather.IsEmailNotification)
        //                {
        //                    CustomEmailModel customEmailModel = new CustomEmailModel();
        //                    customEmailModel.TemplateId = "d-85d82716351f4eb2b9a3a1fbaa972336";
        //                    customEmailModel.Email = user.Email;

        //                    dynamic templateData = new ExpandoObject();
        //                    templateData.Username = userName;
        //                    templateData.Message = message.ToString();
        //                    customEmailModel.TemplateData = templateData;
        //                    _customEmail.SendEmailAsync(customEmailModel);
        //                }

        //                message.Insert(0, $"{Environment.NewLine}{Environment.NewLine}---WeatherReminder---{Environment.NewLine}{Environment.NewLine}Hi {userName}, {Environment.NewLine}{Environment.NewLine}");
        //                message.Append($"{Environment.NewLine}{Environment.NewLine}To stop receiving notifications, log in to weatherreminder.franobi.com and update your settings. Or reply STOP to unsubscribe. Msg&Data Rates May Apply.");
        //                if (user.Weather.IsSmsNotification && user.PhoneNumberConfirmed)
        //                {
        //                    _textMessage.SendText(user.CountryCode, user.PhoneNumber, message.ToString());
        //                }
        //            }

        //        }
        //    }
        //}
    }
}
