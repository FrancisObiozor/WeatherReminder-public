using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WeatherReminder.Models.DataStorageModel;
using WeatherReminder.Models.HelpFunctionModel;
using WeatherReminder.Models.WeatherReminderModel;
using WeatherReminder.ViewModels.RemindersViewModels;

namespace WeatherReminder.Controllers
{
    public class ReminderController : Controller
    {
        private readonly IWeatherReminderUserRepository _weatherReminderUserRepository;
        private readonly IHelperFunctions _helperFunctions;
        private readonly SignInManager<WeatherReminderUser> _signInManager;
        private readonly UserManager<WeatherReminderUser> _userManager;


        public ReminderController(IWeatherReminderUserRepository weatherReminderUserRepository,
            IHelperFunctions helperFunctions,
            SignInManager<WeatherReminderUser> signInManager,
            UserManager<WeatherReminderUser> userManager)
        {
            _weatherReminderUserRepository = weatherReminderUserRepository;
            _helperFunctions = helperFunctions;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public IActionResult Edit(int id)
        {
            var user = _weatherReminderUserRepository.GetUser(User);

            var reminderViewModel = new ReminderViewModel();
            var reminder = _helperFunctions.StringToList(user.Weather.Reminders);
            reminderViewModel.DaysBeforeEvent = reminder[id].DaysBeforeEvent;
            reminderViewModel.ReminderTime = reminder[id].ReminderTime;
            reminderViewModel.Reminders = user.Weather.Reminders;
            reminderViewModel.ListNumber = id;

            return View(reminderViewModel);
        }

        [HttpPost]
        public IActionResult Edit(ReminderViewModel reminderViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = _weatherReminderUserRepository.GetUser(User);

                var reminders = _helperFunctions.StringToList(reminderViewModel.Reminders);
                reminders[reminderViewModel.ListNumber].DaysBeforeEvent = reminderViewModel.DaysBeforeEvent;
                reminders[reminderViewModel.ListNumber].ReminderTime = reminderViewModel.ReminderTime;

                var reminderString = _helperFunctions.ListToString(reminders);
                user.Weather.Reminders = reminderString;

                _weatherReminderUserRepository.SaveUser(user);

                return RedirectToAction("Reminders", "Edit");
            }
            return View(reminderViewModel);
        }

        public IActionResult Add()
        {
            var user = _weatherReminderUserRepository.GetUser(User);
            var reminderViewModel = new ReminderViewModel();
            reminderViewModel.Reminders = user.Weather.Reminders;
            return View(reminderViewModel);
        }

        [HttpPost]
        public IActionResult Add(ReminderViewModel reminderViewModel)
        {
            var reminder = new Reminder();
            reminder.ReminderTime = reminderViewModel.ReminderTime;
            reminder.DaysBeforeEvent = reminderViewModel.DaysBeforeEvent;
            var reminders = _helperFunctions.StringToList(reminderViewModel.Reminders);
            reminders.Add(reminder);
            var reminderString = _helperFunctions.ListToString(reminders);

            var user = _weatherReminderUserRepository.GetUser(User);
            user.Weather.Reminders = reminderString;

            if (ModelState.IsValid)
            {
                _weatherReminderUserRepository.SaveUser(user);
                return RedirectToAction("Reminders", "Edit");
            }

            return View(reminderViewModel);
        }



        //public IActionResult DisplayTemperature()
        //{
        //    var temperature = _weatherReminderUserRepository.GetTemperature(User);
        //    var reminders = _helperFunctions.StringToList(temperature.ReminderDetails.Reminders);

        //    var display = new DisplayRemindersViewModel
        //    {
        //        Reminders = reminders
        //    };
        //    return View(display);
        //}

        //public IActionResult DisplayUmbrella()
        //{
        //    var umbrella = _weatherReminderUserRepository.GetUser(User).Weather.Umbrella;
        //    var reminders = _helperFunctions.StringToList(umbrella.ReminderDetails.Reminders);

        //    var display = new DisplayRemindersViewModel
        //    {
        //        Reminders = reminders
        //    };
        //    return View(display);
        //}

        //public IActionResult DisplaySnow()
        //{
        //    var snow = _weatherReminderUserRepository.GetUser(User).Weather.Snow;
        //    var reminders = _helperFunctions.StringToList(snow.ReminderDetails.Reminders);

        //    var display = new DisplayRemindersViewModel
        //    {
        //        Reminders = reminders
        //    };
        //    return View(display);
        //}


        //public IActionResult EditTemperature(int id, bool isReminder, int temperatureCutoff)
        //{
        //    ClaimsPrincipal currentUser = this.User;
        //    var temperature = _weatherReminderUserRepository.GetTemperature(currentUser);

        //    if (isReminder != temperature.ReminderDetails.IsReminder || temperatureCutoff != temperature.TemperatureCutoff)
        //    {
        //        temperature.ReminderDetails.IsReminder = isReminder;
        //        temperature.TemperatureCutoff = temperatureCutoff;
        //        _weatherReminderUserRepository.SaveTemperature(temperature, currentUser);
        //    }

