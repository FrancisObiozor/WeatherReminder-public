using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using WeatherReminder.Models.ApiModel.AutoCompleteApi;
using WeatherReminder.Models.ApiModel.EmailApi;
using WeatherReminder.Models.ApiModel.GeocodeApi;
using WeatherReminder.Models.ApiModel.IpAddressGeocodeApi;
using WeatherReminder.Models.ApiModel.LocationFromBrowserApi;
using WeatherReminder.Models.ApiModel.TextMessageApi.VerifyCellApi;
using WeatherReminder.Models.ApiModel.WeatherForecastApi.ForecastApi;
using WeatherReminder.Models.ApiModel.WeatherForecastApi.LocationApi;
using WeatherReminder.Models.PositionModel;
using WeatherReminder.Models.UnconfirmedUsers;
using WeatherReminder.Models.WeatherReminderModel;

namespace WeatherReminder.Areas.Identity.Pages.RegisterUser
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<WeatherReminderUser> _signInManager;
        private readonly UserManager<WeatherReminderUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IIpAddressGeocode _ipAddressGeocode;
        private readonly IGeocodeLocation _geocodeLocation;
        private readonly ILocationKey _locationKey;
        private readonly IAutoComplete _autoComplete;
        private readonly IVerifyCell _verifyCell;
        private readonly IWeatherForecast _weatherForecast;
        private readonly ICustomEmail _customEmail;
        private readonly IUnconfirmedUserRepository _unconfirmedUserRepository;
        private readonly ILocationFromBrowser _locationFromBrowser;

        public RegisterModel(
            UserManager<WeatherReminderUser> userManager,
            SignInManager<WeatherReminderUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            IIpAddressGeocode ipAddressGeocode,
            IGeocodeLocation geocodeLocation,
            ILocationKey locationKey,
            IAutoComplete autoComplete,
            IVerifyCell verifyCell,
            IWeatherForecast weatherForecast,
            ICustomEmail customEmail,
            IUnconfirmedUserRepository unconfirmedUserRepository,
            ILocationFromBrowser locationFromBrowser
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _ipAddressGeocode = ipAddressGeocode;
            _geocodeLocation = geocodeLocation;
            _locationKey = locationKey;
            _autoComplete = autoComplete;
            _verifyCell = verifyCell;
            _weatherForecast = weatherForecast;
            _customEmail = customEmail;
            _unconfirmedUserRepository = unconfirmedUserRepository;
            _locationFromBrowser = locationFromBrowser;
        }

        [BindProperty]
        public InputModel Input { get; set; }
        public List<string> Options { get; set; }
        public bool IsLocationMatch { get; set; } = true;
        public bool IsAnAutoCompleteOption { get; set; }
        public bool OptionsExist { get; set; } = true;
        public string ReturnUrl { get; set; }

        public bool? FoundCurrentLocation { get; set; }
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        [TempData]
        public string Phone { get; set; }

        [TempData]
        public int PageVisits { get; set; }

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

            [Phone]
            [Display(Name = "Country Code")]
            public string CountryCode { get; set; }

            [Phone]
            [Display(Name = "Phone Number")]
            public string PhoneNumber { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            public string UserTime { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            Input = new InputModel();
            Input.CountryCode = "1";

            var locationFromBrowser = _locationFromBrowser.CityStateCountry;
            var isLocationFromBrowser = !(locationFromBrowser == null || object.ReferenceEquals(locationFromBrowser, new object()));

            if (isLocationFromBrowser)
            {
                Input.City = locationFromBrowser.City;
                Input.State = locationFromBrowser.State;
                Input.Country = locationFromBrowser.Country;
            }
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            //Verify Location
            var cityStateCountry = new CityStateCountry();
            cityStateCountry.City = Input.City;
            cityStateCountry.State = Input.State;
            cityStateCountry.Country = Input.Country;

            IsAnAutoCompleteOption = _autoComplete.IsAnAutoCompleteOption(cityStateCountry).Result;
            if (!IsAnAutoCompleteOption)
            {
                ShowUserAutoCompleteOptions(cityStateCountry);
                //////
                return Page();
            }
            /////////////////////

            returnUrl = returnUrl ?? Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                WeatherReminderUser user = ConstructUser(cityStateCountry);
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);

                    //Record all new accounts. New accounts will be delete after an hour if not verified
                    var unconfirmedUser = new UnconfirmedUser()
                    {
                        Email = user.Email,
                        AccountCreated = DateTime.Now
                    };
                    _unconfirmedUserRepository.AddUser(unconfirmedUser);

                    _logger.LogInformation("User created a new account with password.");

                    CustomEmailModel customEmailModel = await ConstructConfirmationEmail(returnUrl, user);
                    _customEmail.SendEmailAsync(customEmailModel);


                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl });
                    }
                    //else
                    //{
                    //    await _signInManager.SignInAsync(user, isPersistent: false);
                    //    //return LocalRedirect(returnUrl);
                    //    return RedirectToAction("Index", "Home");
                    //}
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private async Task<CustomEmailModel> ConstructConfirmationEmail(string returnUrl, WeatherReminderUser user)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            var callbackUrl = Url.Page(
                "Profile/EmailUser/ConfirmEmail",
                pageHandler: null,
                values: new { area = "Identity", userId = user.Id, code, returnUrl },
                protocol: Request.Scheme);

            var customEmailModel = new CustomEmailModel();
            customEmailModel.Email = Input.Email;
            customEmailModel.TemplateId = "d-fa93f10b438d4bb8a17bb0054a9464a2";
            customEmailModel.TemplateData = new
            {
                ChangeEmail = callbackUrl
            };

            return customEmailModel;
        }

        private void ShowUserAutoCompleteOptions(CityStateCountry cityStateCountry)
        {
            Options = _autoComplete.AutoCompleteOptions(cityStateCountry).Result;
            if (Options == null)
            {
                OptionsExist = false;
            }
        }

        private WeatherReminderUser ConstructUser(CityStateCountry cityStateCountry)
        {
            var latLong = _geocodeLocation.GetLatitudeLongitude(cityStateCountry).Result;
            var locationKey = _locationKey.GetLocationKey(latLong).Result;
            var weatherForecast = _weatherForecast.GetWeatherForcast(locationKey).Result;

            //Convert weather forecast array to an XML string
            StringWriter writer = new StringWriter();
            XmlSerializer serializer = new XmlSerializer(typeof(DailyForecast[]));
            serializer.Serialize(writer, weatherForecast);
            var dailyForecast = writer.ToString();

            var now = DateTime.Now;
            var userTime = DateTime.Parse(Input.UserTime);
            var timeZoneDifference = -(now - userTime);

            var user = new WeatherReminderUser
            {
                Location = new UserLocation
                {
                    City = Input.City,
                    State = Input.State,
                    Country = Input.Country,
                    Latitude = latLong.Latitude,
                    Longitude = latLong.Longitude,
                    LocationKey = locationKey,
                },

                Weather = new WeatherSettings
                {
                    Temperature = new Temperature
                    {
                        TemperatureCutoff = 70,
                        IsReminder = true,
                    },

                    Umbrella = new Umbrella
                    {
                        IsReminder = true,
                    },

                    Snow = new Snow
                    {
                        IsReminder = true
                    },
                    Reminders = $"0, 8:00 AM{Environment.NewLine}",
                    IsEmailNotification = true,
                    IsSmsNotification = true,
                    IsCellVerified = false
                },
                DailyForecast = dailyForecast,
                UserName = Input.Email,
                Email = Input.Email,
                CountryCode = Input.CountryCode,
                TimeZoneDifference = timeZoneDifference.ToString("hh\\:mm")
            };
            return user;
        }
    }
}
