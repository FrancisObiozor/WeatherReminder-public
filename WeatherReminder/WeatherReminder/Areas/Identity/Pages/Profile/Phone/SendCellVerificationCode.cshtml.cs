using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Twilio.Exceptions;
using WeatherReminder.Models.ApiModel.TextMessageApi.VerifyCellApi;
using WeatherReminder.Models.DataStorageModel;
using WeatherReminder.Models.WeatherReminderModel;

namespace WeatherReminder.Areas.Identity.Pages.Profile.Phone
{
    public class SendCellVerificationCodeModel : PageModel
    {
        private readonly IWeatherReminderUserRepository _weatherReminderUserRepository;
        private readonly IVerifyCell _verifyCell;

        public SendCellVerificationCodeModel(IWeatherReminderUserRepository weatherReminderUserRepository,
                                             IVerifyCell verifyCell)
        {
            _weatherReminderUserRepository = weatherReminderUserRepository;
            _verifyCell = verifyCell;
        }

        public IActionResult OnGet()
        {
            WeatherReminderUser user = _weatherReminderUserRepository.GetUser(User);

            if (!string.IsNullOrEmpty(user.PhoneNumber) && !user.PhoneNumberConfirmed)
            {
                try
                {
                    _verifyCell.SendVerificationCode(user.CountryCode, user.PhoneNumber);
                }
                catch (ApiException e)
                {
                    if (e.Message.Contains("Max send attempts reached"))
                    {

                        return RedirectToPage("/Account/Manage/UpdatePhoneInProfile", new { area = "Identity", maxSendAttemptsApiError = true });
                        //You have attempted to send the verification code more than 5 times and have reached the limit. You have to wait 10 minutes for verification to expire.
                    }

                    if (e.Message.Contains("Invalid parameter `To`:"))
                    {
                        return RedirectToPage("/Account/Manage/UpdatePhoneInProfile", new { area = "Identity", invalidPhoneNumberApiError = true, phoneVerificationFailed = true });
                    }
                }
                return RedirectToPage("/Account/Manage/EnterVerificationCode", new { area = "Identity", countryCode = user.CountryCode, phoneNumber = user.PhoneNumber });

            }
            return RedirectToPage("/Account/Manage/UpdatePhoneInProfile", new { area = "Identity" });
        }
    }
}