        //    var reminderViewModel = new ReminderViewModel();
        //    var reminder = _helperFunctions.StringToList(temperature.ReminderDetails.Reminders);
        //    reminderViewModel.DaysBeforeEvent = reminder[id].DaysBeforeEvent; 
        //    reminderViewModel.ReminderTime = reminder[id].ReminderTime;
        //    reminderViewModel.Reminders = temperature.ReminderDetails.Reminders;
        //    reminderViewModel.ListNumber = id;

        //    return View(reminderViewModel);
        //}

        //[HttpPost]
        //public IActionResult EditTemperature(ReminderViewModel reminderViewModel)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var temperature = _weatherReminderUserRepository.GetTemperature(User);

        //        var reminders = _helperFunctions.StringToList(reminderViewModel.Reminders);
        //        reminders[reminderViewModel.ListNumber].DaysBeforeEvent = reminderViewModel.DaysBeforeEvent;
        //        reminders[reminderViewModel.ListNumber].ReminderTime = reminderViewModel.ReminderTime;

        //        var reminderString = _helperFunctions.ListToString(reminders);
        //        temperature.ReminderDetails.Reminders = reminderString;

        //        _weatherReminderUserRepository.SaveTemperature(temperature, User);

        //        return RedirectToAction("Temperature", "Edit");
        //    }
        //    return View(reminderViewModel);
        //}

        //public IActionResult AddTemperature(bool isReminder, int tempCutoff)
        //{
        //    ClaimsPrincipal currentUser = this.User;
        //    var temperature = _weatherReminderUserRepository.GetTemperature(currentUser);

        //    if ( isReminder != temperature.ReminderDetails.IsReminder || tempCutoff != temperature.TemperatureCutoff)
        //    {
        //        temperature.ReminderDetails.IsReminder = isReminder;
        //        temperature.TemperatureCutoff = tempCutoff;
        //        _weatherReminderUserRepository.SaveTemperature(temperature, currentUser);
        //    }

        //    var reminderViewModel = new ReminderViewModel();
        //    reminderViewModel.Reminders = temperature.ReminderDetails.Reminders;
        //    ViewBag.returnUrl = Request.Headers["Referer"].ToString();
        //    return View(reminderViewModel);
        //}

        //[HttpPost]
        //public IActionResult AddTemperature(ReminderViewModel reminderViewModel, string returnUrl)
        //{
        //    var reminder = new Reminder();
        //    reminder.ReminderTime = reminderViewModel.ReminderTime;
        //    reminder.DaysBeforeEvent = reminderViewModel.DaysBeforeEvent;
        //    var reminders = _helperFunctions.StringToList(reminderViewModel.Reminders);
        //    reminders.Add(reminder);
        //    var reminderString = _helperFunctions.ListToString(reminders);

        //    var temperature = _weatherReminderUserRepository.GetTemperature(User);
        //    temperature.ReminderDetails.Reminders = reminderString;

        //    if (ModelState.IsValid)
        //    {
        //        _weatherReminderUserRepository.SaveTemperature(temperature, User);
        //        //return Redirect(returnUrl);
        //        return RedirectToAction("Temperature", "Edit");
        //    }

        //    return View(reminderViewModel);
        //}


        //public IActionResult EditUmbrella(int id, bool isReminder)
        //{
        //    var user = _weatherReminderUserRepository.GetUser(User);

        //    if (isReminder != user.Weather.Umbrella.ReminderDetails.IsReminder)
        //    {
        //        user.Weather.Umbrella.ReminderDetails.IsReminder = isReminder;
        //        _weatherReminderUserRepository.SaveUser(user);
        //    }

        //    var umbrella = user.Weather.Umbrella;
        //    var reminderViewModel = new ReminderViewModel();
        //    var reminder = _helperFunctions.StringToList(umbrella.ReminderDetails.Reminders);
        //    reminderViewModel.DaysBeforeEvent = reminder[id].DaysBeforeEvent;
        //    reminderViewModel.ReminderTime = reminder[id].ReminderTime;
        //    reminderViewModel.Reminders = umbrella.ReminderDetails.Reminders;
        //    reminderViewModel.ListNumber = id;

        //    return View(reminderViewModel);
        //}

        //[HttpPost]
        //public IActionResult EditUmbrella(ReminderViewModel reminderViewModel)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = _weatherReminderUserRepository.GetUser(User);

        //        var reminders = _helperFunctions.StringToList(reminderViewModel.Reminders);
        //        reminders[reminderViewModel.ListNumber].DaysBeforeEvent = reminderViewModel.DaysBeforeEvent;
        //        reminders[reminderViewModel.ListNumber].ReminderTime = reminderViewModel.ReminderTime;

        //        var reminderString = _helperFunctions.ListToString(reminders);
        //        user.Weather.Umbrella.ReminderDetails.Reminders = reminderString;

        //        _weatherReminderUserRepository.SaveUser(user);

        //        return RedirectToAction("Umbrella", "Edit");
        //    }
        //    return View(reminderViewModel);
        //}

