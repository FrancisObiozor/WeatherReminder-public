using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using WeatherReminder.Models;
using WeatherReminder.Models.ApiModel.AutoCompleteApi;
using WeatherReminder.Models.ApiModel.GeocodeApi;
using WeatherReminder.Models.ApiModel.IpAddressGeocodeApi;
using WeatherReminder.Models.ApiModel.WeatherForecastApi.LocationApi;
using WeatherReminder.Models.DataStorageModel;
using WeatherReminder.Models.PositionModel;
using WeatherReminder.Models.WeatherReminderModel;

namespace WeatherReminder.Areas.Identity.Pages.Profile.Home
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<WeatherReminderUser> _userManager;
        private readonly SignInManager<WeatherReminderUser> _signInManager;
        private readonly IWeatherReminderUserRepository _weatherReminderUserRepository;
        private readonly IGeocodeLocation _geocodeLocation;
        private readonly IAutoComplete _autoComplete;
        private readonly ILocationKey _locationKey;
        private readonly IIpAddressGeocode _ipAddressGeocode;

        public IndexModel(
            UserManager<WeatherReminderUser> userManager,
            SignInManager<WeatherReminderUser> signInManager,
            IWeatherReminderUserRepository weatherReminderUserRepository,
            IGeocodeLocation geocodeLocation,
            IAutoComplete autoComplete,
            ILocationKey locationKey,
            IIpAddressGeocode ipAddressGeocode
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _weatherReminderUserRepository = weatherReminderUserRepository;
            _geocodeLocation = geocodeLocation;
            _autoComplete = autoComplete;
            _locationKey = locationKey;
            _ipAddressGeocode = ipAddressGeocode;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        [TempData]
        public string StatusMessage { get; set; }


        public string IpCity { get; set; }
        public string IpState { get; set; }
        public string IpCountry { get; set; }


        public List<string> Options { get; set; }
        public bool IsLocationMatch { get; set; } = true;
        public bool OptionsExist { get; set; } = true;
        public bool? FoundCurrentLocation { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "City")]
            public string City { get; set; }

            [Required]
            [Display(Name = "State/Province/Region")]
            public string State { get; set; }

            [Required]
            [Display(Name = "Country")]
            public string Country { get; set; }

            [Display(Name = "Use Current Location")]
            public bool PullLocationFromIp { get; set; }

            [Display(Name = "Text Message Notifications")]
            public bool TextMessageNotifications { get; set; }

            [Display(Name = "Email Notifications")]
            public bool EmailNotifications { get; set; }

        }

        private async Task LoadAsync(WeatherReminderUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Input = new InputModel
            {
                City = user.Location.City,
                State = user.Location.State,
                Country = user.Location.Country,
                TextMessageNotifications = user.Weather.IsSmsNotification,
                EmailNotifications = user.Weather.IsEmailNotification,
                PullLocationFromIp = user.Location.PullLocationFromIp
            };

        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = _weatherReminderUserRepository.GetUser(User);

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var userLocation = _ipAddressGeocode.GetLocationFromIp().Result.Location;
            if (userLocation != null)
            {
                IpCity = userLocation.City;
                IpState = userLocation.Region_name;
                IpCountry = userLocation.Country_name;
                FoundCurrentLocation = true;
            }
            else
            {
                FoundCurrentLocation = false;
            }

            await LoadAsync(user);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = _weatherReminderUserRepository.GetUser(User);

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            if (ModelState.IsValid)
            {
                //If profile changes
                if (Input.City != user.Location.City ||
                    Input.State != user.Location.State ||
                    Input.Country != user.Location.Country)
                {
                    //Verify Location
                    var cityStateCountry = new CityStateCountry();
                    cityStateCountry.City = Input.City;
                    cityStateCountry.State = Input.State;
                    cityStateCountry.Country = Input.Country;

                    IsLocationMatch = _geocodeLocation.VerifyLocationMatches(cityStateCountry).Result;
                    if (!IsLocationMatch)
                    {
                        Options = _autoComplete.AutoCompleteOptions(cityStateCountry).Result;
                        if (Options == null)
                        {
                            OptionsExist = false;
                        }
                        return Page();
                    }
                    var latLong = _geocodeLocation.GetLatitudeLongitude(cityStateCountry).Result;
                    var locationKey = _locationKey.GetLocationKey(latLong).Result;

                    user.Location.Latitude = latLong.Latitude;
                    user.Location.Longitude = latLong.Longitude;
                    user.Location.LocationKey = locationKey;

                    StatusMessage = "Your profile has been updated";
                }

                //City
                if (Input.City != user.Location.City)
                {
                    user.Location.City = Input.City;
                    StatusMessage = "Your profile has been updated";
                }

                //State
                if (Input.State != user.Location.State)
                {
                    user.Location.State = Input.State;
                    StatusMessage = "Your profile has been updated";
                }

                //Country
                if (Input.Country != user.Location.Country)
                {
                    user.Location.Country = Input.Country;
                    StatusMessage = "Your profile has been updated";
                }

                //Pull location from IP
                if (Input.PullLocationFromIp != user.Location.PullLocationFromIp)
                {
                    user.Location.PullLocationFromIp = Input.PullLocationFromIp;
                    StatusMessage = "Your profile has been updated";
                }

                //Text Notifications
                if (Input.TextMessageNotifications != user.Weather.IsSmsNotification)
                {
                    user.Weather.IsSmsNotification = Input.TextMessageNotifications;
                    StatusMessage = "Your profile has been updated";
                }

                //Email Notifications
                if (Input.EmailNotifications != user.Weather.IsEmailNotification)
                {
                    user.Weather.IsEmailNotification = Input.EmailNotifications;
                    StatusMessage = "Your profile has been updated";
                }

                _userManager.UpdateAsync(user).Wait();

                _signInManager.RefreshSignInAsync(user).Wait();
            }

            return RedirectToPage();
        }

    }
}

