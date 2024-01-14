using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WeatherReminder.Models.DataStorageModel;
using WeatherReminder.Models.HelpFunctionModel;
using WeatherReminder.ViewModels.RemindersViewModels;

namespace WeatherReminder.Controllers
{
    public class EditController : Controller
    {

        private readonly IWeatherReminderUserRepository _weatherReminderUserRepository;
        private readonly IHelperFunctions _helperFunctions;

        public EditController(IWeatherReminderUserRepository weatherReminderUserRepository,
            IHelperFunctions helperFunctions
            )
        {
            _weatherReminderUserRepository = weatherReminderUserRepository;
            _helperFunctions = helperFunctions;
        }

        public IActionResult Reminders(int? id, 
                                        bool? currentIsUmbrellaOn, 
                                        bool? currentIsSnowOn, 
                                        bool? currentIsTemperatureOn, 
                                        int? currentTemperatureCutoff)
        {
            ClaimsPrincipal currentUser = this.User;
            var user = _weatherReminderUserRepository.GetUser(currentUser);
            var reminders = user.Weather.Reminders;
            var edit = new EditRemindersViewModel();

            var existingIsUmbrellaOn = user.Weather.Umbrella.IsReminder;
            var existingIsSnowOn = user.Weather.Snow.IsReminder;
            var existingIsTemperatureOn = user.Weather.Temperature.IsReminder;
            var existingTemperatureCutoff = user.Weather.Temperature.TemperatureCutoff;
            
            if ((currentIsUmbrellaOn != null && currentIsUmbrellaOn != existingIsUmbrellaOn) || 
                (currentIsSnowOn != null && currentIsSnowOn != existingIsSnowOn) || 
                (currentIsTemperatureOn != null && currentIsTemperatureOn  != existingIsTemperatureOn) || 
                (currentTemperatureCutoff != null && currentTemperatureCutoff != existingTemperatureCutoff))
            {
                user.Weather.Umbrella.IsReminder = (bool)currentIsUmbrellaOn;
                user.Weather.Snow.IsReminder = (bool)currentIsSnowOn;
                user.Weather.Temperature.IsReminder = (bool)currentIsTemperatureOn;
                user.Weather.Temperature.TemperatureCutoff = (int)currentTemperatureCutoff;
                _weatherReminderUserRepository.SaveUser(user);
            }

            if (id != null)
            {
                int index = (int)id;
                var reminderList = _helperFunctions.StringToList(reminders);
                reminderList.RemoveAt(index);
                edit.RemindersList = reminderList;
                edit.RemindersString = _helperFunctions.ListToString(edit.RemindersList);
                user.Weather.Reminders = edit.RemindersString;
                _weatherReminderUserRepository.SaveUser(user);
            }
            else
            {
                edit.RemindersList = _helperFunctions.StringToList(reminders);
                edit.RemindersString = reminders;
            }

            return View(edit);
        }

        [HttpPost]
        public IActionResult Reminders(EditRemindersViewModel editReminders)
        {
            var user = _weatherReminderUserRepository.GetUser(User);
            user.Weather.Reminders = editReminders.RemindersString;

            if (ModelState.IsValid)
            {
                _weatherReminderUserRepository.SaveUser(user);
                return RedirectToAction("Index", "Home");
            }

            return View(editReminders);
        }


        //public IActionResult Temperature(int? id, bool? currentIsReminder, int? currentTempCutoff)
        //{
        //    ClaimsPrincipal currentUser = this.User;
        //    var user = _weatherReminderUserRepository.GetUser(currentUser);
        //    var reminders = user.Weather.Temperature.ReminderDetails.Reminders;
        //    var edit = new EditTemperatureViewModel();

        //    var existingIsReminder = user.Weather.Temperature.ReminderDetails.IsReminder;
        //    var existingTempCutoff = user.Weather.Temperature.TemperatureCutoff;
        //    if ((currentIsReminder != null && currentIsReminder != existingIsReminder) || (currentTempCutoff != null && currentTempCutoff != existingTempCutoff))
        //    {
        //        user.Weather.Temperature.ReminderDetails.IsReminder = (bool)currentIsReminder;
        //        user.Weather.Temperature.TemperatureCutoff = (int)currentTempCutoff;
        //        _weatherReminderUserRepository.SaveUser(user);
        //    }

        //    if (id != null)
        //    {
        //        int index = (int)id;
        //        var reminderList = _helperFunctions.StringToList(reminders);
        //        reminderList.RemoveAt(index);
        //        edit.RemindersList = reminderList;
        //        edit.RemindersString = _helperFunctions.ListToString(edit.RemindersList);
        //        user.Weather.Temperature.ReminderDetails.Reminders = edit.RemindersString;
        //        _weatherReminderUserRepository.SaveUser(user);
        //    }
        //    else
        //    {
        //        edit.RemindersList = _helperFunctions.StringToList(reminders);
        //        edit.RemindersString = reminders;
        //    }

