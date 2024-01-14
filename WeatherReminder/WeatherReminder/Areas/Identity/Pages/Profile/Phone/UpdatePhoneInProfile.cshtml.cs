using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
using Twilio.Exceptions;
using WeatherReminder.Models.ApiModel.TextMessageApi.VerifyCellApi;
using WeatherReminder.Models.DataStorageModel;
using WeatherReminder.Models.WeatherReminderModel;

namespace WeatherReminder.Areas.Identity.Pages.Profile.Phone
{
    public class UpdatePhoneInProfileModel : PageModel
    {
        private readonly UserManager<WeatherReminderUser> _userManager;
        private readonly SignInManager<WeatherReminderUser> _signInManager;
        private readonly IVerifyCell _verifyCell;
        private readonly IWeatherReminderUserRepository _weatherReminderUserRepository;

        public UpdatePhoneInProfileModel(
            UserManager<WeatherReminderUser> userManager,
            SignInManager<WeatherReminderUser> signInManager,
            IVerifyCell verifyCell,
            IWeatherReminderUserRepository weatherReminderUserRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _verifyCell = verifyCell;
            _weatherReminderUserRepository = weatherReminderUserRepository;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        //[TempData]
        public string StatusMessage { get; set; }

        public bool? InvalidPhoneNumberApiError { get; set; }
        public bool? MaxSendAttemptsApiError { get; set; }
        public bool? PhoneVerificationFailed { get; set; }
        public bool? PhoneNumberConfirmed { get; set; }
        public bool? PhoneVerificationSuccess { get; set; }

        public class InputModel
        {
            [Display(Name = "Country Code")]
            public string CountryCode { get; set; }

            [Phone]
            [Display(Name = "Cell Number")]
            public string PhoneNumber { get; set; }
        }

        private async Task LoadAsync(WeatherReminderUser user, string updatedCountryCode, string updatedPhoneNumber)
        {
            var databasePhoneNumber = await _userManager.GetPhoneNumberAsync(user);
            var databaseCountryCode = _weatherReminderUserRepository.GetUser(User).CountryCode;

            if (updatedPhoneNumber == null)
            {
                Input = new InputModel
                {
                    CountryCode = databaseCountryCode,
                    PhoneNumber = databasePhoneNumber
                };
            }
            else
            {
                Input = new InputModel
                {
                    CountryCode = updatedCountryCode,
                    PhoneNumber = updatedPhoneNumber
                };
            }
            PhoneNumberConfirmed = user.PhoneNumberConfirmed;
        }


        public async Task<IActionResult> OnGetAsync(bool? invalidPhoneNumberApiError,
                                                    bool? maxSendAttemptsApiError,
                                                    bool? phoneVerificationFailed,
                                                    bool? phoneVerificationSuccess,
                                                    string countryCode,
                                                    string phoneNumber)
        {
            var user = _weatherReminderUserRepository.GetUser(User);

            InvalidPhoneNumberApiError = invalidPhoneNumberApiError;
            MaxSendAttemptsApiError = maxSendAttemptsApiError;
            PhoneVerificationFailed = phoneVerificationFailed;
            PhoneNumberConfirmed = user.PhoneNumberConfirmed;
            PhoneVerificationSuccess = phoneVerificationSuccess;

            if (phoneVerificationSuccess == true)
            {
                StatusMessage = "Your phone number has been verified";
            }

            if (user.PhoneNumberConfirmed == false && phoneVerificationFailed == true)
            {
                StatusMessage = "Error: Your phone number could not be verified";
            }

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user, countryCode, phoneNumber);

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
                await LoadAsync(user, null, null);
                return Page();
            }


            if (ModelState.IsValid)
            {

                await _userManager.UpdateAsync(user);

                await _signInManager.RefreshSignInAsync(user);
                //return RedirectToPage();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            /////////////////////////////////////*******************
            if (Input.PhoneNumber != phoneNumber)
            {
                try
                {
                    _verifyCell.SendVerificationCode(Input.CountryCode, Input.PhoneNumber);
                }
                catch (ApiException e)
                {
                    if (e.Message == "Max send attempts reached")
                    {
                        MaxSendAttemptsApiError = true;
                        return RedirectToPage("UpdatePhoneInProfile", new { maxSendAttemptsApiError = true, phone = Input.PhoneNumber });
                        //You have attempted to send the verification code more than 5 times and have reached the limit. You have to wait 10 minutes for verification to expire.
                    }

                    if (e.Message.Contains("Invalid parameter `To`:"))
                    {
                        InvalidPhoneNumberApiError = true;
                        return RedirectToPage("UpdatePhoneInProfile", new { invalidPhoneNumberApiError = true, phone = Input.PhoneNumber });
                    }
                }

                return RedirectToPage("EnterVerificationCode", new { phone = Input.PhoneNumber });
            }
            return RedirectToPage();
        }
    }
}




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