//Old code for reference - Use to verify cell phone in profile tab. Move cell updates to another tab
//[TempData]
//public string Phone { get; set; }

//[TempData]
//public int PageVisits { get; set; }
//public bool InvalidPhoneNumberApiError { get; set; }
//public bool MaxSendAttemptsApiError { get; set; }
//public bool PhoneVerificationFailed { get; set; }
//public bool PhoneVerificationSuccess { get; set; }

//if (!_cellVerificationStatus.HasCellBeenVerified)
//{
//    Phone = phoneNumber;
//}
//else
//{
//    Phone = _cellVerificationStatus.CellNumber;
//}

//if (!_cellVerificationStatus.HasCellBeenVerified && PageVisits > 0)
//{
//    PhoneVerificationFailed = true;
//    PageVisits = 0;
//    _cellVerificationStatus.VerificationAttempts = 0;
//}

//if (_cellVerificationStatus.HasCellBeenVerified == true)
//{
//    PhoneVerificationSuccess = true;
//    _cellVerificationStatus.HasCellBeenVerified = false;
//    StatusMessage = "Your profile has been updated";
//}

/////////////////////////////////////
//if (HasCodeBeenSent == true)
//{
//    var cellIsValid = _verifyCell.IsNumberValid(Phone, Input.VerificationCode);
//    if (cellIsValid == true)
//    {
//        var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Phone);
//        if (!setPhoneResult.Succeeded)
//        {
//            StatusMessage = "Unexpected error when trying to set phone number.";
//            return RedirectToPage();
//        }
//    }
//}

////////////////////////////////////
//var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

/////////////////////////////////////*******************
//if (Input.PhoneNumber != phoneNumber)
//{
//    try
//    {
//        _verifyCell.SendVerificationCode(Input.PhoneNumber);
//    }
//    catch (ApiException e)
//    {
//        if (e.Message == "Max send attempts reached")
//        {
//            MaxSendAttemptsApiError = true;
//            //You have attempted to send the verification code more than 5 times and have reached the limit. You have to wait 10 minutes for verification to expire.
//        }

//        if (e.Message.Contains("Invalid parameter `To`:"))
//        {
//            InvalidPhoneNumberApiError = true;
//        }

//        await LoadAsync(user);
//        return Page();
//    }

//    Phone = Input.PhoneNumber;
//    _cellVerificationStatus.CellNumber = Input.PhoneNumber;
//    PageVisits++;
//    return RedirectToPage("EnterVerificationCode");
//}