        //    edit.TemperatureCutoff = user.Weather.Temperature.TemperatureCutoff;
        //    edit.IsReminder = user.Weather.Temperature.ReminderDetails.IsReminder;

        //    return View(edit);
        //}

        //[HttpPost]
        //public IActionResult Temperature(EditTemperatureViewModel editTemp)
        //{
        //    var temperature = _weatherReminderUserRepository.GetTemperature(User);
        //    temperature.TemperatureCutoff = editTemp.TemperatureCutoff;
        //    temperature.ReminderDetails.IsReminder = editTemp.IsReminder;
        //    temperature.ReminderDetails.Reminders = editTemp.RemindersString;

        //    if (ModelState.IsValid)
        //    {
        //        _weatherReminderUserRepository.SaveTemperature(temperature, User);
        //        return RedirectToAction("Index", "Home");
        //    }

        //    return View(editTemp);
        //}



        //public IActionResult Umbrella(int? id, bool? currentIsReminder)
        //{
        //    var user = _weatherReminderUserRepository.GetUser(User);
        //    var reminders = user.Weather.Umbrella.ReminderDetails.Reminders;
        //    var edit = new EditUmbrellaViewModel();

        //    var existingIsReminder = user.Weather.Umbrella.ReminderDetails.IsReminder;

        //    if (currentIsReminder != null && currentIsReminder != existingIsReminder)
        //    {
        //        user.Weather.Umbrella.ReminderDetails.IsReminder = (bool)currentIsReminder;
        //        _weatherReminderUserRepository.SaveUser(user);
        //    }

        //    if (id != null)
        //    {
        //        int index = (int)id;
        //        var reminderList = _helperFunctions.StringToList(reminders);
        //        reminderList.RemoveAt(index);

        //        edit.RemindersList = reminderList;
        //        edit.RemindersString = _helperFunctions.ListToString(edit.RemindersList);

        //        user.Weather.Umbrella.ReminderDetails.Reminders = edit.RemindersString;
        //        _weatherReminderUserRepository.SaveUser(user);
        //    }
        //    else
        //    {
        //        edit.RemindersList = _helperFunctions.StringToList(reminders);
        //        edit.RemindersString = reminders;
        //    }

        //    edit.IsReminder = user.Weather.Umbrella.ReminderDetails.IsReminder;

        //    return View(edit);
        //}


        //[HttpPost]
        //public IActionResult Umbrella(EditUmbrellaViewModel editUmbrella)
        //{
        //    var user = _weatherReminderUserRepository.GetUser(User);
        //    user.Weather.Umbrella.ReminderDetails.IsReminder = editUmbrella.IsReminder;
        //    user.Weather.Umbrella.ReminderDetails.Reminders = editUmbrella.RemindersString;

        //    if (ModelState.IsValid)
        //    {
        //        _weatherReminderUserRepository.SaveUser(user);
        //        return RedirectToAction("Index", "Home");
        //    }

        //    return View(editUmbrella);
        //}




        //public IActionResult Snow(int? id, bool? currentIsReminder)
        //{
        //    var user = _weatherReminderUserRepository.GetUser(User);
        //    var reminders = user.Weather.Snow.ReminderDetails.Reminders;
        //    var edit = new EditSnowViewModel();

        //    var existingIsReminder = user.Weather.Snow.ReminderDetails.IsReminder;

        //    if (currentIsReminder != null && currentIsReminder != existingIsReminder)
        //    {
        //        user.Weather.Snow.ReminderDetails.IsReminder = (bool)currentIsReminder;
        //        _weatherReminderUserRepository.SaveUser(user);
        //    }

        //    if (id != null)
        //    {
        //        int index = (int)id;
        //        var reminderList = _helperFunctions.StringToList(reminders);
        //        reminderList.RemoveAt(index);

        //        edit.RemindersList = reminderList;
        //        edit.RemindersString = _helperFunctions.ListToString(edit.RemindersList);

        //        user.Weather.Snow.ReminderDetails.Reminders = edit.RemindersString;
        //        _weatherReminderUserRepository.SaveUser(user);
        //    }
        //    else
        //    {
        //        edit.RemindersList = _helperFunctions.StringToList(reminders);
        //        edit.RemindersString = reminders;
        //    }

        //    edit.IsReminder = user.Weather.Snow.ReminderDetails.IsReminder;

        //    return View(edit);
        //}


        //[HttpPost]
        //public IActionResult Snow(EditSnowViewModel editSnow)
        //{
        //    var user = _weatherReminderUserRepository.GetUser(User);
        //    user.Weather.Snow.ReminderDetails.IsReminder = editSnow.IsReminder;
        //    user.Weather.Snow.ReminderDetails.Reminders = editSnow.RemindersString;

        //    if (ModelState.IsValid)
        //    {
        //        _weatherReminderUserRepository.SaveUser(user);
        //        return RedirectToAction("Index", "Home");
        //    }

        //    return View(editSnow);
        //}
    }
}