        //public IActionResult AddUmbrella(bool? isReminder)
        //{
        //    var user = _weatherReminderUserRepository.GetUser(User);

        //    if (isReminder != null && isReminder != user.Weather.Umbrella.ReminderDetails.IsReminder)
        //    {
        //        user.Weather.Umbrella.ReminderDetails.IsReminder = (bool)isReminder;
        //        _weatherReminderUserRepository.SaveUser(user);
        //    }

        //    var umbrella = user.Weather.Umbrella;
        //    var reminderViewModel = new ReminderViewModel();
        //    reminderViewModel.Reminders = umbrella.ReminderDetails.Reminders;
        //    ViewBag.returnUrl = Request.Headers["Referer"].ToString();
        //    return View(reminderViewModel);
        //}

        //[HttpPost]
        //public IActionResult AddUmbrella(ReminderViewModel reminderViewModel, string returnUrl)
        //{
        //    var reminder = new Reminder();
        //    reminder.ReminderTime = reminderViewModel.ReminderTime;
        //    reminder.DaysBeforeEvent = reminderViewModel.DaysBeforeEvent;
        //    var reminders = _helperFunctions.StringToList(reminderViewModel.Reminders);
        //    reminders.Add(reminder);
        //    var reminderString = _helperFunctions.ListToString(reminders);

        //    var user = _weatherReminderUserRepository.GetUser(User);
        //    user.Weather.Umbrella.ReminderDetails.Reminders = reminderString;

        //    if (ModelState.IsValid)
        //    {
        //        _weatherReminderUserRepository.SaveUser(user);
        //        return RedirectToAction("Umbrella", "Edit");

        //    }

        //    return View(reminderViewModel);
        //}



        //public IActionResult EditSnow(int id, bool isReminder)
        //{
        //    var user = _weatherReminderUserRepository.GetUser(User);

        //    if (isReminder != user.Weather.Snow.ReminderDetails.IsReminder)
        //    {
        //        user.Weather.Snow.ReminderDetails.IsReminder = isReminder;
        //        _weatherReminderUserRepository.SaveUser(user);
        //    }

        //    var snow = user.Weather.Snow;
        //    var reminderViewModel = new ReminderViewModel();
        //    var reminder = _helperFunctions.StringToList(snow.ReminderDetails.Reminders);
        //    reminderViewModel.DaysBeforeEvent = reminder[id].DaysBeforeEvent;
        //    reminderViewModel.ReminderTime = reminder[id].ReminderTime;
        //    reminderViewModel.Reminders = snow.ReminderDetails.Reminders;
        //    reminderViewModel.ListNumber = id;

        //    return View(reminderViewModel);
        //}

        //[HttpPost]
        //public IActionResult EditSnow(ReminderViewModel reminderViewModel)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = _weatherReminderUserRepository.GetUser(User);

        //        var reminders = _helperFunctions.StringToList(reminderViewModel.Reminders);
        //        reminders[reminderViewModel.ListNumber].DaysBeforeEvent = reminderViewModel.DaysBeforeEvent;
        //        reminders[reminderViewModel.ListNumber].ReminderTime = reminderViewModel.ReminderTime;

        //        var reminderString = _helperFunctions.ListToString(reminders);
        //        user.Weather.Snow.ReminderDetails.Reminders = reminderString;

        //        _weatherReminderUserRepository.SaveUser(user);

        //        return RedirectToAction("Snow", "Edit");
        //    }
        //    return View(reminderViewModel);
        //}

        //public IActionResult AddSnow(bool? isReminder)
        //{
        //    var user = _weatherReminderUserRepository.GetUser(User);

        //    if (isReminder != null && isReminder != user.Weather.Snow.ReminderDetails.IsReminder)
        //    {
        //        user.Weather.Snow.ReminderDetails.IsReminder = (bool)isReminder;
        //        _weatherReminderUserRepository.SaveUser(user);
        //    }

        //    var snow = user.Weather.Snow;
        //    var reminderViewModel = new ReminderViewModel();
        //    reminderViewModel.Reminders = snow.ReminderDetails.Reminders;
        //    ViewBag.returnUrl = Request.Headers["Referer"].ToString();
        //    return View(reminderViewModel);
        //}

        //[HttpPost]
        //public IActionResult AddSnow(ReminderViewModel reminderViewModel, string returnUrl)
        //{
        //    var reminder = new Reminder();
        //    reminder.ReminderTime = reminderViewModel.ReminderTime;
        //    reminder.DaysBeforeEvent = reminderViewModel.DaysBeforeEvent;
        //    var reminders = _helperFunctions.StringToList(reminderViewModel.Reminders);
        //    reminders.Add(reminder);
        //    var reminderString = _helperFunctions.ListToString(reminders);

        //    var user = _weatherReminderUserRepository.GetUser(User);
        //    user.Weather.Snow.ReminderDetails.Reminders = reminderString;

        //    if (ModelState.IsValid)
        //    {
        //        _weatherReminderUserRepository.SaveUser(user);
        //        return RedirectToAction("Snow", "Edit");

        //    }

        //    return View(reminderViewModel);
        //}
    }
}
