using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;
using Twilio.Exceptions;
using WeatherReminder.Models.ApiModel.TextMessageApi.VerifyCellApi;
using WeatherReminder.Models.DataStorageModel;
using WeatherReminder.Models.WeatherReminderModel;

namespace WeatherReminder.Areas.Identity.Pages.Profile.EmailUser
{
    public class EnterVerificationCodeModel : PageModel
    {
        private readonly UserManager<WeatherReminderUser> _userManager;
        private readonly IVerifyCell _verifyCell;
        private readonly IWeatherReminderUserRepository _weatherReminderUserRepository;

        public EnterVerificationCodeModel(UserManager<WeatherReminderUser> userManager,
                                          IVerifyCell verifyCell,
                                          IWeatherReminderUserRepository weatherReminderUserRepository)
        {
            _userManager = userManager;
            _verifyCell = verifyCell;
            _weatherReminderUserRepository = weatherReminderUserRepository;
        }

        [Required]
        [BindProperty]
        public string VerificationCode { get; set; }

        [BindProperty]
        public bool? VerificationFailed { get; set; }

        [BindProperty]
        public int VerificationAttempts { get; set; }

        [BindProperty]
        public string CountryCode { get; set; }

        [BindProperty]
        public string PhoneNumber { get; set; }

        public IActionResult OnGet(string countryCode, string phoneNumber, int verificationAttempts, bool? verificationFailed)
        {
            CountryCode = countryCode;
            PhoneNumber = phoneNumber;
            VerificationAttempts = verificationAttempts;
            VerificationFailed = verificationFailed;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {

            ClaimsPrincipal currentUser = User;
            WeatherReminderUser user = _weatherReminderUserRepository.GetUser(currentUser);

            try
            {
                user.PhoneNumberConfirmed = _verifyCell.IsNumberValid(CountryCode, PhoneNumber, VerificationCode);
            }
            catch (ApiException e)
            {
                if (e.Message == "Invalid parameter: Code")
                {
                    if (VerificationAttempts == 5)
                    {
                        return RedirectToPage("/Account/Manage/UpdatePhoneInProfile", new
                        {
                            area = "Identity",
                            phoneVerificationFailed = true,
                            countryCode = CountryCode,
                            phoneNumber = PhoneNumber
                        });
                    }
                }

                if (e.Message == "Max check attempts reached")
                {
                    return RedirectToPage("/Account/Manage/UpdatePhoneInProfile", new
                    {
                        area = "Identity",
                        phoneVerificationFailed = true,
                        maxSendAttemptsApiError = true,
                        countryCode = CountryCode,
                        phoneNumber = PhoneNumber
                    });
                }

                return RedirectToPage("EnterVerificationCode", new
                {
                    countryCode = CountryCode,
                    phoneNumber = PhoneNumber,
                    verificationAttempts = VerificationAttempts++,
                    verificationFailed = true
                });
            }

            if (user.PhoneNumberConfirmed)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, PhoneNumber);

                if (!setPhoneResult.Succeeded)
                {
                    return RedirectToPage("/Account/Manage/UpdatePhoneInProfile", new
                    {
                        area = "Identity",
                        phoneVerificationFailed = true,
                        countryCode = CountryCode,
                        phoneNumber = PhoneNumber
                    });
                }

                user.CountryCode = CountryCode;
                user.PhoneNumberConfirmed = true;
                _weatherReminderUserRepository.SaveUser(user);
                return RedirectToPage("/Account/Manage/UpdatePhoneInProfile", new { area = "Identity", phoneVerificationSuccess = true });
            }
            else
            {
                if (VerificationAttempts == 5)
                {
                    return RedirectToPage("/Account/Manage/UpdatePhoneInProfile", new
                    {
                        area = "Identity",
                        phoneVerificationFailed = true,
                        countryCode = CountryCode,
                        phoneNumber = PhoneNumber
                    });
                }
                return RedirectToPage("EnterVerificationCode", new
                {
                    countryCode = CountryCode,
                    phoneNumber = PhoneNumber,
                    verificationAttempts = VerificationAttempts++,
                    verificationFailed = true
                });
            }

        }
    }
}
